using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public class WorkingPath
    {
        public string Value;

        public WorkingPath(string value)
        {
            Value = value;
        }

        public static implicit operator string(WorkingPath w)
        {
            return w.Value;
        }

        public static implicit operator WorkingPath(string value)
        {
            return new WorkingPath(value);
        }

        public override int GetHashCode()
        {
            if (Value != null)
                return Value.GetHashCode();
            else
                return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorkingPath))
                return false;

            return string.Equals(Value, ((WorkingPath)obj).Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
