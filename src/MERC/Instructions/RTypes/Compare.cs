using System;
namespace MERC.Instructions.RTypes
{
    public class Compare : RTypeInstruction
    {
        public Compare()
        {
            this.name = "comp";
            this.opCode = 15;
        }

        protected override void Execute()
        {
            base.Execute();
            string a = Convert.ToString(this.rs.Value, 2).PadLeft(16, '0');
            string b = Convert.ToString(this.rt.Value, 2).PadLeft(16, '0');
            a = a.Substring(a.Length - 8);
            b = b.Substring(b.Length - 8);
            if (a == b)
            {
                this.result = 0;
            }
            else if (this.rs.Value > this.rt.Value)
            {
                this.result = -1;
            }
            else
            {
                this.result = 1;
            }
            MemoryStoreAndWrite();
        }
    }
}
