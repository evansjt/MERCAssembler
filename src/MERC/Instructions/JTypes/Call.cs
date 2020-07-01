using System;
namespace MERC.Instructions.JTypes
{
    public class Call : JTypeInstruction
    {
        public Call()
        {
            this.name = "call";
            this.opCode = 4;
        }

        protected override void MemoryStoreAndWrite()
        {
            var returnPc = Globals.Pc + 1;
            Globals.Registers["$ra"].SetValue(returnPc);
            Globals.Pc = this.result;
            ClearTempRegisters();
        }

        private void ClearTempRegisters()
        {
            foreach (var register in Globals.Registers)
            {
                if (!register.Value.SavedAcrossCall)
                {
                    register.Value.SetValue(0);
                }
            }
        }
    }
}
