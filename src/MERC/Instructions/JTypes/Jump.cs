using System;
namespace MERC.Instructions.JTypes
{
    public class Jump : JTypeInstruction
    {
        public Jump()
        {
            this.name = "j";
            this.opCode = 3;
        }

        protected override void MemoryStoreAndWrite()
        {
            Globals.Pc = this.result;
        }
    }
}
