namespace MERC.Instructions.ITypes
{
    public class StoreWord : ITypeInstruction
    {
        public StoreWord()
        {
            this.name = "store";
            this.opCode = 10;
        }

        protected override void Execute()
        {
            Globals.Pc = this.newPc;
            int a = this.rs.Value;
            if (this.rd.Name.Equals("$sp") && this.imm >= 0)
            {
                Globals.Stack[this.imm / 2] = a;
            }
            else
            {
                this.result = a + (this.imm / 2);
                MemoryStoreAndWrite();
            }
        }

        protected override void MemoryStoreAndWrite()
        {
            Globals.DataSegment[this.result] = this.rd.Value;
        }
    }
}
