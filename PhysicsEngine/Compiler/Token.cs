using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;


namespace PhysicsEngine {
	public class Token {
		private static readonly HashSet<string> functionLibrary = new HashSet<string>() { "Print", "Add", "Sum", "Derivative", "Integral" };
		public TokenType TokenType;
		public string TokenString;
		public Numerics.BigRational TokenNumValue;

		public Token(string tokenString, TokenType tokenType) {
			TokenString = tokenString;
			TokenType = tokenType;
			if (TokenType == TokenType.number) {
				Decimal tempValForParse = Decimal.Parse(TokenString);
				//TODO: I don't understand the double BR construtcor. THis seems to be broken.
				//ADd the numerator and denominator explicitly
				TokenNumValue = new Numerics.BigRational(tempValForParse);
				
			}
			if (TokenType == TokenType.charString) {
				if (functionLibrary.Contains(TokenString)) {
					TokenType = TokenType.function;
				} else {
					TokenType = TokenType.variable;
					//TODO: Look up variable value in a dictionary
				}
			}
		}
	}
}
