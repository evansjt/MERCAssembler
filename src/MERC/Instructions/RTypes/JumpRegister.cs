using System;
namespace MERC.Instructions.RTypes
{
    public class JumpRegister : RTypeInstruction
    {
        public JumpRegister()
        {
            this.name = "jr";
            this.opCode = 22;
        }

        protected override void FetchAndDecode(string[] instructionArray)
        {
            this.rs = Globals.Registers["$zero"];
            this.rt = Globals.Registers["$zero"];
            foreach (var register in Globals.Registers)
            {
                if (register.Key.Equals(instructionArray[0]))
                {
                    this.rd = register.Value;
                }
            }
            if (this.rd is null)
            {
                Console.WriteLine($"ERROR: Unknown register at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
        }

        protected override void Execute()
        {
            this.result = this.rs.Value;
            MemoryStoreAndWrite();
        }

        protected override void MemoryStoreAndWrite()
        {
            Globals.Pc = this.result;
        }
    }
}
