namespace MERC.Instructions.ITypes
{
    public class LoadWord : ITypeInstruction
    {
        public LoadWord()
        {
            this.name = "load";
            this.opCode = 7;
        }

        protected override void Execute()
        {
            Globals.Pc = this.newPc;
            int a = this.rs.Value;
            if (this.rs.Name.Equals("$sp") && this.imm >= 0)
            {
                var b = Globals.Stack[this.imm / 2];
                Globals.Registers[this.rd.Name].SetValue(b);
            }
            else
            {
                this.result = a + (this.imm / 2);
                MemoryStoreAndWrite();
            }
        }

        protected override void MemoryStoreAndWrite()
        {
            var valueAtAddress = Globals.DataSegment[this.result];
            Globals.Registers[this.rd.Name].SetValue(valueAtAddress);
        }
    }
}
