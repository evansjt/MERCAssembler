using System;
namespace MERC.Instructions.ITypes
{
    public class Blt : BranchInstruction
    {
        public Blt()
        {
            this.name = "blt";
            this.opCode = 18;
        }

        protected override void Execute()
        {
            int a = this.rd.Value;
            int b = this.rs.Value;
            Globals.Pc = (a < b) ? (this.newPc + this.imm) : this.newPc;
        }
    }
}
