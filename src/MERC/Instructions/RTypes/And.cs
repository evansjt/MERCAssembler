using System;
namespace MERC.Instructions.RTypes
{
    public class And : RTypeInstruction
    {
        public And()
        {
            this.name = "and";
            this.opCode = 12;
        }

        protected override void Execute()
        {
            base.Execute();
            int a = this.rs.Value;
            int b = this.rt.Value;
            this.result = a & b;
            MemoryStoreAndWrite();
        }
    }
}
