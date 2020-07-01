using System;
using System.Linq;

namespace MERC.Instructions
{
    public abstract class RTypeInstruction : Instruction
    {
        protected Register rd;
        protected Register rs;
        protected Register rt;
        protected int result;

        protected override void FetchAndDecode(string[] instructionArray)
        {
            instructionArgs = String.Join(", ", instructionArray);
            FetchRegisters(instructionArray);
        }

        protected virtual void FetchRegisters(string[] instructionArray)
        {
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
                if (register.Key.Equals(instructionArray[2]))
                {
                    if (register.Value.Address > 7 || register.Value.Address < 0)
                    {
                        Console.WriteLine($"ERROR: Register overflow at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                        Globals.ExceptionThrown = true;
                        return;
                    }
                    this.rt = register.Value;
                }
            }
            if (this.rd is null || this.rs is null || this.rt is null)
            {
                Console.WriteLine($"ERROR: Unknown register at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
        }

        protected override void Execute()
        {
            Globals.Pc++;
        }

        protected override void MemoryStoreAndWrite()
        {
            this.rd.SetValue(this.result);
            Globals.Registers[this.rd.Name].SetValue(this.rd.Value);
        }

        protected override void StoreMachineCode()
        {
            var op = this.opCode << 11;
            var rdAddr = this.rd.Address << 7;
            var rsAddr = this.rs.Address << 3;
            var rtAddr = this.rt.Address;
            var machCode = op ^ rdAddr ^ rsAddr ^ rtAddr;
            string hexVal = machCode.ToString("X4");
            Globals.MachineCode.Add(hexVal);
            Globals.Pc++;
        }
    }
}
