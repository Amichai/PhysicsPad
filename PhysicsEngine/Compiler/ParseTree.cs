using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using System.Diagnostics;
using MathNet.Numerics;
using System.Numerics;
using PhysicsEngine.ReferenceLibraries;
using BigRationalNumerics;

namespace PhysicsEngine.Compiler {
	public abstract class ParseTree {
		public enum nodeType { number, function, operation, variable }
		public bool numericalEvaluation;
		public List<TreeNode> children = new List<TreeNode>();
		public string name = string.Empty;
		public Value val;
		public nodeType type;
		internal static string output = "\n";

		public string Visualize(string indent, bool last) {
			output += indent;
			if (last) {
				output += "\\ ";
				indent += "  ";
			} else {
				output += "| ";
				indent += "| ";
			}
			output += name + ": " + val.GetValueToString() + "\n";

			int i = 0;
			foreach (TreeNode c in children) {
				c.Visualize(indent, i == children.Count - 1);
				i++;
			}
			return output;
		}
	}

	public class TreeNode : ParseTree{
		internal void AppendNumber(BigRational tokenVal) {
			TreeNode child = new TreeNode();
			child.type = nodeType.number;
			child.val = new Value(tokenVal, Restrictions.none);
			child.name = tokenVal.ToString();
			child.numericalEvaluation = true;
			children.Insert(0, child);
			//clear the static visualization output string:
			output = string.Empty;
		}

		/// <summary>
		/// Take each token string from the list of postfixedtokens and build a parse tree
		/// with each node evaluated when possible. 
		/// </summary>
		internal void AppendOperator(Token token) {
			string tokenString = token.TokenString;
			int numberOfChildLeafs = token.numberOfChildren;
			TreeNode child = new TreeNode();
			child.type = nodeType.operation;
			child.name = tokenString;
			TreeNode[] childLeafNodes = new TreeNode[numberOfChildLeafs];
			for (int i = 0; i < numberOfChildLeafs; i++) {
				if (children.Count() == 0)
					throw new Exception("Post fixed token notation error.");
				childLeafNodes[i] = children[0];
				children.RemoveAt(0);
				child.children.Insert(0, childLeafNodes[i]);
			}
			if (childLeafNodes.All(i => i.numericalEvaluation)) {
				child.numericalEvaluation = true;
				List<BigRational> paramaters = childLeafNodes.Select(i => i.val.RationalValue).ToList();
				if (token.TokenType == TokenType.suffixOperator || token.TokenType == TokenType.infixOperator)
					child.val = postFixedOperatorEvaluator(paramaters, tokenString);
				else if (token.TokenType == TokenType.function) {
					child.val = functionEvaluator(paramaters, tokenString);
				} else
					throw new Exception("Not operator or function can't evaluate");
			}
			flattenTieredAddOrMult(child);
			children.Insert(0, child);
		}

		/// <summary>
		/// For commutative operations its possible to have one operation node with many children
		/// instead of tiered iterations of the operation. This method is to change from the 
		/// latter to the former.</summary>
		private void flattenTieredAddOrMult(TreeNode node) {
			TreeNode adjustedNode = new TreeNode();
			switch (node.name) {
				case "+":
				for (int i = 0; i < node.children.Count(); i++) {
					if (node.children[i].name == "+") {
						adjustedNode = node.children[i];
						node.children.RemoveAt(i);
						foreach (TreeNode t in adjustedNode.children) {
							node.children.Add(t);
						}
					}
				}
				break;
				case "*":
				List<int> commonFactors = new List<int>();
				for(int i=0; i < node.children.Count(); i++){
					if (node.children[i].name == "*") {
						adjustedNode = node.children[i];
						node.children.RemoveAt(i);
						foreach (TreeNode t in adjustedNode.children) {
							node.children.Add(t);
						}
					}
				}
				if (this.numericalEvaluation) {
					
					//this means that all the children are numbers and we have evaluated
						//TODO: take out the common factors from val.factors
						//Make it work by using exponents
				}
				break;
			}
		}

