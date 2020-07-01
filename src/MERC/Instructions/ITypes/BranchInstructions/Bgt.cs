using System;
namespace MERC.Instructions.ITypes
{
    public class Bgt : BranchInstruction
    {
        public Bgt()
        {
            this.name = "bgt";
            this.opCode = 19;
        }

        protected override void Execute()
        {
            int a = this.rd.Value;
            int b = this.rs.Value;
            Globals.Pc = (a > b) ? (this.newPc + this.imm) : this.newPc;
        }
    }
}
