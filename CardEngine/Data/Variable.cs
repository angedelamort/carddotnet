using System;

namespace CardEngine.Data
{
    public class Variable : IData
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
    }
}
