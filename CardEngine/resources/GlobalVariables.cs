using System;
using ScriptEngine;

namespace Sandbox
{
    public class __GlobalVariables
    {
        private readonly IGlobalsApi api;

        public __GlobalVariables(IGlobalsApi api)
        {
            this.api = api;
        }

        {{VARIABLES}}
    }
}