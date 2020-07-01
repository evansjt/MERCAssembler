using System;
using System.IO;
using MERC.Instructions.RTypes;

namespace MERC
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            PrintWelcome();

            string command;
            string[] commandArray;

            do
            {
                Globals.Reset();
                Console.Write("MERC_READY> ");
                command = Console.ReadLine();
                commandArray = command.Split(' ');

                if (commandArray.Length > 0)
                {
                    switch (commandArray[0])
                    {
                        case "run":
                            ParseMercFile(commandArray, true);
                            break;
                        case "debug":
                            ParseMercFile(commandArray, true);
                            break;
                        case "write":
                            ParseMercFile(commandArray, false);
                            break;
                        case "link":
                            ParseMercFile(commandArray, false);
                            break;
                        case "help":
                            PrintHelp();
                            break;
                        case "exit":
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown command. Try again.\n");
                            break;
                    }
                }
            } while (command != "exit");

            Environment.Exit(0);
        }

        private static void WriteToFile()
        {
            string path = "output";
            string fullPath = Path.GetFullPath(path);
            using (StreamWriter file = new StreamWriter($"{fullPath}/output.coe"))
            {
                file.WriteLine("; Memory Instantiation\nmemory_initialization_radix=16;\nmemory_initialization_vector= ");
                for (int i = 0; i < Globals.MachineCode.Count; i++)
                {
                    if (i == Globals.MachineCode.Count - 1)
                        file.Write(Globals.MachineCode[i]);
                    else
                        file.WriteLine($"{Globals.MachineCode[i]},");
                }
            }

            Console.WriteLine("\nSUCCESS!\nCheck the \"output\" directory for the output .coe file.");
        }

        private static void ParseMercFile(string[] commandArray, bool printResults)
        {
            if (commandArray.Length < 2)
            {
                Console.WriteLine("ERROR: This command must have a .merc filename provided as a second argument\n");
            }
            else
            {
                Globals.Command = commandArray[0];
                if (Globals.Command == "run" || Globals.Command == "debug")
                {
                    InsertArguments(commandArray);
                }
                try
                {
                    int fileCount = 1;
                    do
                    {
                        ReadMercFile(commandArray[fileCount], Path.GetFullPath("input"));
                        fileCount++;
                        if (Globals.Command == "link")
                        {
                            int remainingAddresses = Globals.BaseAddressInc - (Globals.InstructionLines.Count % Globals.BaseAddressInc);
                            for (int i = 0; i < remainingAddresses; i++)
                            {
                                Globals.InstructionLines.Add(new string[] { "nop" });
                            }
                            Globals.LastBaseAddress += Globals.BaseAddressInc;
                            Globals.Pc = 0;
                            Globals.StartPcLineNum = 0;
                            Globals.LinesLength = 0;
                        }
                    } while (Globals.Command == "link" && fileCount < commandArray.Length && !Globals.ExceptionThrown);


                    if (!Globals.ExceptionThrown)
                    {
                        if (Globals.Command == "link")
                        {
                            Globals.Command = "write";
                        }

                        InstructionRunner runner = new InstructionRunner();
                        runner.RunInstructions();

                        if (printResults)
                        {
                            Globals.PrintRegisters();
                            Globals.PrintDataSegment();
                            Globals.PrintStack();
                        }
                        else
                        {
                            WriteToFile();
                        }
                    }

                    Console.WriteLine();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("ERROR: .merc file does not exist in the \"input\" directory.\n");
                }
            }
        }

        private static void ReadMercFile(string filename, string fullPath)
        {
            string[] lines = File.ReadAllLines($"{fullPath}/{filename}.merc");
            Parser parser = new Parser(lines);
            parser.Parse();
        }

        private static void InsertArguments(string[] commandArray)
        {
            if (commandArray.Length > 2)
            {
                Globals.Registers["$arg0"].SetValue(Int32.TryParse(commandArray[2], out int a0) ? a0 : 0);
                if (commandArray.Length > 3)
                {
                    Globals.Registers["$arg1"].SetValue(Int32.TryParse(commandArray[3], out int a1) ? a1 : 0);
                }
            }
        }

        public static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the MERC-Assembler - Version 1.0");
            Console.WriteLine(@"
         ___           ___           ___           ___     
        /\__\         /\  \         /\  \         /\  \    
       /::|  |       /::\  \       /::\  \       /::\  \   
      /:|:|  |      /:/\:\  \     /:/\:\  \     /:/\:\  \  
     /:/|:|__|__   /::\~\:\  \   /::\~\:\  \   /:/  \:\  \ 
    /:/ |::::\__\ /:/\:\ \:\__\ /:/\:\ \:\__\ /:/__/ \:\__\
    \/__/~~/:/  / \:\~\:\ \/__/ \/_|::\/:/  / \:\  \  \/__/
          /:/  /   \:\ \:\__\      |:|::/  /   \:\  \      
         /:/  /     \:\ \/__/      |:|\/__/     \:\  \     
        /:/  /       \:\__\        |:|  |        \:\__\    
        \/__/         \/__/         \|__|         \/__/

            ");
            Console.WriteLine("Copyright (c) - Nick Carpenter, Jake Evans, Craig McGee, Angel Rivera\n");
            Console.WriteLine("For help on how to operate this console application, type \"help\"\n");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("\nHELP:");
            Console.WriteLine("-----");

            string[,] commDesc = { {"run [filename] [arg0] [arg1]" , "runs the entire .merc file*" },
                                    {"debug [filename] [arg0] [arg1]","steps through each instruction of the.merc file*" },
                                    { "write [filename]", "writes machine code from input into a .coe file in the \"output\" directory" },
                                    { "link [filename1] [filename2] ...", "links multiple .merc programs and writes their machine code sequentially into a .coe file in the \"output\" directory" },
                                    { "help", "prints all possible commands for this console application" },
                                    { "exit", "exits the console application" } };

            string[,] debugCommDesc = { { "n", "moves on to next instruction" },
                                    { "q", "quits debug mode" },
                                    { "r", "prints registers at current state" },
                                    { "d", "prints data segment at current state" },
                                    { "s", "prints stack at current state" },
                                    { "a", "prints registers, data segment, and stack at current state" } };

            Console.WriteLine("Here are all possible commands to use for this console application:\n");

            for (int i = 0; i < commDesc.GetLength(0); i++)
            {
                Console.Write("- \"");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(commDesc[i, 0]);
                Console.ResetColor();
                Console.WriteLine($"\" : {commDesc[i, 1]}");
                if (commDesc[i, 0].Contains("debug"))
                {
                    for (int j = 0; j < debugCommDesc.GetLength(0); j++)
                    {
                        Console.Write("\t=> \"");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(debugCommDesc[j, 0]);
                        Console.ResetColor();
                        Console.WriteLine($"\" : {debugCommDesc[j, 1]}");
                    }
                }
            }
            Console.WriteLine("\n*(if arg0 or arg1 are not provided, they are automatically set to zero)\n\n** NOTE: [filename] must be a .merc file (named with no spaces) inside the \"input\" directory. **\n");
        }
    }
}
