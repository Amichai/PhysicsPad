using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Compiler;
using System.Diagnostics;

namespace PhysicsEngine {
	public class PostfixedTokens : Tokens{
		private void handleOperator(Token token) {
			//If the current op has higher precedence, add to the stack
			//true if the last operator on the stack has precedence over the current operator
			while (operatorStack.Count() > 0 && operatorStack.First().TokenType == TokenType.infixOperator
				&& precedenceTest(operatorStack.First().TokenString, token.TokenString)) {
				tokens.Add(operatorStack.Pop());
			}
			operatorStack.Push(token);
		}

		List<int> numberOfFunctionParameters = new List<int>();
		Stack<Token> operatorStack = new Stack<Token>();
		public PostfixedTokens(List<Token> inputTokens) {
			foreach (Token token in inputTokens) {
				if (token.TokenType == TokenType.number || token.TokenType == TokenType.variable) {
					tokens.Add(token);
				}
				if (token.TokenType == TokenType.function) {
					operatorStack.Push(token);
					numberOfFunctionParameters.Add(0);
				}
				//TODO: Make a new token type called argument seperator
				if (token.TokenType == TokenType.argSeperator) { //","
					if (operatorStack.Count() == 0) {
						ErrorLog.Add(new ErrorMessage(token.TokenString + " operator syntax error."));
					} else {
						if(numberOfFunctionParameters.Count > 0)
							numberOfFunctionParameters[numberOfFunctionParameters.Count - 1] = numberOfFunctionParameters.Last() + 1;
						while (operatorStack.First().TokenType != TokenType.openBrace) {
							tokens.Add(operatorStack.Pop());
						}
					}
				}
				if (token.TokenType == TokenType.infixOperator) {
					handleOperator(token);
				}
				if (token.TokenType == TokenType.suffixOperator) {
					tokens.Add(token);
				}
				if (token.TokenType == TokenType.openBrace) {
					operatorStack.Push(token);
				}
				if (token.TokenType == TokenType.closedBrace) {
					Debug.Print(operatorStack.First().TokenType.ToString());
					if(numberOfFunctionParameters.Count() > 0)
						numberOfFunctionParameters[numberOfFunctionParameters.Count - 1] = numberOfFunctionParameters.Last() + 1;
					while (operatorStack.First().TokenType != TokenType.openBrace) {
						if (operatorStack.Count() == 0) {
							ErrorLog.Add(new ErrorMessage("mismatched parenthesis error"));
						}
						tokens.Add(operatorStack.Pop());
					}
					operatorStack.Pop(); //Pop the left parenthesis off the stack
					if (operatorStack.Count > 0 && operatorStack.First().TokenType == TokenType.function) {
						Token tokenToAdd = operatorStack.Pop();
						tokenToAdd.numberOfChildren = numberOfFunctionParameters.Last();
						numberOfFunctionParameters.RemoveAt(numberOfFunctionParameters.Count() - 1);
						tokens.Add(tokenToAdd);
					}
				}
			}
			while (operatorStack.Count() > 0) {
				tokens.Add(operatorStack.Pop());
			}
			//TODO: Handle mismatched bracket exception
		}

		private int getOperatorValue(string op) {
			int opValue = int.MinValue;
			if (op == "(") {
				opValue = 0;
			}

			if (op == "+" ||
				op == "-") {
				opValue = 1;
			}
			if (op == "*" ||
				op == "/" ||
				op == "%") {
				opValue = 2;
			}
			if (op == "^") {
				opValue = 3;
			}
			if (opValue == int.MinValue)
				throw new Exception("Unable to evaluate operator value");
			return opValue;
		}

		List<string> rightAssociative = new List<string>() { "^" };
		private bool precedenceTest(string op1, string op2) {
			int op1Value = getOperatorValue(op1);
			int op2Value = getOperatorValue(op2);
			if (op1Value < op2Value)
				return false;
			if (op1Value > op2Value)
				return true;
			if (op1Value == op2Value){
				if (rightAssociative.Contains(op2))
					return false;
				else
					return true;
			}
			throw new Exception("Unhandled");
		}

		public override void Add(Token token) {
			tokens.Add(token);
		}
		
		public TreeNode BuildParseTree() {
			TreeNode parseTree = new TreeNode();
			foreach (Token token in tokens) {
				switch (token.TokenType) {
					case TokenType.number:
						parseTree.AppendNumber(token.TokenNumValue);
						break;
					case TokenType.infixOperator:
						parseTree.AppendOperator(token);
						break;
					case TokenType.suffixOperator:
						parseTree.AppendOperator(token);
						break;
					case TokenType.variable:
						if (!parseTree.AppendVariable(token.TokenString)) {
							return null;
						}
						break;
					case TokenType.function:
						parseTree.AppendOperator(token);
						break;
					default:
						throw new Exception("This token type cannot be appended to the parse tree");
				}
			}
			//The root node is always redundant by construction
			return parseTree.children.First();						
		}
	}
}
