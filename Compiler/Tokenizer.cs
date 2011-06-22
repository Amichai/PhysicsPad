using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SystemLogging;
using System.Numerics;

namespace Compiler {
	public enum TokenType {
		number, function, charString, infixOperator, argSeperator, empty, closedBrace, openBrace, equalSign, variable, suffixOperator,
		keyword,
	}
	public enum CharType { number, letter, infixArithmeticOp, comma, period, plusOrMinusSign, brace, whitespace, suffixOp }
	class Tokenizer {
		public readonly static HashSet<char> argumentSeperator = new HashSet<char>() { ',' };
		public readonly static HashSet<char> infixArithmeticOperations = new HashSet<char>() { '/', '*', '^', '%' };
		public readonly static HashSet<char> suffixOperation = new HashSet<char>() { '!' };
		
		string compilerInput = string.Empty;

		public Tokenizer(string input) {
			compilerInput = input; 
		}

		Tokens allTokens = new Tokens();

		private class currentChar {
			public char val { get; set; }
			public CharType currentCharTokenType;

			public currentChar(char c){
				val = c;
				if (char.IsNumber(c)) {
					currentCharTokenType = CharType.number;
				}
				if (c == '.') {
					currentCharTokenType = CharType.period;
				}
				if (char.IsLetter(c) || c == '_')
					currentCharTokenType = CharType.letter;
				if (argumentSeperator.Contains(c))
					currentCharTokenType = CharType.comma;
				if (infixArithmeticOperations.Contains(c)) {
					currentCharTokenType = CharType.infixArithmeticOp;
				}
				if (c == '(' || c == ')')
					currentCharTokenType = CharType.brace;
				if (c == ' ')
					currentCharTokenType = CharType.whitespace;
				if (c == '+' || c == '-')
					currentCharTokenType = CharType.plusOrMinusSign;
				if (suffixOperation.Contains(c))
					currentCharTokenType = CharType.suffixOp;
			}
		}

		private class currentCharString {
			public string tokenString = string.Empty; 
			private HashSet<CharType> syntaxIllegalCharTypes = new HashSet<CharType>();
			private HashSet<CharType> charsThatAppendToCurrentString = new HashSet<CharType>();

			public IToken PublishCurrentTokenString() {
				IToken tokenToReturn = null ;
				switch (currentStringTokenType) {
				case TokenType.number:
					tokenToReturn = new NumberToken(tokenString);
					break;
				case TokenType.infixOperator:
					tokenToReturn = new OperatorToken(tokenString, TokenType.infixOperator);
					break;
				case TokenType.suffixOperator:
					tokenToReturn = new OperatorToken(tokenString, TokenType.suffixOperator);
					break;
				case TokenType.charString:
					tokenString = tokenString.ToUpper();
					if (FunctionToken.Library.Contains(tokenString)) {
						tokenToReturn = new FunctionToken(tokenString);		
					} else if (VariableToken.VariableLibrary.Contains(tokenString)) {
						tokenToReturn = new VariableToken(tokenString);
					} else if(UnitToken.Units.Contains(tokenString)){
						tokenToReturn = new UnitToken(tokenString);
					} else if (KeywordToken.Keywords.Contains(tokenString)) {
						tokenToReturn = new KeywordToken(tokenString);
					}
					break;
					case TokenType.openBrace:
					tokenToReturn = new SyntaxToken(tokenString, TokenType.openBrace);
					break;
					case TokenType.closedBrace:
					tokenToReturn = new SyntaxToken(tokenString, TokenType.closedBrace);
					break;
					case TokenType.argSeperator:
					tokenToReturn = new SyntaxToken(tokenString, TokenType.argSeperator);
					break;
				}
				return tokenToReturn;
			}

			private bool decimalNumber = false;

