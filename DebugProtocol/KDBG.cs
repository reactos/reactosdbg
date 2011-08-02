using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using AbstractPipe;
using DebugProtocol;
using System.Diagnostics;

namespace KDBGProtocol
{
    public class KDBG : IDebugProtocol
    {
        #region Regular expressions
        static Regex mMemoryRowUpdate = new Regex("<(?<addr>[^>]+)>: (?<row>[0-9a-fA-F? ]*)");
        static Regex mModuleOffset = new Regex("(?<modname>[^:]+):(?<offset>[0-9a-fA-f]+).*");
        static Regex mModuleUpdate = new Regex("(?<base>[0-9a-fA-F]+)  (?<size>[0-9a-fA-F]+)  (?<modname>\\w+\\.\\w+)");
        static Regex mRegLineCS_EIP = new Regex("CS:EIP  0x(?<cs>[0-9a-fA-F]+):0x(?<eip>[0-9a-fA-F]+).*");
        static Regex mRegLineSS_ESP = new Regex("SS:ESP  0x(?<ss>[0-9a-fA-F]+):0x(?<esp>[0-9a-fA-F]+).*");
        static Regex mRegLineEAX_EBX = new Regex("EAX  0x(?<eax>[0-9a-fA-F]+)[ \t]+EBX  0x(?<ebx>[0-9a-fA-F]+).*");
        static Regex mRegLineECX_EDX = new Regex("ECX  0x(?<ecx>[0-9a-fA-F]+)[ \t]+EDX  0x(?<edx>[0-9a-fA-F]+).*");
        static Regex mRegLineEBP = new Regex("EBP  0x(?<ebp>[0-9a-fA-F]+).*");
        static Regex mRegLineEFLAGS = new Regex("EFLAGS  0x(?<eflags>[0-9a-fA-F]+).*");
        static Regex mSregLine = new Regex("[CDEFGS]S  0x(?<seg>[0-9a-fA-F]+).*");
        static Regex mProcListHeading = new Regex("PID[ \\t]+State[ \\t]+Filename.*");
        static Regex mThreadListHeading = new Regex("TID[ \\t]+State[ \\t]+Prio.*");
        static Regex mProcListEntry = new Regex("^(?<cur>([*]|))0x(?<pid>[0-9a-fA-F]+)[ \\t](?<state>[a-zA-Z ]+)[ \\t](?<name>[a-zA-Z. ]+).*");
        static Regex mThreadListEntry = new Regex("^(?<cur>([*]|))0x(?<tid>[0-9a-fA-F]+)[ \\t]+(?<state>.*)0x(?<eip>[0-9a-fA-F]*)");
        static Regex mBreakpointListEntry = new Regex("^\\*?(?<id>[0-9]{3})[ \\t]+(?<type>BP[X|M])[ \\t]+0x(?<address>[0-9a-fA-F]+)(?<rest>.+)");
        #endregion

        #region Fields
        bool mRunning = true;
        Pipe mConnection;
        Queue<string> mCommandBuffer = new Queue<string>();
        Dictionary<string, ulong> mModuleList = new Dictionary<string, ulong>();
        ulong[] mRegisters = new ulong[32];
        IList<Breakpoint> mBreakpoints = new List<Breakpoint>();

        bool mFirstModuleUpdate = false;
        bool mReceivingProcs = false;
        bool mReceivingThreads = false;
        bool mReceivingBreakpoints = false;
        StringBuilder mInputBuffer = new StringBuilder();
        int mUsedInput;
        #endregion

        #region Events
        public event ConsoleOutputEventHandler ConsoleOutputEvent;
        public event RegisterChangeEventHandler RegisterChangeEvent;
        public event BreakpointChangeEventHandler BreakpointChangeEvent;
        public event SignalDeliveredEventHandler SignalDeliveredEvent;
        public event RemoteGDBErrorHandler RemoteGDBError;
        public event MemoryUpdateEventHandler MemoryUpdateEvent;
        public event ModuleListEventHandler ModuleListEvent;
        public event ProcessListEventHandler ProcessListEvent;
        public event ThreadListEventHandler ThreadListEvent;
        #endregion

        public KDBG(Pipe connection)
        { 
            mConnection = connection;
            mConnection.PipeReceiveEvent += PipeReceiveEvent;
            mConnection.PipeErrorEvent += PipeErrorEvent;
        }

