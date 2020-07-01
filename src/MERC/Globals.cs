using System.Collections.Generic;
using MERC.Instructions;
using MERC.Instructions.RTypes;
using MERC.Instructions.ITypes;
using MERC.Instructions.JTypes;
using MERC.Instructions.LTypes;
using MERC.Registers;
using System;

namespace MERC
{
    public static class Globals
    {
        public static string Command { get; internal set; }

        public static int Pc { get; set; }

        public static int BaseAddressInc { get; set; } = 64;

        public static int LastBaseAddress { get; set; }

        public static int StartPcLineNum { get; set; }

        public static int LinesLength { get; set; }

        public static bool ExceptionThrown { get; set; }

        public static List<int> DataSegment { get; set; } = new List<int>();

        public static List<int> Stack { get; set; } = new List<int>();

        public static List<string[]> InstructionLines { get; set; } = new List<string[]>();

        public static List<string> MachineCode { get; set; } = new List<string>();

        public static Dictionary<string, int> Labels { get; set; } = new Dictionary<string, int>();

        public static List<Instruction> InstructionSets { get; private set; } = new List<Instruction> {
            new Add(),
            new Addi(),
            new And(),
            new Beq(),
            new Bgt(),
            new Bget(),
            new Blt(),
            new Blet(),
            new Bne(),
            new Call(),
            new Compare(),
            new Jump(),
            new JumpRegister(),
            new LoadWord(),
            new LoadLowerImm(),
            new LoadUpperImm(),
            new NOp(),
            new Not(),
            new Or(),
            new Sll(),
            new Srl(),
            new StoreWord(),
            new Subtract()
        };

        public static Dictionary<string, Register> Registers { get; set; } = new Dictionary<string, Register>
        {
            {"$zero", new ZeroRegister()},
            {"$0", new ZeroRegister()},
            {"$sp", new ConcreteRegister("$sp", 1, 0, true)},
            {"$t0", new ConcreteRegister("$t0", 2, 0, false)},
            {"$t1", new ConcreteRegister("$t1", 3, 0, false)},
            {"$t2", new ConcreteRegister("$t2", 4, 0, false)},
            {"$ra", new ConcreteRegister("$ra", 5, 0, true)},
            {"$s0", new ConcreteRegister("$s0", 6, 0, true)},
            {"$s1", new ConcreteRegister("$s1", 7, 0, true)},
            {"$s2", new ConcreteRegister("$s2", 8, 0, true)},
            {"$rv0", new ConcreteRegister("$rv0", 9, 0, true)},
            {"$rv1", new ConcreteRegister("$rv1", 10, 0, true)},
            {"$arg0", new ConcreteRegister("$arg0", 11, 0, true)},
            {"$arg1", new ConcreteRegister("$arg1", 12, 0, true)},
            {"$at", new ConcreteRegister("$at", 13, 0, false)},
            {"$k0", new ConcreteRegister("$k0", 14, 0, false)},
            {"$k1", new ConcreteRegister("$k1", 15, 0, false)}
        };

        internal static void Reset()
        {
            Pc = 0;
            StartPcLineNum = 0;
            LinesLength = 0;
            LastBaseAddress = 0;
            ExceptionThrown = false;
            InstructionLines = new List<string[]>();
            DataSegment = new List<int>();
            Stack = new List<int>();
            MachineCode = new List<string>();
            Labels = new Dictionary<string, int>();
            foreach (var register in Registers)
            {
                register.Value.SetValue(0);
            }
        }

        internal static void PrintRegisters()
        {
            Console.WriteLine("\nREGISTERS");
            Console.WriteLine("---------");
            foreach (var reg in Registers)
            {
                Console.WriteLine($"{reg.Key}: {reg.Value.Value}");
            }
            Console.WriteLine();
        }

        internal static void PrintDataSegment()
        {
            Console.WriteLine("\nDATA SEGMENT");
            Console.WriteLine("------------");
            if (DataSegment.Count == 0)
            {
                Console.WriteLine("No data to display");
            }
            else
            {
                for (int i = 0; i < DataSegment.Count; i++)
                {
                    string hexVal = (i * 2).ToString("X4");
                    Console.WriteLine($"0x{hexVal}: {DataSegment[i]}");
                }
            }
            Console.WriteLine();
        }

        internal static void PrintStack()
        {
            Console.WriteLine("\nSTACK");
            Console.WriteLine("-----");
            if (Stack.Count == 0)
            {
                Console.WriteLine("Stack empty");
            }
            else
            {
                for (int i = 0; i < Stack.Count; i++)
                {
                    string hexVal = (i * 2).ToString("X4");
                    Console.WriteLine($"0x{hexVal}: {Stack[i]}");
                }
            }
            Console.WriteLine();
        }
    }
}