			public IToken AddChar(currentChar c) {
				IToken tokenToReturn  = null;
				//check if the char is legal in this context
				if (syntaxIllegalCharTypes.Contains(c.currentCharTokenType)) {
					ErrorLog.Add(new ErrorMessage("Syntax illegal char cannot be added to token"));
					return null;
				}
				//The char is legal. Check we if we should publish the current string
				if (!charsThatAppendToCurrentString.Contains(c.currentCharTokenType) && tokenString.Count() > 0) {
					tokenToReturn = PublishCurrentTokenString();
					tokenString = string.Empty;
				}
				//Append Char
				tokenString += c.val;
				//Depending on the current char type:
				//Set the local string type and other currenttoken state values
				//Set publication types and char legal types
				switch (c.currentCharTokenType) {
					case CharType.infixArithmeticOp:
						currentStringTokenType = TokenType.infixOperator;
						//Set the value of the publication type, etc.
						//Publish on every char type
						charsThatAppendToCurrentString = new HashSet<CharType>() {};
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.comma, CharType.suffixOp };
						break;
					case CharType.suffixOp:
						currentStringTokenType = TokenType.suffixOperator;
						charsThatAppendToCurrentString = new HashSet<CharType>() {};
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.number };
						break;
					case CharType.brace:
						if(c.val == ')')
							currentStringTokenType = TokenType.closedBrace;
						if (c.val == '(')
							currentStringTokenType = TokenType.openBrace;
						charsThatAppendToCurrentString = new HashSet<CharType>() {};
						syntaxIllegalCharTypes = new HashSet<CharType>() { };
						break;
					case CharType.letter:
						currentStringTokenType = TokenType.charString;
						charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.letter, CharType.number };
						syntaxIllegalCharTypes = new HashSet<CharType>() { };
						break;
					case CharType.number:
						//We're not dealing with a word - we're dealing with a number or +/-
						if (currentStringTokenType != TokenType.charString) {
							currentStringTokenType = TokenType.number;
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.number, CharType.period };
							syntaxIllegalCharTypes = new HashSet<CharType>() { };
						} else {
							currentStringTokenType = TokenType.charString;
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.letter, CharType.number };
							syntaxIllegalCharTypes = new HashSet<CharType>() { };
						}
						break;
					case CharType.period:
						if (currentStringTokenType != TokenType.charString && decimalNumber == false) {
							currentStringTokenType = TokenType.number;
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.number };
							syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.period  };
							decimalNumber = true;
						} else {
							currentStringTokenType = TokenType.charString;
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.letter, CharType.number };
							syntaxIllegalCharTypes = new HashSet<CharType>() { };
						}
						break;
					case CharType.plusOrMinusSign:
						currentStringTokenType = TokenType.infixOperator;
						charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.number };
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.comma, CharType.suffixOp };
						break;
					case CharType.comma:
						currentStringTokenType = TokenType.argSeperator;
						charsThatAppendToCurrentString = new HashSet<CharType>() { };
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.comma };
						break;
					case CharType.whitespace:
						tokenString = string.Empty;
						break;
				}
				return tokenToReturn;
			}
			TokenType currentStringTokenType;
		}

		currentCharString tokenString = new currentCharString();
		public Tokens Scan() {
			IToken tokenToAdd;
			for(int i = 0; i < compilerInput.Count(); i++){
				char c = compilerInput[i];
				tokenToAdd = (IToken)tokenString.AddChar(new currentChar(c));
				if (tokenToAdd != null) {
					if (tokenToAdd.Type == TokenType.infixOperator && i == 1)
						allTokens.Add(new KeywordToken("ANS"));
					allTokens.Add(tokenToAdd);
				}
			}
			//This publishes any content left over at the end of token creation
			if (tokenString.tokenString.Count() > 0) {
				tokenToAdd = tokenString.PublishCurrentTokenString();
				if(tokenToAdd != null)
					allTokens.Add(tokenToAdd);
			} else {
				ErrorLog.Add(new ErrorMessage("No tokens deterimed"));
			}
			return allTokens;
		}
	}
}
