using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Compiler;

namespace AvalonEdit.Sample {
	class CurrentState {
		public string TextOfCurrentLine {get; private set;}
		public string Output {get; private set;}
		public Complex LastValue { get; private set; }
		public Compiler.CompilerOutput CompilerOutput;
		public Compiler.Compiler Compiler = new Compiler.Compiler();

		internal void ComputeLine(string p) {
			TextOfCurrentLine = p;
			CompilerOutput = Compiler.EvaluateString(TextOfCurrentLine);
			if (CompilerOutput != null) {
				Output = CompilerOutput.Output + "\n";
				LastValue = CompilerOutput.ReturnValue;
			}
		}
	}
}
