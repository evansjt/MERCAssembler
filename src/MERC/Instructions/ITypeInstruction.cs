using System;
using System.Linq;

namespace MERC.Instructions
{
    public abstract class ITypeInstruction : Instruction
    {
        protected Register rd;
        protected Register rs;
        protected int imm;
        protected int result;
        protected int newPc;

        protected override void FetchAndDecode(string[] instructionArray)
        {
            this.newPc = Globals.Pc + 1;
            instructionArgs = String.Join(", ", instructionArray);
            FetchRegisters(instructionArray);
            FetchImmediate(instructionArray);
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
                    if (register.Value.Address > 7 || register.Value.Address < 0)
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

        protected virtual void FetchImmediate(string[] instructionArray)
        {
            if (Int32.TryParse(instructionArray[2], out int i))
            {
                if (i > 7 || i < -8)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                this.imm = i;
            }
            else
            {
                Console.WriteLine($"ERROR: Invalid immediate at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
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
            var rsAddr = this.rs.Address << 4;
            var newImm = this.imm;
            if (this.imm < 0)
            {
                var immBin = Convert.ToString(this.imm, 2);
                newImm = Convert.ToInt32(immBin.Substring(immBin.Length - 4), 2);
            }
            var machCode = op ^ rdAddr ^ rsAddr ^ newImm;
            string hexVal = machCode.ToString("X4");
            Globals.MachineCode.Add(hexVal);
            Globals.Pc++;
        }
    }
}