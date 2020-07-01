using System;
namespace MERC.Instructions.RTypes
{
    public class Add : RTypeInstruction
    {
        public Add()
        {
            this.name = "add";
            this.opCode = 0;
        }

        protected override void Execute()
        {
            base.Execute();
            int a = this.rs.Value;
            int b = this.rt.Value;
            this.result = a + b;
            MemoryStoreAndWrite();
        }
    }
}
