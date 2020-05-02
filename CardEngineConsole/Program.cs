using System;
using CardEngine;
using CardEngine.Data;
using CardEngine.Project;
using CardEngine.Widgets;
using CardEngineConsole.UI;

namespace CardEngineConsole
{
    class Program
    {
        public class MyRenderer : IRenderer
        {
            public void Render(IPanel panel)
            {
                if (panel is ConsolePanel consolePanel && consolePanel.IsDirty)
                {
                    foreach (var child in panel.Children)
                    {
                        switch (child)
                        {
                            case IText text:
                                Console.WriteLine(text.Value);
                                break;
                        }
                    }

                    consolePanel.IsDirty = false;
                }
            }
        }

        public class ConsoleLogger : ILogger
        {
            public void Write(string msg) => Console.Write(msg);
            public void WriteLine(string msg) => Console.WriteLine(msg);
        }

        private static bool quit = false;

        static void Main()
        {
            var solution = new Solution();
            var testVar = new Variable {Name = "Test", Type = typeof(string), Value = "Hello World!"};
            solution.GlobalVariables.Add(testVar.Name, testVar);
            solution.Cards[0].PageScript.Source = @"
public void Click() 
{
    Api.WriteLine(Var.Test);
}";

            var panel = new ConsolePanel();
            panel.Children.Add(new ConsoleText { Value = "Press C to Click" });
            panel.Children.Add(new ConsoleText { Value = "Press Q to Quit" });
            panel.Children.Add(new ConsoleText { Value = "----------------" });
            solution.Cards[0].Panel = panel;

            try
            {
                var options = new EngineOptions
                {
                    Renderer = new MyRenderer(), 
                    Logger = new ConsoleLogger()
                };
                var engine = new Engine(solution, options);
                engine.OnError += OnEngineErrorHandler;
                engine.Start();

                do
                {
                    if (Console.KeyAvailable)
                    {
                        var keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.C:
                                engine.InvokeScriptAction("Click");
                                break;

                            case ConsoleKey.Q:
                                quit = true;
                                break;
                        }
                    }
                } while (!quit);

                if (engine.IsRunning)
                    engine.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void OnEngineErrorHandler(object sender, Exception e)
        {
            Console.WriteLine("Error!");
            Console.WriteLine(e);
            quit = true;
        }
    }
}
