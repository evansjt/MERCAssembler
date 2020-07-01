using System;
namespace MERC.Instructions.ITypes
{
    public abstract class BranchInstruction : ITypeInstruction
    {

        public override void AddInstructionToProgram(string[] args)
        {
            if (Int32.TryParse(args[3], out int i))
            {
                if (i > 7 || i < -8)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc+Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                Globals.InstructionLines.Add(args);
            }
            else if (Globals.Labels.TryGetValue(args[3], out int a))
            {
                var distance = (a - (Globals.LastBaseAddress)) - (Globals.Pc+1);
                if (distance > 7 || distance < -8)
                {
                    Console.WriteLine($"ERROR: Immediate overflow at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                    Globals.ExceptionThrown = true;
                    return;
                }
                args[3] = distance.ToString();
                Globals.InstructionLines.Add(args);
            }
            else
            {
                Console.WriteLine($"ERROR: Label \"{args[3]}\" at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")} not specified as a .globl variable.");
                Globals.ExceptionThrown = true;
            }
        }

        protected override void FetchImmediate(string[] instructionArray)
        {
            if (Int32.TryParse(instructionArray[2], out int i))
            {
                this.imm = i;
            }
            else
            {
                Console.WriteLine($"ERROR: Invalid immediate at PC=0x{(Globals.Pc * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
                return;
            }
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
