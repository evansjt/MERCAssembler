using System;
using System.Linq;

namespace MERC.Instructions
{
    public abstract class JTypeInstruction : Instruction
    {
        protected int imm;

        protected int result;

        public override void AddInstructionToProgram(string[] args)
        {
            if (Int32.TryParse(args[1], out int o))
            {
                if (o > 1024 || o < -1024)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                Globals.InstructionLines.Add(args);
            }
            else if (Globals.Labels.TryGetValue(args[1], out int a))
            {
                if (a > 1024 || a < -1024)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                args[1] = a.ToString();
                Globals.InstructionLines.Add(args);
            }
            else
            {
                Console.WriteLine($"ERROR: Label \"{args[1]}\" at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")} not specified as a .globl variable.");
                Globals.ExceptionThrown = true;
            }
        }

        protected override void FetchAndDecode(string[] instructionArray)
        {
            instructionArgs = String.Join(", ", instructionArray);
            FetchImmediate(instructionArray);
        }

        protected virtual void FetchImmediate(string[] instructionArray)
        {
            if (Int32.TryParse(instructionArray[0], out int a))
            {
                this.imm = a;
            }
            else
            {
                Console.WriteLine($"ERROR: Invalid immediate at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
        }

        protected override void Execute()
        {
            this.result = this.imm;
            MemoryStoreAndWrite();
        }

        protected override void StoreMachineCode()
        {
            var op = this.opCode << 11;
            var newImm = this.imm * 2;
            if (this.imm < 0)
            {
                var immBin = Convert.ToString(this.imm, 2);
                newImm = Convert.ToInt32(immBin.Substring(immBin.Length - 4), 2);
            }
            var machCode = op ^ newImm;
            string hexVal = machCode.ToString("X4");
            Globals.MachineCode.Add(hexVal);
            Globals.Pc++;
        }
    }
}
