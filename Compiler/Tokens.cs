using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public class Tokens{
		public List<Token> InAList = new List<Token>();

		public virtual void Add(Token token) {
			testForOperationInference(token);
			InAList.Add(token);
		}

		private void testForOperationInference(Token tokenToAdd) {
			if (InAList.Count() > 0) {
				//When a minus or plus sign is read as a negative number, add a plus sign before the number
				if (tokenToAdd.TokenType == TokenType.number
					&& (InAList.Last().TokenType == TokenType.number || InAList.Last().TokenType == TokenType.closedBrace)
					&& (tokenToAdd.TokenString[0] == '-' || tokenToAdd.TokenString[0] == '+')) {
					InAList.Add(new Token("+", TokenType.infixOperator));
				}
				//Infer a multiplication sign between two sets of parenthesis
				if (tokenToAdd.TokenType == TokenType.openBrace && InAList.Last().TokenType == TokenType.closedBrace) {
					InAList.Add(new Token("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign between parenthesis and a number (that doesn't start with a minus sign)
				if (tokenToAdd.TokenType == TokenType.openBrace && InAList.Last().TokenType == TokenType.number) {
					InAList.Add(new Token("*", TokenType.infixOperator));
				}
				if (tokenToAdd.TokenType == TokenType.number && InAList.Last().TokenType == TokenType.closedBrace && tokenToAdd.TokenString[0] != '-') {
					InAList.Add(new Token("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign beteen a number and a variable
				if (tokenToAdd.TokenType == TokenType.variable && InAList.Last().TokenType == TokenType.number) {
					InAList.Add(new Token("*", TokenType.infixOperator));
				}
				if (tokenToAdd.TokenType == TokenType.number && InAList.Last().TokenType == TokenType.variable) {
					InAList.Add(new Token("*", TokenType.infixOperator));
				}
			}
		}

		public string Visualize() {
			string visualization = string.Empty;
			foreach (Token token in InAList) {
				visualization += token.TokenString + " ";
			}
			return visualization;
		}
	}
}
