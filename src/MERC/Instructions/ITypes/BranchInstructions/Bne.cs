using System;
namespace MERC.Instructions.ITypes
{
    public class Bne : BranchInstruction
    {
        public Bne()
        {
            this.name = "bne";
            this.opCode = 6;
        }

        protected override void Execute()
        {
            string a = Convert.ToString(this.rd.Value, 2).PadLeft(16, '0');
            string b = Convert.ToString(this.rs.Value, 2).PadLeft(16, '0');
            a = a.Substring(a.Length - 8);
            b = b.Substring(b.Length - 8);
            Globals.Pc = (a != b) ? (this.newPc + this.imm) : this.newPc;
        }
    }
}
