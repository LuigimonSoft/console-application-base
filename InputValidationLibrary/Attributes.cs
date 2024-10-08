using System;

namespace InputValidationLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public int Position { get; }

        public ColumnAttribute(int position)
        {
            Position = position;
        }
    }
}
