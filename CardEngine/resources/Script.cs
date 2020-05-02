using System;
using ScriptEngine;

namespace Sandbox
{
    public class {{NAME}}
    {
        public IFunctionsApi Api { get; private set; }
        public __GlobalVariables Var { get; private set; }

        public {{NAME}}(IGlobalsApi globals, IFunctionsApi api)
        {
            Var = new __GlobalVariables(globals);
            Api = api;
        }

        {{CODE}}
    }
}
