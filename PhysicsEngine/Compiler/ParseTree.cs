using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using System.Diagnostics;

namespace PhysicsEngine.Compiler {
	public abstract class ParseTree {
		public enum nodeType { number, function, operation, variable }
		public bool numericalEvaluation;
		public List<TreeNode> children = new List<TreeNode>();
		public string name = string.Empty;
		//public double val = int.MinValue;
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
		internal void AppendNumber(double tokenVal) {
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
		internal void AppendOperator(string tokenString) {
			//Will always take an operation and append two numbers as children
			//This is necessitated by the postfixed token construction
			const int numberOfChildLeafs = 2; //For primitive operations, this number is 2. For functions its variable
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
				child.val = new Value(
							postFixedOperatorEvaluator(childLeafNodes.Select(i => i.val.deciValue).ToList(),
																tokenString),
							new Factors(childLeafNodes.Select(i => i.val.factors).ToList()),
							Restrictions.none);
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

		//TODO: Division
		//TODO: when multiplying two numbers, don't recalculate prime factors, but combine the two lists of factors

		//TODO: This should be from the Value class not doubles
		double postFixedOperatorEvaluator(List<double> values, string tokenString) {
			//TODO: Solve these problems in cases that cannot be evaluated numerically.
			//Possibly extend the Value type for non-numerical evaluation.
			double returnVal = values.Last();
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
					break;
				case "/":
					returnVal /= values[i];
					break;
				case "%":
					returnVal %= values[i];
					break;
				case "^":
					returnVal = Math.Pow(returnVal, values[i]);
					if (values.Count() > 2)
						throw new Exception("Can't have more than two parameters for the power method.");
					break;
				default:
					throw new Exception("unknown operator");
			}
			if (returnVal == int.MinValue)
				throw new Exception("No evaluation happened");
			return returnVal;
		}

		internal bool AppendVariable(string p) {
			if (p == "ans") {
				TreeNode child = new TreeNode();
				child.type = nodeType.number;
				double tokenVal = OutputLog.returnValues.Last().deciValue;
				child.val = new Value(tokenVal, Restrictions.none);
				child.name = tokenVal.ToString();
				child.numericalEvaluation = true;
				children.Insert(0, child);
				//clear the static visualization output string:
				output = string.Empty;
				return true;
			} else {
				ErrorLog.Add(new ErrorMessage("Unknown variable can't be appendend"));
				return false;
			}
		}
	}
		
	//TODO: ParseTreeManipulationMethods
	//Flatten out teired addition -
	//Find common factors over an addition problem
	//Distribute multiplication over addition
	//Addition and multiplication can be rearranged (commutative)
	//Build fractions instead of evaluation
	

}
