using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace CardEngine.Exceptions
{
    public class CompilerException : Exception
    {
        public List<CompilerError> Errors { get; }

        public CompilerException(List<CompilerError> errors)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var error in Errors)
                sb.AppendLine($"{error.FileName}({error.Line},{error.Column}): Error {error.ErrorNumber}: {error.ErrorText}");

            return sb.ToString();
        }
    }
}
