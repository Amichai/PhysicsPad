using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsEngine {
	public class Tokens{
		public List<Token> tokens = new List<Token>();

		public virtual void Add(Token token) {
			testForOperationInference(token);
			tokens.Add(token);
		}

		private void testForOperationInference(Token tokenToAdd) {
			if (tokens.Count() > 0) {
				//When a minus or plus sign is read as a negative number, add a plus sign before the number
				if (tokenToAdd.TokenType == TokenType.number
					&& (tokens.Last().TokenType == TokenType.number || tokens.Last().TokenType == TokenType.closedBrace)
					&& (tokenToAdd.TokenString[0] == '-' || tokenToAdd.TokenString[0] == '+')) {
					tokens.Add(new Token("+", TokenType.infixOperator));
				}
				//Infer a multiplication sign between two sets of parenthesis
				if (tokenToAdd.TokenType == TokenType.openBrace && tokens.Last().TokenType == TokenType.closedBrace) {
					tokens.Add(new Token("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign between parenthesis and a number (that doesn't start with a minus sign)
				if (tokenToAdd.TokenType == TokenType.openBrace && tokens.Last().TokenType == TokenType.number) {
					tokens.Add(new Token("*", TokenType.infixOperator));
				}
				if (tokenToAdd.TokenType == TokenType.number && tokens.Last().TokenType == TokenType.closedBrace && tokenToAdd.TokenString[0] != '-') {
					tokens.Add(new Token("*", TokenType.infixOperator));
				}
				//Infer a multiplication sign beteen a number and a variable
				if (tokenToAdd.TokenType == TokenType.variable && tokens.Last().TokenType == TokenType.number) {
					tokens.Add(new Token("*", TokenType.infixOperator));
				}
				if (tokenToAdd.TokenType == TokenType.number && tokens.Last().TokenType == TokenType.variable) {
					tokens.Add(new Token("*", TokenType.infixOperator));
				}
			}
		}

		public string Visualize() {
			string visualization = string.Empty;
			foreach (Token token in tokens) {
				visualization += token.TokenString + " ";
			}
			return visualization;
		}
	}
}
