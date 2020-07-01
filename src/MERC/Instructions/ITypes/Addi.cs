using System;
namespace MERC.Instructions.ITypes
{
    public class Addi : ITypeInstruction
    {
        public Addi()
        {
            this.name = "addi";
            this.opCode = 1;
        }

        protected override void Execute()
        {
            Globals.Pc = this.newPc;
            if (this.rs.Name.Equals("$sp"))
            {
                if (this.imm > 0 && (-this.imm / 2) < Globals.Stack.Count)
                {
                    Globals.Stack.RemoveRange(Globals.Stack.Count - (this.imm / 2), Globals.Stack.Count);
                }
                else if (this.imm < 0)
                {
                    for (int i = 0; i < (-this.imm / 2); i++)
                    {
                        Globals.Stack.Add(0);
                    }
                }
            }
            int a = this.rs.Value;
            int b = this.imm;
            this.result = a + b;
            MemoryStoreAndWrite();
        }
    }
}
