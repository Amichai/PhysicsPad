using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using MathNet.Numerics;
using BigRationalNumerics;
using System.Numerics;

namespace PhysicsEngine.ReferenceLibraries {
	static class Functions {
		//TODO: If the function typed doesn't exist in the library than don't evaluate anything
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"SUM","SIN","COS","TAN","ABS","SQRT","POW","INVCOS","INVSIN","INVTAN",
		};
		public static Complex Sum(List<Complex> values) {
			Complex returnVal = 0;
			foreach (Complex val in values) {
				returnVal += val;
			}
			return returnVal;
		}
		internal static Complex Sin(Complex value) {
			return MathNet.Numerics.Trig.Sine(value);
		}
		internal static Complex Cos(Complex value) {
			return MathNet.Numerics.Trig.Cosine(value);
		}
		internal static Complex Tan(Complex value) {
			return MathNet.Numerics.Trig.Tangent(value);
		}

		internal static Complex InvSin(Complex value) {
			return MathNet.Numerics.Trig.InverseSine(value);
		}
		internal static Complex InvCos(Complex value) {
			return MathNet.Numerics.Trig.InverseCosine(value);
		}
		internal static Complex InvTan(Complex value) {
			return MathNet.Numerics.Trig.InverseTangent(value);
		}
		internal static Complex Abs(Complex value) {
			return Complex.Abs(value);
		}
		internal static Complex Sqrt(Complex value) {
			return Complex.Sqrt(value);
		}
		internal static Complex Pow(Complex p1, Complex p2) {
			return MathNet.Numerics.ComplexExtensions.Power(p1.Real, p2.Real);
		}

	}
}
