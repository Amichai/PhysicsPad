using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Compiler.Libraries;

namespace Compiler {
	//TODO: Make an IToken interface and different classes for different types of tokens
	public interface IToken {
	
	}

	public class NumberToken : IToken { }
	public class FunctionToken : IToken { }
	public class VariableToken : IToken {
	}
	public class StringToken : IToken { }
	public class OperatorToken : IToken { }
	public class ArgSeperatorToken : IToken { }
	public class BraceToken : IToken { }
	public class EqualSignToken : IToken { }
	public class UnitToken : IToken { }

	public class Token {
		public static readonly HashSet<string> VariableLibrary = new HashSet<string>() { "ANS", "PI" };
		//TODO: Make a static library class to store all functions
		//TODO: Make functions case insensitive
		public TokenType TokenType;
		public string TokenString;
		public Complex TokenNumValue;
		public int numberOfChildren = int.MinValue;
		public IFunction Function;
		public IVariable Variable;

		public Token(string tokenString, TokenType tokenType) {
			TokenString = tokenString;
			TokenType = tokenType;
			switch (TokenType) {
				case TokenType.number:
					TokenNumValue = double.Parse(TokenString);
					break;
				case TokenType.infixOperator:
					numberOfChildren = 2;
					break;
				case TokenType.suffixOperator:
					numberOfChildren = 1;
					break;
				case TokenType.charString:
					TokenString = tokenString.ToUpper();
					if (Functions.Library.Contains(TokenString)) {
						TokenType = TokenType.function;
						#region Functions
						switch (TokenString) {
							case "SUM":
							Function = new Sum();
							break;
							case "SIN":
							Function = new Sin();
							break;
							case "COS":
							Function = new Cos();
							break;
							case "TAN":
							Function = new Tan();
							break;
							case "INVSIN":
							Function = new InvSin();
							break;
							case "INVCOS":
							Function = new InvCos();
							break;
							case "INVTAN":
							Function = new InvTan();
							break;
							case "ABS":
							Function = new Abs();
							break;
							case "SQRT":
							Function = new Sqrt();
							break;
							case "POW":
							Function = new Pow();
							break;
							case "CONVERT":
							Function = new Convert();
							break;
						}
						#endregion
					} else if (VariableLibrary.Contains(tokenString)) {
						TokenType = TokenType.variable;
						switch (tokenString) {
							case "PI":
								Variable = new PI();
								break;
						}
					} else if(Units.MassUnits.Contains(tokenString.ToLower())){
						TokenType = TokenType.massUnit;
					} else if (Units.VolumeUnits.Contains(tokenString.ToLower())) {
						TokenType = TokenType.volumeUnit;
					}
					break;
			}
		}
	}
}
