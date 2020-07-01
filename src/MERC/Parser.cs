using System;
using System.Collections.Generic;
using System.Linq;

namespace MERC
{
    public class Parser
    {
        protected internal List<string[]> lines = new List<string[]>();

        public string[] currentLine;
        public int currLineNum;

        public int lastDataSegmentAddr;

        public Parser(string[] fileLines)
        {
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (!string.IsNullOrEmpty(fileLines[i]))
                {
                    var thisLine = fileLines[i];
                    if (thisLine.Contains("#"))
                    {
                        var commentSymbol = thisLine.IndexOf('#');
                        thisLine = thisLine.Substring(0, commentSymbol);
                    }
                    lines.Add(thisLine.Trim().Replace(",", "").Replace("\t", " ").Split(' '));
                }
            }
        }

        internal void Parse()
        {
            FindStartPcLineNum();
            ParseGlobals();
            Globals.LinesLength = lines.Count - Globals.StartPcLineNum;
            while (Globals.Pc < Globals.LinesLength && !Globals.ExceptionThrown)
            {
                currentLine = lines[Globals.Pc + Globals.StartPcLineNum];
                if (currentLine is null || currentLine.Length == 0)
                {
                    Globals.Pc++;
                }
                else
                {
                    ParseInstruction();
                    Globals.Pc++;
                }
            }
        }

        internal void ParseInstruction()
        {
            bool foundInstruction = false;
            foreach (var inst in Globals.InstructionSets)
            {
                if (inst.name == currentLine[0])
                {
                    inst.AddInstructionToProgram(currentLine);
                    foundInstruction = true;
                    break;
                }
            }
            if (!foundInstruction)
            {
                Console.WriteLine($"ERROR: Invalid Instruction \"{currentLine[0]}\" at PC=0x{((Globals.Pc + Globals.LastBaseAddress) * 2).ToString("X4")}");
                Globals.ExceptionThrown = true;
            }
        }

        internal void FindStartPcLineNum()
        {
            while (Globals.StartPcLineNum < lines.Count && !lines[Globals.StartPcLineNum][0].Contains(".text"))
            {
                Globals.StartPcLineNum++;
            }
            Globals.StartPcLineNum++;
        }

        internal void ParseGlobals()
        {
            while (currLineNum < lines.Count && !lines[currLineNum][0].Contains(".data") && !lines[currLineNum][0].Contains(".text"))
            {
                currentLine = lines[currLineNum];
                if (currentLine[0].Contains(".globl"))
                {
                    if (!IsInData()) ParseLabel();
                }
                currLineNum++;
            }
        }

        private void ParseLabel()
        {
            for (int i = Globals.StartPcLineNum; i < lines.Count; i++)
            {
                if (lines[i][0].Contains(currentLine[1]))
                {
                    if (Globals.Labels.ContainsKey(currentLine[1]))
                    {
                        Console.WriteLine($"ERROR: More than one instance of label \"{currentLine[1]}\" specified");
                        Globals.ExceptionThrown = true;
                        break;
                    }
                    else
                    {
                        Globals.Labels.Add(currentLine[1], (i - Globals.StartPcLineNum) + Globals.LastBaseAddress);
                        lines.Remove(lines[i]);
                    }
                }
            }
        }

        internal bool IsInData()
        {
            for (int i = currLineNum + 1; i < Globals.StartPcLineNum; i++)
            {
                if (lines[i][0].Contains(currentLine[1]))
                {
                    if (lines[i][1].Contains(".word"))
                    {
                        int firstAddress = lastDataSegmentAddr;
                        for (int j = 2; j < lines[i].Length; j++)
                        {
                            Globals.DataSegment.Add(Int32.Parse(lines[i][j].Replace(",", "")));
                            lastDataSegmentAddr++;
                        }
                        if (Globals.Labels.ContainsKey(currentLine[1]))
                        {
                            Console.WriteLine($"ERROR: More than one instance of label \"{currentLine[1]}\" specified");
                            Globals.ExceptionThrown = true;
                            break;
                        }
                        else
                        {
                            Globals.Labels.Add(currentLine[1], firstAddress);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}