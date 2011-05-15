using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using MathNet.Numerics;
using BigRationalNumerics;

namespace PhysicsEngine.ReferenceLibraries {
	static class Functions {
		//TODO: If the function typed doesn't exist in the library than don't evaluate anything
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"SUM","SIN","COS","TAN","ABS","SQRT","POW","INVCOS","INVSIN","INVTAN",
		};
		public static Value Sum(List<BigRational> values) {
			BigRational returnVal = 0;
			foreach (BigRational val in values) {
				returnVal += val;
			}
			return new Value(returnVal, Restrictions.none);
		}
		//TODO: Improve the system for flagging irrational numbers to not be computed as decimals
		internal static Value Sin(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.Sine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value Cos(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.Cosine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value Tan(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.Tangent(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

		internal static Value InvSin(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.InverseSine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value InvCos(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.InverseCosine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value InvTan(double value) {
			BigRational returnVal = new BigRational(MathNet.Numerics.Trig.InverseTangent(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

		internal static Value Abs(double value) {
			BigRational returnVal = new BigRational(Math.Abs(value));
			return new Value(returnVal, Restrictions.none);
		}

		internal static Value Sqrt(double value) {
			//TODO: The return value of this function and the pow value needs to be complex
			System.Numerics.Complex complexVal = new System.Numerics.Complex(value, 0);
			BigRational returnVal = new BigRational(MathNet.Numerics.ComplexExtensions.SquareRoot(complexVal).Real);
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

		internal static Value Pow(double p1, double p2) {
			System.Numerics.Complex complexVal1 = new System.Numerics.Complex(p1, 0);
			System.Numerics.Complex complexVal2 = new System.Numerics.Complex(p2, 0);
			BigRational returnVal = new BigRational(MathNet.Numerics.ComplexExtensions.Power(complexVal1.Real, complexVal2.Real).Real);
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

	}
}