		Value functionEvaluator(List<BigRational> values, string tokenString) {
			switch (tokenString) {
				case "SUM":
					return Functions.Sum(values);
				case "SIN":
					if (values.Count() != 1)
						throw new Exception("Sine only takes one argument");
					return Functions.Sin((double)values.First());
				case "COS":
					if (values.Count() != 1)
						throw new Exception("Cosine only takes one argument");
					return Functions.Cos((double)values.First());
				case "TAN":
					if (values.Count() != 1)
						throw new Exception("Tangent only takes one argument");
					return Functions.Tan((double)values.First());
				case "INVSIN":
					if (values.Count() != 1)
						throw new Exception("InvSine only takes one argument");
					return Functions.InvSin((double)values.First());
				case "INVCOS":
					if (values.Count() != 1)
						throw new Exception("InvCosine only takes one argument");
					return Functions.InvCos((double)values.First());
				case "INVTAN":
					if (values.Count() != 1)
						throw new Exception("InvTangent only takes one argument");
					return Functions.InvTan((double)values.First());	
				case "ABS":
					if (values.Count() != 1)
						throw new Exception("Abs() only takes one argument");
					return Functions.Abs((double)values.First());
				case "SQRT":
					if (values.Count() != 1)
						throw new Exception("Sqrt() only takes one argument");
					return Functions.Sqrt((double)values.First());
				case "POW":
					if (values.Count() != 2)
						throw new Exception("Pow() only takes two arguments");
					return Functions.Pow((double)values.First(), (double)values.Last());
				default:


					throw new Exception("Function not in function library");
			}
		}

		Value postFixedOperatorEvaluator(List<BigRational> values, string tokenString) {			
			//TODO: Solve these problems in cases that cannot be evaluated numerically.
			//Possibly extend the Value type for non-numerical evaluation.
			Factors factors;
			BigRational returnVal = values.Last();
			List<BigInteger> listOfFactors = new List<BigInteger>();
			if (tokenString == "!") {
				if (returnVal.Denominator != 1)
					ErrorLog.Add(new ErrorMessage("Rounded because cannot take the factorial of a non integer"));
				else {
					for (int i = 2; i < returnVal + 1; i++) {
						listOfFactors.Add(i);
					}
					returnVal = new BigRational(((BigInteger)Combinatorics.Permutations((int)returnVal.Numerator)), 1);
					factors = new Factors(listOfFactors);
					return new Value(returnVal, factors, Restrictions.dontFactorMe);
				}
				if (values.Count() > 1)
					throw new Exception("suffix notation can only have one child");
			}
			for (int i = values.Count() - 2; i >= 0; i--) 
				switch (tokenString) {
				case "+":
					returnVal += values[i];
					break;
				case "-":
					returnVal -= values[i];
					break;
				case "*":
					returnVal *= values[i];
					//TODO: Combine factors into a new factors list so the result doesn't need to be factored
					break;
				case "/":
					returnVal /= values[i];
					break;
				case "%":
					returnVal %= values[i];
					break;
				case "^":
					returnVal = BigRational.Pow(returnVal, (BigInteger)values[i]);
					if (values.Count() > 2)
						throw new Exception("Can't have more than two parameters for the power method.");
					break;
				case "Sum":
					returnVal += values[i];
					break;
				default:
					throw new Exception("unknown operator");
			}
			if (returnVal == int.MinValue)
				throw new Exception("No evaluation happened");
			return new Value(returnVal, Restrictions.none);
		}

		internal bool AppendVariable(string variableName) {
			TreeNode child = new TreeNode();
			child.type = nodeType.number;
			switch (variableName) {
				case "ANS":
					BigRational tokenVal = OutputLog.returnValues.Last().RationalValue;
					child.val = new Value(tokenVal, Restrictions.none);
					child.name = tokenVal.ToString();
					break;
				case "PI":
					child.val = Variable.PI;
					child.name = child.val.ToString();
					break;
				default:
					ErrorLog.Add(new ErrorMessage("Unknown variable can't be appendend"));
					return false;
			}
			
			child.numericalEvaluation = true;
			children.Insert(0, child);
			//clear the static visualization output string:
			output = string.Empty;
			return true;
		}
	}
	//TODO: ParseTreeManipulationMethods
	//Flatten out teired addition -
	//Find common factors over an addition problem
	//Distribute multiplication over addition
	//Addition and multiplication can be rearranged (commutative)
	//Build fractions instead of evaluation
}
