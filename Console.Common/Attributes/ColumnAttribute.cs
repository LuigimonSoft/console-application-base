using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBase.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public int _index { get; }

        public ColumnAttribute(int index)
        {
            _index = index;
        }
    }
}
 