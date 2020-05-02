using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CardEngine.Data;
using CardEngine.Exceptions;
using CardEngine.Project;
using ScriptEngine;

namespace CardEngine.State
{
    // TODO: Normally, the script should be able to be instantiated once and only call generate with different cards when in engine-mode.
    internal class ScriptGenerator
    {
        private readonly IFunctionsApi functionApi;
        private readonly IGlobalsApi globalsApi;
        private readonly List<Variable> variables;
        private readonly List<Script> scripts;

        private static readonly string[] DefaultAssemblies = {"System.dll", "System.Core.dll", "ScriptEngine.dll"};
        private const string ScriptNamespace = "Sandbox";
        private const string ScriptClassName = "Script";
        private const int ScriptLineOffset = 17;

        public ScriptGenerator(IFunctionsApi functionApi, IGlobalsApi globalsApi, List<Variable> variables, List<Script> scripts)
        {
            this.functionApi = functionApi;
            this.globalsApi = globalsApi;
            this.variables = variables;
            this.scripts = scripts;
        }

        public object CreateScriptInstance(Card card)
        {
            var csp = CodeDomProvider.CreateProvider("CSharp");
            var parameters =
                new CompilerParameters(DefaultAssemblies)
                {
                    GenerateInMemory = true,
                    IncludeDebugInformation = true, // TODO
                    TreatWarningsAsErrors = true,
                    GenerateExecutable = false
                };

            var filenameCollection = new List<string>();
            filenameCollection.Add(GetGlobalVariables());       // TODO: should be in the constructor
            filenameCollection.AddRange(GetGlobalScripts());    // TODO: should be in the constructor
            filenameCollection.Add(GetScript(card));
            var results = csp.CompileAssemblyFromFile(parameters, filenameCollection.ToArray());

            var errors = results.Errors.Cast<CompilerError>().ToList();
            if (errors.Count > 0)
            {
                throw new CompilerException(errors.Select(x =>
                {
                    var name = Path.GetFileName(x.FileName);
                    x.FileName = $"{name}";
                    x.Line -= ScriptLineOffset;
                    return x;
                }).ToList());
            }

            var scriptType = results.CompiledAssembly.GetType($"{ScriptNamespace}.{ScriptClassName}");
            var ctor = scriptType.GetConstructor(new[] { typeof(IGlobalsApi), typeof(IFunctionsApi) });
            if (ctor != null)
                return ctor.Invoke(new object[] { globalsApi, functionApi });
            else
                throw new NotImplementedException("Script doesn't contains a proper constructor.'");
        }

        private List<string> GetGlobalScripts()
        {
            if (scripts == null)
                return new List<string>();

            var filenameCollection = new List<string>();
            foreach (var script in scripts)
                filenameCollection.Add(GetScript(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), $"script.{script.Name}"), script.Source, script.Name));

            return filenameCollection;
        }

        private string GetScript(Card card)
        {
            return GetScript(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), $"card.{card.Name}"), card.PageScript.Source, ScriptClassName);
        }

        private string GetScript(string filename, string script, string className)
        {
            var code = File.ReadAllText("resources/Script.cs")
                .Replace("{{CODE}}", Environment.NewLine + script)
                .Replace("{{NAME}}", className);

            var file = new FileInfo(filename);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, code);

            return filename;
        }

        private string GetGlobalVariables()
        {
            var globals = File.ReadAllText("resources/GlobalVariables.cs");
            var sb = new StringBuilder();
            foreach (var item in variables)
            {
                sb.Append($@"

        public {item.Type.Name} {item.Name}
        {{
            get {{ return ({item.Type.Name})api.GetVariable(""{item.Name}""); }}
            set {{ api.SetVariable(""{item.Name}"", value); }}
        }}");
            }

            var code = globals.Replace("{{VARIABLES}}", sb.ToString());
            var filename = Path.Combine(Path.GetTempPath(), "global-variables");

            var file = new FileInfo(filename);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, code);

            return filename;
        }
    }
}
