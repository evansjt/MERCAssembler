using System;
using System.Linq;

namespace MERC.Instructions
{
    public abstract class LTypeInstruction : Instruction
    {
        protected Register rd;
        protected int imm;

        protected int result;

        public override void AddInstructionToProgram(string[] args)
        {
            if (Int32.TryParse(args[2], out int o))
            {
                if (o > 255 || o < 0)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                Globals.InstructionLines.Add(args);
            }
            else if (Globals.Labels.TryGetValue(args[2], out int a))
            {
                if (a > 255 || a < 0)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                args[2] = a.ToString();
                Globals.InstructionLines.Add(args);
            }
            else
            {
                Console.WriteLine($"ERROR: Label \"{args[2]}\" at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")} not specified as a .globl variable.");
                Globals.ExceptionThrown = true;
            }
        }

        protected override void FetchAndDecode(string[] instructionArray)
        {
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
                    if (register.Value.Address > 7 || register.Value.Address < 0)
                    {
                        Console.WriteLine($"ERROR: Register overflow at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                        Globals.ExceptionThrown = true;
                        return;
                    }
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

        protected virtual void FetchImmediate(string[] instructionArray)
        {
            if (Int32.TryParse(instructionArray[1], out int i))
            {
                if (i > 255 || i < 0)
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
            var rdAddr = this.rd.Address << 8;
            var newImm = this.imm;
            if (this.imm < 0)
            {
                var immBin = Convert.ToString(this.imm, 2);
                newImm = Convert.ToInt32(immBin.Substring(immBin.Length - 4), 2);
            }
            var machCode = op ^ rdAddr ^ newImm;
            string hexVal = machCode.ToString("X4");
            Globals.MachineCode.Add(hexVal);
            Globals.Pc++;
        }
    }
}
