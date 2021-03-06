﻿using System;
namespace MERC.Instructions.ITypes
{
    public class Bget : BranchInstruction
    {
        public Bget()
        {
            this.name = "bget";
            this.opCode = 21;
        }

        protected override void Execute()
        {
            int a = this.rd.Value;
            int b = this.rs.Value;
            Globals.Pc = (a >= b) ? (this.newPc + this.imm) : this.newPc;
        }
    }
}
