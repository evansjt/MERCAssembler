using System;
namespace MERC.Instructions.LTypes
{
    public class LoadUpperImm : LTypeInstruction
    {
        public LoadUpperImm()
        {
            this.name = "lui";
            this.opCode = 9;
        }

        protected override void Execute()
        {
            Globals.Pc++;
            int upper = this.imm << 8;
            this.result = upper ^ this.rd.Value;
            MemoryStoreAndWrite();
        }
    }
}
