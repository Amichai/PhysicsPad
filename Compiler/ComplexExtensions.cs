using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using MathNet.Numerics;
using SystemLogging;

namespace Compiler {
	static public class ComplexExtensions {
		public static string FullVisualization(this Complex num) {
			string output = string.Empty;
			output += num.Real.ToString();
			if(!num.IsReal()) {
				output += " +i" + num.Imaginary.ToString();
			}
			return output;
		}
		public static Complex Factorial(this Complex num) {
			if (!num.IsReal())
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored"));
			if (Math.Floor(num.Real) != num.Real)
				ErrorLog.Add(new ErrorMessage("Rounded to the nearest integer"));
			return new Complex(MathNet.Numerics.Combinatorics.Permutations((int)num.Real), 0);
		}
		public static Complex Modulus(this Complex num1, Complex num2) {
			if (!num1.IsReal())
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored for first parameter"));
			if (!num2.IsReal())
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored for second parameter"));
			return new Complex(num1.Real % num2.Real, 0);
		}
	}
}
