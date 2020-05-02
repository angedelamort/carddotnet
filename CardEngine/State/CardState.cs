using System;
using System.Linq;
using CardEngine.Project;
using CardEngine.Widgets;
using ScriptEngine;

namespace CardEngine.State
{
    // TODO: all events should be in a class with constants.
    internal class CardState
    {
        private readonly ScriptGenerator generator;
        private readonly object scriptInstance;

        //public Type ScriptType { get; private set; }
        //public object ScriptInstance { get; private set; }
        public Card Card { get; }
        public IPanel Panel { get; }

        public CardState(Card card, ScriptGenerator generator)
        {
            this.generator = generator;
            Card = card;
            Panel = Card.Panel?.Copy();

            scriptInstance = generator.CreateScriptInstance(Card);
        }

        private void LoadCardScript(Solution solution, IFunctionsApi functionApi, IGlobalsApi globalsApi)
        {
            //var generator = new ScriptGenerator(
            //    functionApi, 
            //    globalsApi, 
            //    solution.GlobalVariables.Values.ToList(),
            //    solution.GlobalScripts.Values.ToList());

            //generator.GenerateScript(Card);
            //var csp = CodeDomProvider.CreateProvider("CSharp");
            //var parameters =
            //    new CompilerParameters(new[] { "System.dll", "System.Core.dll", "ScriptEngine.dll" }) // TODO: list of assembly names that we want in the file...
            //    {
            //        GenerateInMemory = true,
            //        IncludeDebugInformation = true, // TODO
            //        TreatWarningsAsErrors = true,
            //        GenerateExecutable = false
            //    };

            //var scriptInfo = GetScript(Card);
            //var results = csp.CompileAssemblyFromSource(parameters, GetGlobals(solution), scriptInfo.content);

            //var errors = results.Errors.Cast<CompilerError>().ToList();
            //if (errors.Count > 0)
            //{
            //    throw new CompilerException(errors.Select(x =>
            //    {
            //        x.FileName = $"{Card.Name}#{Card.Id}";
            //        x.Line -= scriptInfo.lineOffset;
            //        return x;
            //    }).ToList());
            //}

            //ScriptType = results.CompiledAssembly.GetType("Sandbox.Script");
            //var ctor = ScriptType.GetConstructor(new[] { typeof(IGlobalsApi), typeof(IFunctionsApi) });
            //if (ctor != null)
            //    ScriptInstance = ctor.Invoke(new object[] { globalsApi, functionApi });
            //else
            //    throw new NotImplementedException("Script doesn't contains a proper constructor.'");
        }

        //private (string content, int lineOffset) GetScript(Card page)
        //{
        //    var script = File.ReadAllText("resources/Script.cs");
        //    var lineOffset = 1;
        //    foreach (var line in script.Split('\n'))
        //    {
        //        if (line.Contains("{{CODE}}"))
        //        {
        //            break;
        //        }

        //        lineOffset++;
        //    }

        //    return (script.Replace("{{CODE}}", Environment.NewLine + page.PageScript.Source), lineOffset);
        //}

        //private string GetGlobals(Solution solution)
        //{
        //    var globals = File.ReadAllText("resources/Globals.cs");
        //    var sb = new StringBuilder();
        //    foreach (var item in solution.GlobalVariables.Values)
        //    {
        //        sb.Append($@"

        //public {item.Type.Name} {item.Name}
        //{{
        //    get {{ return ({item.Type.Name})api.GetVariable(""{item.Name}""); }}
        //    set {{ api.SetVariable(""{item.Name}"", value); }}
        //}}");
        //    }

        //    return globals.Replace("{{VARIABLES}}", sb.ToString());
        //}

        public object InvokeScriptMethod(string methodName, params object[] parameters)
        {
            var methodParams = parameters.Select(x => x.GetType()).ToArray();
            var method = scriptInstance.GetType().GetMethod(methodName, methodParams);
            if (method != null)
                return method.Invoke(scriptInstance, new object[] { });
            throw new MissingMethodException($"No method found for {methodName} with {parameters.Length} parameters");
        }

        public object InvokeScriptMethodIfExists(string methodName, params object[] parameters)
        {
            var methodParams = parameters.Select(x => x.GetType()).ToArray();
            var method = scriptInstance.GetType().GetMethod(methodName, methodParams);
            return method != null ? method.Invoke(scriptInstance, new object[] { }) : null;
        }

        public void DestroyEvent()
        {
            DoEvent("OnDestroy");
        }

        public void CreateEvent()
        {
            DoEvent("OnCreate");
        }

        public void HideEvent()
        {
            DoEvent("OnHide");
        }

        private void DoEvent(string name)
        {
            InvokeScriptMethodIfExists(name);
        }

        public void ShowEvent()
        {
            DoEvent("OnShow");
        }

        public void ShownEvent()
        {
            DoEvent("OnShown");
        }

        public void HiddenEvent()
        {
            DoEvent("OnHidden");
        }

        public void PreRender()
        {
            DoEvent("OnPreRender");
        }

        public void PostRender()
        {
            DoEvent("OnPostRender");
        }
    }
}
