using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Tokens{
		public List<IToken> InAList = new List<IToken>();

		public virtual void Add(IToken token) {
			testForOperationInference(token);
			InAList.Add(token);
		}

		private void testForOperationInference(IToken tokenToAdd) {
			if (InAList.Count() > 0) {
				//When a minus or plus sign is read as a negative number, add a plus sign before the number
				if (tokenToAdd.Type == TokenType.number
					&& (InAList.Last().Type == TokenType.number || InAList.Last().Type == TokenType.closedBrace)
					&& (tokenToAdd.TokenString[0] == '-' || tokenToAdd.TokenString[0] == '+')) {
					InAList.Add(new OperatorToken("+", TokenType.infixOperator));
				}
				//Infer a multiplication sign between two sets of parenthesis
				if (tokenToAdd.Type == TokenType.openBrace && InAList.Last().Type == TokenType.closedBrace) {
					InAList.Add(new OperatorToken("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign between parenthesis and a number (that doesn't start with a minus sign)
				if (tokenToAdd.Type == TokenType.openBrace && InAList.Last().Type == TokenType.number) {
					InAList.Add(new OperatorToken("*", TokenType.infixOperator));
				}
				if (tokenToAdd.Type == TokenType.number && InAList.Last().Type == TokenType.closedBrace && tokenToAdd.TokenString[0] != '-') {
					InAList.Add(new OperatorToken("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign beteen a number and a variable
				if (tokenToAdd.Type == TokenType.variable && InAList.Last().Type == TokenType.number) {
					InAList.Add(new OperatorToken("*", TokenType.infixOperator));
				}
				if (tokenToAdd.Type == TokenType.number && InAList.Last().Type == TokenType.variable) {
					InAList.Add(new OperatorToken("*", TokenType.infixOperator));
				}
			}
		}

		public string Visualize() {
			string visualization = string.Empty;
			foreach (IToken token in InAList) {
				visualization += token.TokenString + " ";
			}
			return visualization;
		}
	}
}
