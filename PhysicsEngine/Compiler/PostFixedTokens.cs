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
			while (operatorStack.Count() > 0 && operatorStack.First().TokenType == TokenType.arithmeticOp
				&& precedenceTest(operatorStack.First().TokenString, token.TokenString)) {
				tokens.Add(operatorStack.Pop());
			}
			operatorStack.Push(token);
		}

		Stack<Token> operatorStack = new Stack<Token>();
		public PostfixedTokens(List<Token> inputTokens) {
			foreach (Token token in inputTokens) {
				if (token.TokenType == TokenType.number || token.TokenType == TokenType.variable) {
					tokens.Add(token);
				}
				if (token.TokenType == TokenType.function) {
					operatorStack.Push(token);
				}
				if (token.TokenType == TokenType.syntaxChar) {
					if (operatorStack.Count() == 0) {
						ErrorLog.Add(new ErrorMessage(token.TokenString + " operator syntax error."));
					} else {
						while (operatorStack.First().TokenType != TokenType.openBrace)
							tokens.Add(operatorStack.Pop());
					}
				}
				if (token.TokenType == TokenType.arithmeticOp) {
					handleOperator(token);
				}
				if (token.TokenType == TokenType.openBrace) {
					operatorStack.Push(token);
				}
				if (token.TokenType == TokenType.closedBrace) {
					while (operatorStack.First().TokenType != TokenType.openBrace) {
						tokens.Add(operatorStack.Pop());
						//If no parenthesis found, mismatched parenthesis exception
					}
					operatorStack.Pop();
				}
			}
			while (operatorStack.Count() > 0)
				tokens.Add(operatorStack.Pop());
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
					case TokenType.arithmeticOp:
						parseTree.AppendOperator(token.TokenString);
						break;
					case TokenType.variable:
						if (!parseTree.AppendVariable(token.TokenString)) {
							return null;
						}
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
