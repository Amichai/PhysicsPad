using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine;
using PhysicsEngine.Numbers;
using PhysicsEngine.Compiler;


namespace PhysicsEngine.Expression {
	public class Expression {
		public string Input = string.Empty;
		public Tokens Tokens;
		public Value ReturnValue;
		public ParseTree ParseTree = new TreeNode();
		public PostfixedTokens PostFixedTokens;
		public string Output = string.Empty;
		public Expression(string input) {			
			this.Input			= input;
			Tokens				= new Tokenizer(input).Scan();
			if (Tokens != null && Tokens.tokens.Count() > 0) {
				PostFixedTokens = new PostfixedTokens(Tokens.tokens);
				ParseTree = PostFixedTokens.BuildParseTree();
				if (ParseTree != null) {
					ReturnValue = ParseTree.val;
					Output = ReturnValue.FullVisualization();
				}
			}
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


		//TODO: Handle factorial and build the Value class so it won't overflow
	}
}
