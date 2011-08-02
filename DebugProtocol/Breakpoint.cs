using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugProtocol
{
    public class Breakpoint
    {
        public enum BPType { Software, Hardware, WriteWatch, ReadWatch, AccessWatch };

        public int ID { get; set; }
        public BPType BreakpointType { get; set; }
        public ulong Address { get; set; }
        public int Length { get; set; }
        public string Condition { get; set; }
        public bool Enabled { get; set; }

        public Breakpoint(int id)
        {
            ID = id;
            BreakpointType = BPType.Software;
            Address = 0;
            Length = 1;
            Condition = string.Empty;
            Enabled = false;
        }

        public Breakpoint(int id, BPType type, ulong addr, int len, string cond)
        {
            ID = id;
            BreakpointType = type;
            Address = addr;
            Length = len;
            Condition = cond;
            Enabled = false;
        }

        //TODO: include condition/enabled in hashcode?
        public override int GetHashCode()
        {
            return (int)(((int)BreakpointType) ^ (int)Address ^ (Length << 28));
        }

        public override bool Equals(object other)
        {
            Breakpoint otherbp = other as Breakpoint;
            if (otherbp == null) return false;
            return
                otherbp.ID == ID;/*
                (otherbp.BreakpointType == BreakpointType) &&
                (otherbp.Address == Address) &&
                (otherbp.Length == Length);*/
        }
    }
}
