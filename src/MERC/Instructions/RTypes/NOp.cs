using System;
namespace MERC.Instructions.RTypes
{
    public class NOp : RTypeInstruction
    {
        public NOp()
        {
            this.name = "nop";
            this.opCode = 14;
        }

        protected override void FetchAndDecode(string[] instructionArray)
        {
            this.rd = Globals.Registers["$zero"];
            this.rs = Globals.Registers["$zero"];
            this.rt = Globals.Registers["$zero"];
        }

        protected override void Execute() {
            base.Execute();
        }
    }
}
