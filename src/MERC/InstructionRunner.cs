using System;
using System.Linq;
using MERC.Instructions;

namespace MERC
{
    public class InstructionRunner
    {
        public InstructionRunner()
        {
            Globals.Pc = 0;
            Globals.LinesLength = Globals.InstructionLines.Count;
        }

        internal void RunInstructions()
        {
            while (Globals.Pc < Globals.LinesLength && !Globals.ExceptionThrown)
            {
                string instString = Globals.InstructionLines[Globals.Pc][0];
                Instruction inst = Globals.InstructionSets.FirstOrDefault(i => i.name == instString);
                string[] args = Globals.InstructionLines[Globals.Pc].Skip(1).ToArray();
                switch (Globals.Command)
                {
                    case "run":
                        inst.RunInstruction(args);
                        break;
                    case "debug":
                        HandleDebugForInstruction(inst, args);
                        break;
                    case "write":
                        inst.WriteToOutput(args);
                        break;
                }
            }
        }

        private void HandleDebugForInstruction(Instruction inst, string[] args)
        {
            string c = "";
            if (inst.name != "nop")
            {
                do
                {
                    string hexVal = (Globals.Pc * 2).ToString("X4");
                    Console.WriteLine($"0x{hexVal}: {inst.name} {String.Join(", ", args)}");
                    Console.Write(">");
                    c = Console.ReadLine();
                    switch (c)
                    {
                        case "r":
                            Globals.PrintRegisters();
                            break;
                        case "d":
                            Globals.PrintDataSegment();
                            break;
                        case "s":
                            Globals.PrintStack();
                            break;
                        case "a":
                            Globals.PrintRegisters();
                            Globals.PrintDataSegment();
                            Globals.PrintStack();
                            break;
                        case "q":
                            Globals.Pc = Globals.LinesLength;
                            break;
                    }
                } while (c != "n" && c != "q");
            }
            if (c != "q")
            {
                inst.RunInstruction(args);
            }
        }
    }
}