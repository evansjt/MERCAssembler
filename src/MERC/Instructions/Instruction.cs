using System;
namespace MERC.Instructions
{
    public abstract class Instruction
    {
        public string name;
        public string instructionArgs;
        protected int opCode;

        protected abstract void FetchAndDecode(string[] instructionArray);
        protected abstract void Execute();
        protected abstract void MemoryStoreAndWrite();
        protected abstract void StoreMachineCode();


        public virtual void AddInstructionToProgram(string[] args)
        {
            Globals.InstructionLines.Add(args);
        }

        internal void RunInstruction(string[] currentLine)
        {
            FetchAndDecode(currentLine);
            if (!Globals.ExceptionThrown)
            {
                Execute();
            }
        }

        internal void WriteToOutput(string[] currentLine)
        {
            FetchAndDecode(currentLine);
            StoreMachineCode();
        }
    }
}
