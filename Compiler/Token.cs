using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Compiler.Libraries;

namespace Compiler {
	//TODO: Make an IToken interface and different classes for different types of tokens
	public interface IToken {
		string TokenString { get; set; }
		TokenType Type { get; set; }
	}

	public class NumberToken : IToken {
		public string TokenString { get; set; }
		public Complex TokenNumValue;
		public NumberToken(string val) {
			this.TokenString = val;
			this.TokenNumValue = new Complex(double.Parse(val),0); 
		}
		public NumberToken(Complex val) {
			this.TokenString = val.ToString();
			this.TokenNumValue = val;
		}
		public TokenType Type { get; set; }
	}
	public class FunctionToken : IToken {
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"SUM","SIN","COS","TAN","ABS","SQRT","POW","INVCOS","INVSIN","INVTAN","CONVERT"
		};
		public string TokenString { get; set; }
		public int numberOfChildren = int.MinValue;
		public IFunction Function;
		public TokenType Type { get; set; }
		public FunctionToken(string tokenString) {
			this.TokenString = tokenString;
			this.Type = TokenType.function;
			switch (tokenString) {
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
		}
	}
	public class VariableToken : IToken {
		public TokenType Type {get; set;}
		public string TokenString { get; set; }
		public static readonly HashSet<string> VariableLibrary = new HashSet<string>() { "PI" };
		public IVariable Variable;
		public VariableToken(string tokenString) {
			switch (tokenString) {
				case "PI":
					Variable = new PI();
					break;
			}
			this.TokenString = tokenString;
		}
	}
	public class StringToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
	}
	public class OperatorToken : IToken {
		public OperatorToken(string tokenString, TokenType tokenType) {
			this.TokenString = tokenString;
			this.Type = tokenType;
		}
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
	}
	public class ArgSeperatorToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
	}
	public class BraceToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
	}
	public class EqualSignToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
	}
	public class UnitToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }
		public static HashSet<string> Units = new HashSet<string>() { "AMU", "GRAM", "KILOGRAM", "PICOGRAM", "MILIGRAM", "TON", 
																		"LITER", "GALLON", "CUP", "MILILITER", "NANOLITER" };
		public IUnit unit;
		public UnitToken(string tokenString) {
			switch (tokenString) {
				case "AMU":
					break;
				case "GRAM":
					break;
				case "KILOGRAM":
					break;
				case "PICOGRAM":
					break;
				case "MILIGRAM":
					break;
				case "TON":
					break;
				case "LITER":
					break;
				case "GALLON":
					break;
				case "CUP":
					break;
				case "MILILITER":
					break;
				case "NANOLITER":
					break;
			}
			this.TokenString = tokenString;
		}
	}
	public class KeywordToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }		
		public IToken Token;
		public static HashSet<string> Keywords = new HashSet<string>() { "ANS" };
		public KeywordToken(string tokenString) {
			this.TokenString = tokenString;
			switch (tokenString) {
				case "ANS":
					Type = TokenType.keyword;
					Token = new NumberToken(SystemLog.LastNumber());
					break;
			}
		}
	}
	public class SyntaxToken : IToken {
		public string TokenString { get; set; }
		public TokenType Type { get; set; }		
		public SyntaxToken(string tokenString, TokenType type) {
			this.TokenString = tokenString;
			this.Type = type;
		}

	}
}
