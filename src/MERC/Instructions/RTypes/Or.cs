using System;
namespace MERC.Instructions.RTypes
{
    public class Or : RTypeInstruction
    {
        public Or()
        {
            this.name = "or";
            this.opCode = 11;
        }

        protected override void Execute()
        {
            base.Execute();
            var a = this.rs.Value;
            var b = this.rt.Value;
            this.result = a | b;
            MemoryStoreAndWrite();
        }
    }
}
