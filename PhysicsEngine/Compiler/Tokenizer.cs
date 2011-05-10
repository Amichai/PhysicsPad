using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PhysicsEngine {
	public enum TokenType { number, function, charString, arithmeticOp, syntaxChar, empty, closedBrace, openBrace, equalSign, variable, suffixOp }
	public enum CharType { number, letter, infixArithmeticOp, syntaxChar, period, plusOrMinusSign, brace, whitespace, suffixOp }
	class Tokenizer {
		public readonly static HashSet<char> syntaxChars = new HashSet<char>() { ',', '{', '}' };
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
				if (syntaxChars.Contains(c))
					currentCharTokenType = CharType.syntaxChar;
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

			public Token Flush() {
				Token tokenToReturn = null;
				tokenToReturn = new Token(tokenString, currentStringTokenType);
				return tokenToReturn;
			}

			public Token AddChar(currentChar c) {
				Token tokenToReturn  = null;
				//check if the char is legal in this context
				if (syntaxIllegalCharTypes.Contains(c.currentCharTokenType)) {
					ErrorLog.Add(new ErrorMessage("Syntax illegal char cannot be added to token"));
					return null;
				}
				//The char is legal. Check we if we should publish the current string
				if (!charsThatAppendToCurrentString.Contains(c.currentCharTokenType) && tokenString.Count() > 0) {
					tokenToReturn = new Token(tokenString, currentStringTokenType);
					tokenString = string.Empty;
				}
				//Append Char
				tokenString += c.val;
				//Depending on the current char type:
				//Set the local string type and other currenttoken state values
				//Set publication types and char legal types
				switch (c.currentCharTokenType) {
					case CharType.infixArithmeticOp:
						currentStringTokenType = TokenType.arithmeticOp;
						//Set the value of the publication type, etc.
						//Publish on every char type
						charsThatAppendToCurrentString = new HashSet<CharType>() {};
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.syntaxChar, CharType.suffixOp };
						break;
					case CharType.suffixOp:
						currentStringTokenType = TokenType.suffixOp;
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
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.number, CharType.letter, CharType.period };
							syntaxIllegalCharTypes = new HashSet<CharType>() { };
						} else {
							currentStringTokenType = TokenType.charString;
							charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.letter, CharType.number };
							syntaxIllegalCharTypes = new HashSet<CharType>() { };
						}
						break;
					case CharType.plusOrMinusSign:
						currentStringTokenType = TokenType.arithmeticOp;
						charsThatAppendToCurrentString = new HashSet<CharType>() { CharType.number };
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.syntaxChar, CharType.suffixOp };
						break;
					case CharType.syntaxChar:
						currentStringTokenType = TokenType.syntaxChar;
						charsThatAppendToCurrentString = new HashSet<CharType>() { };
						syntaxIllegalCharTypes = new HashSet<CharType>() { CharType.infixArithmeticOp, CharType.syntaxChar };
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
			Token tokenToAdd;
			for(int i = 0; i < compilerInput.Count(); i++){
				char c = compilerInput[i];
				tokenToAdd = tokenString.AddChar(new currentChar(c));
				if (tokenToAdd != null) {
					if (tokenToAdd.TokenType == TokenType.arithmeticOp && i == 1)
						allTokens.Add(new Token("ans", TokenType.variable));
					allTokens.Add(tokenToAdd);
				}
			}
			//This publishes any content left over at the end of token creation
			if (tokenString.tokenString.Count() > 0) {
				tokenToAdd = tokenString.Flush();
				if(tokenToAdd != null)
					allTokens.Add(tokenToAdd);
			} else {
				ErrorLog.Add(new ErrorMessage("No tokens deterimed"));
			}
			return allTokens;
		}
	}
}
