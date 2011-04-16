using System;
using System.Collections.Generic;
using System.Text;

namespace RosDBG
{
    /// <summary>
    /// Record of X86 registers, stored in stateful variables.
    /// Individual flag data is automatically extracted from the appropriate bits of the Flags register.
    /// </summary>
    class StatefulX86Registers
    {
        public StatefulVariable<UInt32> EAX, EBX, ECX, EDX, ESI, EDI;
        public StatefulVariable<UInt32> EBP, ESP, EIP;
        public StatefulVariable<UInt16> CS, DS, SS, ES, FS, GS;
        public StatefulVariable<UInt32> Flags;
        public StatefulVariable<bool> CarryFlag, ParityFlag, AdjustFlag, ZeroFlag, SignFlag, // Bits 0..7
            TrapFlag, InterruptEnableFlag, DirectionFlag, OverflowFlag, NestedTaskFlag, // Bits 8..15
            ResumeFlag, Virtual8086ModeFlag, AlignmentCheck, VirtualInterruptFlag, VirtualInterruptPending, AllowCPUID; // Bits 16..21
        public StatefulVariable<byte> IOPrivilegeLevel; // Bits 12 and 13

        [Flags]
        public enum FlagMask
        {
            CarryFlag = 1 << 0,
            ParityFlag = 1 << 2,
            AdjustFlag = 1 << 4,
            ZeroFlag = 1 << 6,
            SignFlag = 1 << 7,
            TrapFlag = 1 << 8,
            InterruptEnableFlag = 1 << 9,
            DirectionFlag = 1 << 10,
            OverflowFlag = 1 << 11,
            IOPrivilegeLevel = 3 << 12,
            NestedTaskFlag = 1 << 14,
            ResumeFlag = 1 << 16,
            V8086ModeFlag = 1 << 17,
            AlignmentCheck = 1 << 18,
            VirtualInterruptFlag = 1 << 19,
            VirtualInterruptPending = 1 << 20,
            AllowCPUID = 1 << 21
        }

        public StatefulX86Registers()
        {
            EAX = new StatefulVariable<UInt32>();
            EBX = new StatefulVariable<UInt32>();
            ECX = new StatefulVariable<UInt32>();
            EDX = new StatefulVariable<UInt32>();
            ESI = new StatefulVariable<UInt32>();
            EDI = new StatefulVariable<UInt32>();
            
            EBP = new StatefulVariable<UInt32>();
            ESP = new StatefulVariable<UInt32>();
            EIP = new StatefulVariable<UInt32>();
            
            CS = new StatefulVariable<UInt16>();
            DS = new StatefulVariable<UInt16>();
            SS = new StatefulVariable<UInt16>();
            ES = new StatefulVariable<UInt16>();
            FS = new StatefulVariable<UInt16>();
            GS = new StatefulVariable<UInt16>();

            Flags = new StatefulVariable<UInt32>();
            
            CarryFlag = new StatefulVariable<bool>();
            ParityFlag = new StatefulVariable<bool>();
            AdjustFlag = new StatefulVariable<bool>();
            ZeroFlag = new StatefulVariable<bool>();
            SignFlag = new StatefulVariable<bool>();
            TrapFlag = new StatefulVariable<bool>();
            InterruptEnableFlag = new StatefulVariable<bool>();
            DirectionFlag = new StatefulVariable<bool>();
            OverflowFlag = new StatefulVariable<bool>();
            NestedTaskFlag = new StatefulVariable<bool>();
            ResumeFlag = new StatefulVariable<bool>();
            Virtual8086ModeFlag = new StatefulVariable<bool>();
            AlignmentCheck = new StatefulVariable<bool>();
            VirtualInterruptFlag = new StatefulVariable<bool>();
            VirtualInterruptPending = new StatefulVariable<bool>();
            AllowCPUID = new StatefulVariable<bool>();

            IOPrivilegeLevel = new StatefulVariable<byte>();

            Flags.Updated += new EventHandler(Flags_OnUpdate);
        }

        // Cascade the individual bits of the flags register towards the respective variables
        private void Flags_OnUpdate(object sender, EventArgs e)
        {
            CarryFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.CarryFlag) != 0);
            ParityFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.ParityFlag) != 0);
            AdjustFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.AdjustFlag) != 0);
            ZeroFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.ZeroFlag) != 0);
            SignFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.SignFlag) != 0);
            TrapFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.TrapFlag) != 0);
            InterruptEnableFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.InterruptEnableFlag) != 0);
            DirectionFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.DirectionFlag) != 0);
            OverflowFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.OverflowFlag) != 0);
            NestedTaskFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.NestedTaskFlag) != 0);
            ResumeFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.ResumeFlag) != 0);
            Virtual8086ModeFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.V8086ModeFlag) != 0);
            AlignmentCheck.Set((Flags.CurrentValue & (UInt32)FlagMask.AlignmentCheck) != 0);
            VirtualInterruptFlag.Set((Flags.CurrentValue & (UInt32)FlagMask.VirtualInterruptFlag) != 0);
            VirtualInterruptPending.Set((Flags.CurrentValue & (UInt32)FlagMask.VirtualInterruptPending) != 0);
            AllowCPUID.Set((Flags.CurrentValue & (UInt32)FlagMask.AllowCPUID) != 0);

            UInt32 iopl = (Flags.CurrentValue & (UInt32)FlagMask.IOPrivilegeLevel) >> 12;
            IOPrivilegeLevel.Set((byte)iopl);
        }
    }
}
