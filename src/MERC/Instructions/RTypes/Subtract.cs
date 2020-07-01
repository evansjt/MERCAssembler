using System;
namespace MERC.Instructions.RTypes
{
    public class Subtract : RTypeInstruction
    {
        public Subtract()
        {
            this.name = "sub";
            this.opCode = 2;
        }

        protected override void Execute()
        {
            base.Execute();
            int a = this.rs.Value;
            int b = this.rt.Value;
            this.result = a - b;
            MemoryStoreAndWrite();
        }
    }
}
