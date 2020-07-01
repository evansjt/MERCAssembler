using System;
using System.Linq;

namespace MERC.Instructions.RTypes
{
    public class Not : RTypeInstruction
    {
        public Not()
        {
            this.name = "not";
            this.opCode = 13;
        }

        protected override void FetchRegisters(string[] instructionArray)
        {
            this.rt = Globals.Registers["$zero"];
            foreach (var register in Globals.Registers)
            {
                if (register.Key.Equals(instructionArray[0]))
                {
                    if (register.Value.Address > 15 || register.Value.Address < 0)
                    {
                        Console.WriteLine($"ERROR: Register overflow at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                        Globals.ExceptionThrown = true;
                        return;
                    }
                    this.rd = register.Value;
                }
                if (register.Key.Equals(instructionArray[1]))
                {
                    if (register.Value.Address > 15 || register.Value.Address < 0)
                    {
                        Console.WriteLine($"ERROR: Register overflow at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                        Globals.ExceptionThrown = true;
                        return;
                    }
                    this.rs = register.Value;
                }
            }
            if (this.rd is null || this.rs is null)
            {
                Console.WriteLine($"ERROR: Unknown register at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
        }

        protected override void Execute()
        {
            base.Execute();
            int a = this.rs.Value;
            this.result = ~a;
            MemoryStoreAndWrite();
        }
    }
}
