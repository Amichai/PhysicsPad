using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine;
using PhysicsEngine.Numbers;

namespace Compiler {
	public class CompilerOutput {
		public string Input = string.Empty;
		public Tokens Tokens;
		public System.Numerics.Complex ReturnValue;
		public ParseTree ParseTree = new TreeNode();
		public PostfixedTokens PostFixedTokens;
		public string Output = string.Empty;
		public CompilerOutput(string input, Tokens tokens, System.Numerics.Complex returnVal, ParseTree parseTree, 
			PostfixedTokens postFixedTokens, string output) {
				this.Input = input;
				this.Tokens = tokens;
				this.ReturnValue = returnVal;
				this.ParseTree = parseTree;
				this.PostFixedTokens = postFixedTokens;
				this.Output = output;
		}
		//Define variables 
		//Allow the resolution of one variable from the context
		//Figure out function derivations
		//graph
		//solve
		//Render - eq visualizer
		//Handle infinite series
		//Sums
		//Calc
	}
}
