using System;
namespace MERC.Instructions.ITypes
{
    public class Blet : BranchInstruction
    {
        public Blet()
        {
            this.name = "blet";
            this.opCode = 20;
        }

        protected override void Execute()
        {
            int a = this.rd.Value;
            int b = this.rs.Value;
            Globals.Pc = (a <= b) ? (this.newPc + this.imm) : this.newPc;
        }
    }
}
