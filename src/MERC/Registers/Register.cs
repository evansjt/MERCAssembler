using System;
using System.Collections.Generic;

namespace MERC
{
    public abstract class Register
    {
        public string Name { get; protected set; }

        public int Value { get; protected set; }
        public int Address { get; protected set; }
        public bool SavedAcrossCall { get; protected set; }

        public virtual void SetValue(int newValue)
        {
            this.Value = newValue;
        }
    }
}
