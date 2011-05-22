using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace PhysicsEngine.Numbers {
	/*
	public class Complex {
		BigNumber RealPart;
		BigNumber imaginaryPart;
		public Complex(BigNumber real, BigNumber imaginary){
			RealPart = real;
			imaginaryPart = imaginary;
		}
	}
	*/
	public static class ComplexExtensions {
		public static string FullVisualization(this Complex num){
			string output = string.Empty;
			output += num.Real.ToString();
			if (num.Imaginary != double.MinValue && num.Imaginary != 0) {
				output += " +i" + num.Imaginary.ToString(); 
			}
			return output;	
		}
		public static Complex Factorial(this Complex num) {
			if (num.Imaginary != double.MinValue)
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored"));
			if(Math.Floor(num.Real) != num.Real)
				ErrorLog.Add(new ErrorMessage("Rounded to the nearest integer"));
			return new Complex(MathNet.Numerics.Combinatorics.Permutations((int)num.Real), double.MinValue);
		}
		public static Complex Modulus(this Complex num1, Complex num2) {
			if (num1.Imaginary != double.MinValue)
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored for first parameter"));
			if (num2.Imaginary != double.MinValue)
				ErrorLog.Add(new ErrorMessage("Imaginary part ignored for second parameter"));
			return new Complex(num1.Real % num2.Real, double.MinValue);
		}
	}
}