        #region Event handlers
        void PipeErrorEvent(object sender, PipeErrorEventArgs args)
        {
        }

        void PipeReceiveEvent(object sender, PipeReceiveEventArgs args)
        {
            bool tookText = false;
            string inbufStr;
            int promptIdx;

            mInputBuffer.Append(args.Received);

            if (mInputBuffer.ToString().Substring(mUsedInput).Contains("key to continue"))
            {
                mConnection.Write("\r");
                mUsedInput = mInputBuffer.Length;
            }

            while ((promptIdx = (inbufStr = mInputBuffer.ToString()).IndexOf("kdb:> ")) != -1)
            {
                string pretext = inbufStr.Substring(0, promptIdx);
                string[] theInput = pretext.Split(new char[] { '\n' });
                int remove = pretext.Length + "kdb:> ".Length;

                mInputBuffer.Remove(0, remove);
                mUsedInput = Math.Max(0, mUsedInput - remove);

                if (!tookText)
                {
                    if (mRunning)
                    {
                        mRunning = false;
                    }
                    tookText = true;
                }

                foreach (string line in theInput)
                {
                    string cleanedLine = line.Trim();
                    try 
                    {
                        if (cleanedLine.Contains("Entered debugger on "))
                        {
                            mReceivingProcs = false;
                            mReceivingThreads = false;
                            GetRegisterUpdate();
                            GetProcesses();
                            continue;
                        }

                        if (!mFirstModuleUpdate)
                        {
                            GetModuleUpdate();
                            mFirstModuleUpdate = true;
                        }

                        if (TryParseMemory(cleanedLine)) continue;
                        if (TryParseModule(cleanedLine)) continue;
                        if (TryParseRegisters(cleanedLine)) continue;
                        if (TryParseSegmentRegisters(cleanedLine)) continue;
                        if (TryParsePID(cleanedLine)) continue;
                        if (TryParseTID(cleanedLine)) continue;
                        if (TryParseBreakpoint(cleanedLine)) continue;
                    }
                    catch (Exception) { /* Error line ... we'll ignore it for now */ }
                }
            }

            if (tookText)
            {
                lock (mCommandBuffer)
                {
                    if (mCommandBuffer.Count > 0)
                    {
                        string firstCommand = mCommandBuffer.Dequeue();
                        mConnection.Write(firstCommand + "\r");
                    }
                }
            }
        }
        #endregion

        #region Individual parsing methods, each one looking for a specific item
        private bool TryParseMemory(string cleanedLine)
        {
            Match memoryMatch = mMemoryRowUpdate.Match(cleanedLine);
            if (memoryMatch.Success)
            {
                string addrStr = memoryMatch.Groups["addr"].ToString();
                Match modOffset = mModuleOffset.Match(addrStr);
                ulong updateAddress;
                if (modOffset.Success)
                {
                    string modname = modOffset.Groups["modname"].ToString();
                    ulong offset = ulong.Parse(modOffset.Groups["offset"].ToString(), NumberStyles.HexNumber);
                    ulong modbase;
                    if (mModuleList.TryGetValue(modname.ToUpper(), out modbase))
                        updateAddress = modbase + offset;
                    else
                        return true; // Couldn't resolve the address of the named module ...
                }
                else
                {
                    updateAddress = ulong.Parse(addrStr, NumberStyles.HexNumber);
                }
                string[] memWords = memoryMatch.Groups["row"].ToString().Split(new char[] { ' ' });
                byte[] updateBytes = new byte[4 * memWords.Length];
                int ctr = 0;
                foreach (string word in memWords)
                {
                    if (word[0] == '?')
                    {
                        if (MemoryUpdateEvent != null)
                            MemoryUpdateEvent(this, new MemoryUpdateEventArgs((updateAddress & ~0xfffUL), null));
                    }
                    else
                    {
                        int wordParsed = int.Parse(word, NumberStyles.HexNumber);
                        int curCtr = ctr;
                        for (ctr = curCtr; ctr < curCtr + 4; ctr++)
                        {
                            updateBytes[ctr] = (byte)(wordParsed & 0xff);
                            wordParsed >>= 8;
                        }
                    }
                }
                if (MemoryUpdateEvent != null)
                    MemoryUpdateEvent(this, new MemoryUpdateEventArgs(updateAddress, updateBytes));

                return true;
            }
            return false;
        }

