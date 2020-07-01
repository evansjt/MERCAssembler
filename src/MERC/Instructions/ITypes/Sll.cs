using System;
namespace MERC.Instructions.ITypes
{
    public class Sll : ITypeInstruction
    {
        public Sll()
        {
            this.name = "sll";
            this.opCode = 16;
        }

        protected override void Execute()
        {
            Globals.Pc = this.newPc;
            var a = this.rs.Value;
            var shamt = this.imm;
            this.result = a << shamt;
            MemoryStoreAndWrite();
        }
    }
}
