using System;
namespace MERC.Instructions.ITypes
{
    public class Srl : ITypeInstruction
    {
        public Srl()
        {
            this.name = "srl";
            this.opCode = 17;
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