        private bool TryParseModule(string cleanedLine)
        {
            Match moduleMatch = mModuleUpdate.Match(cleanedLine);
            if (moduleMatch.Success)
            {
                ulong baseAddress = ulong.Parse(moduleMatch.Groups["base"].ToString(), NumberStyles.HexNumber);
                uint moduleSize = uint.Parse(moduleMatch.Groups["size"].ToString(), NumberStyles.HexNumber);
                string moduleName = moduleMatch.Groups["modname"].ToString();
                mModuleList[moduleName.ToUpper()] = baseAddress;
                if (ModuleListEvent != null)
                    ModuleListEvent(this, new ModuleListEventArgs(moduleName, baseAddress));
                return true;
            }
            return false;
        }

        private bool TryParseRegisters(string cleanedLine)
        {
            Match csEipMatch = mRegLineCS_EIP.Match(cleanedLine);
            if (csEipMatch.Success)
            {
                uint cs = uint.Parse(csEipMatch.Groups["cs"].ToString(), NumberStyles.HexNumber);
                ulong eip = ulong.Parse(csEipMatch.Groups["eip"].ToString(), NumberStyles.HexNumber);
                mRegisters[8] = eip;
                mRegisters[10] = cs;
                return true;
            }

            Match ssEspMatch = mRegLineSS_ESP.Match(cleanedLine);
            if (ssEspMatch.Success)
            {
                uint ss = uint.Parse(ssEspMatch.Groups["ss"].ToString(), NumberStyles.HexNumber);
                ulong esp = ulong.Parse(ssEspMatch.Groups["esp"].ToString(), NumberStyles.HexNumber);
                mRegisters[4] = esp;
                mRegisters[15] = ss;
                return true;
            }

            Match eaxEbxMatch = mRegLineEAX_EBX.Match(cleanedLine);
            if (eaxEbxMatch.Success)
            {
                ulong eax = ulong.Parse(eaxEbxMatch.Groups["eax"].ToString(), NumberStyles.HexNumber);
                ulong ebx = ulong.Parse(eaxEbxMatch.Groups["ebx"].ToString(), NumberStyles.HexNumber);
                mRegisters[0] = eax;
                mRegisters[3] = ebx;
                return true;
            }

            Match ecxEdxMatch = mRegLineECX_EDX.Match(cleanedLine);
            if (ecxEdxMatch.Success)
            {
                ulong ecx = ulong.Parse(ecxEdxMatch.Groups["ecx"].ToString(), NumberStyles.HexNumber);
                ulong edx = ulong.Parse(ecxEdxMatch.Groups["edx"].ToString(), NumberStyles.HexNumber);
                mRegisters[1] = ecx;
                mRegisters[2] = edx;
                return true;
            }

            Match ebpMatch = mRegLineEBP.Match(cleanedLine);
            if (ebpMatch.Success)
            {
                ulong ebp = ulong.Parse(ebpMatch.Groups["ebp"].ToString(), NumberStyles.HexNumber);
                mRegisters[5] = ebp;
                return true;
            }

            Match eflagsMatch = mRegLineEFLAGS.Match(cleanedLine);
            if (eflagsMatch.Success)
            {
                ulong eflags = ulong.Parse(eflagsMatch.Groups["eflags"].ToString(), NumberStyles.HexNumber);
                mRegisters[9] = eflags;
                if (RegisterChangeEvent != null)
                    RegisterChangeEvent(this, new RegisterChangeEventArgs(mRegisters));
                return true;
            }
            return false;
        }

