using System;
namespace MERC.Registers
{
    public class ZeroRegister : Register
    {
        public ZeroRegister()
        {
            this.Name = "$zero";
            this.Value = 0;
            this.Address = 0;
        }

        public override void SetValue(int newValue) { }
    }
}
