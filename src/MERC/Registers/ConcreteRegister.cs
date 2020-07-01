using System;
namespace MERC.Registers
{
    public class ConcreteRegister : Register
    {

        public ConcreteRegister(string name, int address, int value, bool savedAcrossCall)
        {
            this.Name = name;
            this.Address = address;
            this.Value = value;
            this.SavedAcrossCall = savedAcrossCall;
        }
    }
}