        private bool TryParseSegmentRegisters(string cleanedLine)
        {
            Match sregMatch = mSregLine.Match(cleanedLine);
            if (sregMatch.Success)
            {
                char[] segmap = new char[] { 'C', 'D', 'E', 'F', 'G', 'S' };
                uint sreg = uint.Parse(sregMatch.Groups["seg"].ToString(), NumberStyles.HexNumber);
                int findSeg;
                for (findSeg = 0; findSeg < segmap.Length; findSeg++)
                {
                    if (segmap[findSeg] == cleanedLine[0])
                    {
                        mRegisters[10 + findSeg] = sreg;
                        if (segmap[findSeg] == 'S' && RegisterChangeEvent != null)
                            RegisterChangeEvent(this, new RegisterChangeEventArgs(mRegisters));
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool TryParsePID(string cleanedLine)
        {
            Match pidHeadMatch = mProcListHeading.Match(cleanedLine);
            if (pidHeadMatch.Success)
            {
                mReceivingThreads = false;
                mReceivingProcs = true;
                if (ProcessListEvent != null)
                    ProcessListEvent(this, new ProcessListEventArgs());
                return true;
            }
            else
            {
                Match pidEntryMatch = mProcListEntry.Match(cleanedLine);
                if (pidEntryMatch.Success && mReceivingProcs)
                {
                    if (ProcessListEvent != null)
                        ProcessListEvent(this, new ProcessListEventArgs(ulong.Parse(pidEntryMatch.Groups["pid"].ToString(), NumberStyles.HexNumber), pidEntryMatch.Groups["cur"].Length > 0,
                            pidEntryMatch.Groups["state"].ToString(), pidEntryMatch.Groups["name"].ToString()));
                    return true;
                }
                else
                {
                    if ((mReceivingProcs || cleanedLine.Contains("No processes")) && ProcessListEvent != null)
                    {
                        ProcessListEvent(this, new ProcessListEventArgs(true));
                        mReceivingProcs = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryParseTID(string cleanedLine)
        {
            Match tidHeadMatch = mThreadListHeading.Match(cleanedLine);
            if (tidHeadMatch.Success)
            {
                mReceivingThreads = true;
                mReceivingProcs = false;
                if (ThreadListEvent != null)
                    ThreadListEvent(this, new ThreadListEventArgs());
                return true;
            }
            else
            {
                Match tidEntryMatch = mThreadListEntry.Match(cleanedLine);
                if (tidEntryMatch.Success && mReceivingThreads)
                {
                    if (ThreadListEvent != null)
                        ThreadListEvent(this, new ThreadListEventArgs(ulong.Parse(tidEntryMatch.Groups["tid"].ToString(), NumberStyles.HexNumber), tidEntryMatch.Groups["cur"].Length > 0, ulong.Parse(tidEntryMatch.Groups["eip"].ToString(), NumberStyles.HexNumber)));
                    return true;
                }
                else
                {
                    if (mReceivingThreads && ThreadListEvent != null)
                    {
                        ThreadListEvent(this, new ThreadListEventArgs(true));
                        mReceivingThreads = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryParseBreakpoint(string cleanedLine)
        {
            if (cleanedLine.Equals("Breakpoints:")) // Start looking for breakpoint data
            {
                mReceivingBreakpoints = true;
                return true;
            }
            else if (cleanedLine.Equals("No breakpoints.")) // Nothing present
            {
                mReceivingBreakpoints = false;
                mBreakpoints.Clear();
                if (BreakpointChangeEvent != null)
                    BreakpointChangeEvent(this, new BreakpointChangeEventArgs(mBreakpoints));
                return true;
            }
            else if (mReceivingBreakpoints)
            {
                Match breakpointMatch = mBreakpointListEntry.Match(cleanedLine);
                if (breakpointMatch.Success)
                {
                    // Process breakpoint data
                    int id = int.Parse(breakpointMatch.Groups["id"].ToString(), NumberStyles.Integer);
                    Breakpoint bp = new Breakpoint(id);

                    string type = breakpointMatch.Groups["type"].ToString();
                    bp.Address = ulong.Parse(breakpointMatch.Groups["address"].ToString(), NumberStyles.HexNumber);
                    bp.Enabled = true;
                    bp.Condition = string.Empty;

                    string rest = breakpointMatch.Groups["rest"].ToString();
                    int cond_start = rest.LastIndexOf("IF");
                    if (cond_start >= 0) // Separate the condition from the other properties
                    {
                        bp.Condition = rest.Substring(cond_start + 2).Trim();
                        rest = rest.Substring(0, cond_start);
                    }

                    string[] properties = rest.Split(' ');
                    foreach (string property in properties)
                    {
                        if (type.Equals("BPM"))
                        {
                            if (property.Equals("read"))
                                bp.BreakpointType = Breakpoint.BPType.ReadWatch;
                            else if (property.Equals("write"))
                                bp.BreakpointType = Breakpoint.BPType.WriteWatch;
                            else if (property.Equals("rdwr"))
                                bp.BreakpointType = Breakpoint.BPType.AccessWatch;
                            else if (property.Equals("exec"))
                                bp.BreakpointType = Breakpoint.BPType.Hardware;
                            else if (property.Equals("byte"))
                                bp.Length = 1;
                            else if (property.Equals("word"))
                                bp.Length = 2;
                            else if (property.Equals("dword"))
                                bp.Length = 4;
                        }
                        else if (type.Equals("BPX"))
                        {
                            bp.BreakpointType = Breakpoint.BPType.Software;
                        }

                        // Common properties
                        if (property.Equals("disabled"))
                            bp.Enabled = false;
                    }

                    mBreakpoints.Add(bp);
                    return true;
                }
                else // Something else; it implies that we've got all the breakpoint data
                {
                    if (BreakpointChangeEvent != null)
                        BreakpointChangeEvent(this, new BreakpointChangeEventArgs(mBreakpoints));
                    mBreakpoints.Clear();
                    mReceivingBreakpoints = false;
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region Upstream commands
        public void QueueCommand(string command)
        {
            lock (mCommandBuffer)
            {
                mCommandBuffer.Enqueue(command);
            }
        }

        public void Write(string wr)
        {
            /* Forward user input from RawTraffic if connected to kdbg */
            if (!mRunning)
            {
                mConnection.Write(wr + "\r");
            }
        }

        public void Close()
        {
        }

        #endregion

        #region Immediately executed commands
        public void Step()
        {
            Write("step");
            GetRegisterUpdate();
            GetModuleUpdate();
            GetProcesses();
        }

        public void Next()
        {
            Write("next");
            Thread.Sleep(100); 
            GetRegisterUpdate();
            GetModuleUpdate();
            GetProcesses();
        }

        public void Break()
        {
            Write("");
            GetRegisterUpdate();
            GetModuleUpdate();
        }

        public void Go(ulong address)
        {
            Write("cont");
            mRunning = true;
            mFirstModuleUpdate = false;
        }

        public void GetBreakpoints()
        {
            Write("bl");
        }

        public void SetBreakpoint(Breakpoint bp)
        {
            string length = "byte";
            if (bp.Length == 2) length = "word";
            else if (bp.Length == 4) length = "dword";

            StringBuilder command = new StringBuilder();

            switch (bp.BreakpointType)
            {
                case Breakpoint.BPType.Software:
                    command.AppendFormat("bpx 0x{0:x8}", bp.Address);
                    break;
                case Breakpoint.BPType.Hardware:
                    command.AppendFormat("bpm x {0} 0x{1:x8}", "byte", bp.Address);
                    break;
                case Breakpoint.BPType.ReadWatch:
                    command.AppendFormat("bpm r {0} 0x{1:x8}", length, bp.Address);
                    break;
                case Breakpoint.BPType.WriteWatch:
                    command.AppendFormat("bpm w {0} 0x{1:x8}", length, bp.Address);
                    break;
                case Breakpoint.BPType.AccessWatch:
                    command.AppendFormat("bpm rw {0} 0x{1:x8}", length, bp.Address);
                    break;
            }

            if (!string.IsNullOrEmpty(bp.Condition))
                command.AppendFormat(" IF {0}", bp.Condition);

            Write(command.ToString());
            GetBreakpoints();
        }

        public void RemoveBreakpoint(int id)
        {
            if (id >= 0)
                Write("bc " + id);
            GetBreakpoints();
        }

        public void EnableBreakpoint(int id)
        {
            if (id >= 0)
                Write("be " + id);
            GetBreakpoints();
        }

        public void DisableBreakpoint(int id)
        {
            if (id >= 0)
                Write("bd " + id);
            GetBreakpoints();
        }

        public void WriteMemory(ulong address, byte[] buf)
        {
        }

        public void GetMemoryUpdate(ulong address, int len)
        {
            Write(string.Format("x 0x{0:X} L {1}", address, len));
        }
        #endregion

        #region Commands placed into the command queue
        public void GetProcesses()
        {
            QueueCommand("proc list");
        }

        public void GetThreads(ulong pid)
        {
            if (pid != 0)
                QueueCommand(string.Format("thread list 0x{0:X8}", pid));
        }

        public void SetProcess(ulong pid)
        {
            QueueCommand(string.Format("proc attach 0x{0:X8}", pid));
            GetRegisterUpdate();
        }

        public void SetThread(ulong tid)
        {
            QueueCommand(string.Format("thread attach 0x{0:X8}", tid));
            GetRegisterUpdate();
        }

        public void GetRegisterUpdate()
        {
            QueueCommand("regs");
            QueueCommand("sregs");
        }

        public void GetModuleUpdate()
        {
            QueueCommand("mod");
        }

        #endregion
    }
}
