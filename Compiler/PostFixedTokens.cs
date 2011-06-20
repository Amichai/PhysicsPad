using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SystemLogging;

namespace Compiler {
	public class PostfixedTokens : Tokens{
		private void handleOperator(IToken token) {
			//If the current op has higher precedence, add to the stack
			//true if the last operator on the stack has precedence over the current operator
			while (operatorStack.Count() > 0 && operatorStack.First().Type == TokenType.infixOperator
				&& precedenceTest(operatorStack.First().TokenString, token.TokenString)) {
					InAList.Add(operatorStack.Pop());
			}
			operatorStack.Push(token);
		}

		List<int> numberOfFunctionParameters = new List<int>();
		Stack<IToken> operatorStack = new Stack<IToken>();
		public PostfixedTokens(List<IToken> inputTokens) {
			foreach (IToken token in inputTokens) {
				switch (token.Type) {
					case TokenType.number:
						InAList.Add(token);
						break;
					case TokenType.variable:
						InAList.Add(token);
						break;
					case TokenType.function:
						operatorStack.Push(token);
						numberOfFunctionParameters.Add(0);
						break;
					case TokenType.argSeperator:
						if (operatorStack.Count() == 0) {
							ErrorLog.Add(new ErrorMessage(token.TokenString + " operator syntax error."));
						} else {
							if (numberOfFunctionParameters.Count > 0)
								numberOfFunctionParameters[numberOfFunctionParameters.Count - 1] = numberOfFunctionParameters.Last() + 1;
							while (operatorStack.First().Type != TokenType.openBrace) {
								InAList.Add(operatorStack.Pop());
							}
						}
						break;
					case TokenType.infixOperator:
						handleOperator(token);
						break;
					case TokenType.suffixOperator:
						InAList.Add(token);
						break;
					case TokenType.openBrace:
						operatorStack.Push(token);
						break;
					case TokenType.closedBrace:
						if(numberOfFunctionParameters.Count() > 0)
							numberOfFunctionParameters[numberOfFunctionParameters.Count - 1] = numberOfFunctionParameters.Last() + 1;
						while (operatorStack.First().Type != TokenType.openBrace) {
							if (operatorStack.Count() == 0) {
								ErrorLog.Add(new ErrorMessage("mismatched parenthesis error"));
							}
							InAList.Add(operatorStack.Pop());
						}
						operatorStack.Pop(); //Pop the left parenthesis off the stack
						if (operatorStack.Count > 0 && operatorStack.First().Type == TokenType.function) {
							FunctionToken tokenToAdd = (FunctionToken)operatorStack.Pop();
							tokenToAdd.numberOfChildren = numberOfFunctionParameters.Last();
							numberOfFunctionParameters.RemoveAt(numberOfFunctionParameters.Count() - 1);
							InAList.Add(tokenToAdd);
						}
						break;
					case TokenType.timeUnit:
						InAList.Add(token);
						break;
					case TokenType.volumeUnit:
						InAList.Add(token);
						break;
					case TokenType.weightUnit:
						InAList.Add(token);
						break;
				}
			}
			while (operatorStack.Count() > 0) {
				InAList.Add(operatorStack.Pop());
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

		public override void Add(IToken token) {
			InAList.Add(token);
		}
		
		public TreeNode BuildParseTree() {
			TreeNode parseTree = new TreeNode();
			foreach (IToken token in InAList) {
				switch (token.Type) {
					case TokenType.number:
						parseTree.AppendNumber((NumberToken)token);
						break;
					case TokenType.infixOperator:
						parseTree.AppendFunction(token);
						break;
					case TokenType.suffixOperator:
						parseTree.AppendFunction(token);
						break;
					case TokenType.variable:
						if (!parseTree.AppendVariable(token.TokenString.ToUpper())) {
							return null;
						}
						break;
					case TokenType.function:
						parseTree.AppendFunction((FunctionToken)token);
						break;
					case TokenType.massUnit:
						parseTree.AppendNumber((NumberToken)token);
						break;
					case TokenType.volumeUnit:
						break;
					case TokenType.weightUnit:
						break;
					case TokenType.speedUnit:
						break;
					case TokenType.timeUnit:
						break;
					default:
						ErrorLog.Add(new ErrorMessage("Fatal parsing error: this token type is unknown cannot be appended to the parse tree"));
						return null;
				}
			}
			if (parseTree.children.Count() > 0)
				//The root node is always redundant by construction
				return parseTree.children.First();
			else return null;
		}
	}
}
