using System;
namespace MERC.Instructions.LTypes
{
    public class LoadLowerImm : LTypeInstruction
    {
        public LoadLowerImm()
        {
            this.name = "lli";
            this.opCode = 8;
        }

        protected override void Execute()
        {
            Globals.Pc++;
            var a = this.rd.Value;
            this.result = a ^ this.imm;
            MemoryStoreAndWrite();
        }
    }
}
