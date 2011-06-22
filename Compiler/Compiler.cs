using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemLogging;

namespace Compiler {
	public class Compiler {
		public CompilerOutput EvaluateString(string input) {
			Tokens tokens = new Tokenizer(input).Scan();
			System.Numerics.Complex returnValue = 0;
			PostfixedTokens postFixedTokens = null;
			ParseTree parseTree = null;
			string output = string.Empty;
			if (tokens != null && tokens.InAList.Count() > 0) {
				postFixedTokens = new PostfixedTokens(tokens.InAList);
				//TODO: Within the postfixed tokens class learn to handle variable names that can't be evaluated
				//TODO: since variables can't start with a number, parse 3PI as 3*PI
				parseTree = postFixedTokens.BuildParseTree();
				if (parseTree != null) {
					returnValue = parseTree.val;
					SystemLog.AddToReturnValues(returnValue);
					output = returnValue.FullVisualization();
				}
			}
			return new CompilerOutput(input, tokens, returnValue, parseTree, postFixedTokens, output);
		}
	}
}
