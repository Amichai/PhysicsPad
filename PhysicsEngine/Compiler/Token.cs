using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;


namespace PhysicsEngine {
	public class Token {
		private static readonly HashSet<string> functionLibrary = new HashSet<string>() { "Print", "Add", "Sum", "Derivative", "Integral" };
		private static readonly HashSet<string> variableLibrary = new HashSet<string>() { };
		public TokenType TokenType;
		public string TokenString;
		public Numerics.BigRational TokenNumValue;
		public int numberOfChildren = int.MinValue;

		public Token(string tokenString, TokenType tokenType) {
			TokenString = tokenString;
			TokenType = tokenType;
			switch (TokenType) {
				case TokenType.number:
					Decimal tempValForParse = Decimal.Parse(TokenString);
					TokenNumValue = new Numerics.BigRational(tempValForParse);
					break;
				case TokenType.arithmeticOp:
					numberOfChildren = 2;
					break;
				case TokenType.suffixOp:
					numberOfChildren = 1;
					break;
				case TokenType.charString:
					if (functionLibrary.Contains(TokenString)) {
						TokenType = TokenType.function;
					} else if (variableLibrary.Contains(tokenString)) {
						TokenType = TokenType.variable;
						//TODO: Look up variable value in a dictionary
					}
					break;
			}
		}
	}
}
