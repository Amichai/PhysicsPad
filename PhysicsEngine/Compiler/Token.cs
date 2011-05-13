using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using PhysicsEngine.ReferenceLibraries;
using BigRationalNumerics;

namespace PhysicsEngine {
	public class Token {
		//TODO: Make a static library class to store all functions
		//TODO: Make functions case insensitive
		public TokenType TokenType;
		public string TokenString;
		public BigRational TokenNumValue;
		public int numberOfChildren = int.MinValue;

		public Token(string tokenString, TokenType tokenType) {
			TokenString = tokenString;
			TokenType = tokenType;
			switch (TokenType) {
				case TokenType.number:
					Decimal tempValForParse = Decimal.Parse(TokenString);
					TokenNumValue = new BigRational(tempValForParse);
					break;
				case TokenType.infixOperator:
					numberOfChildren = 2;
					break;
				case TokenType.suffixOperator:
					numberOfChildren = 1;
					break;
				case TokenType.charString:
					if (Functions.Library.Contains(TokenString.ToUpper())) {
						TokenType = TokenType.function;
						TokenString = tokenString.ToUpper();
					} else if (Variable.Library.Contains(tokenString)) {
						TokenType = TokenType.variable;
						//TODO: Look up variable value in a dictionary
					}
					break;
			}
		}
	}
}
