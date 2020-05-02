using System;

namespace CardEngine.Data
{
    public class Script : IData
    {
        public string Id { get; } = Guid.NewGuid().ToString("N");

        public string Name { get; set; } = "Script";

        public string Source { get; set; }
    }
}
