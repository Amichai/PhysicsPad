using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;

namespace PhysicsEngine.ReferenceLibraries {
	static class Functions {
		//TODO: If the function typed doesn't exist in the library than don't evaluate anything
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"SUM",
			"SIN",
			"COS",
			"TAN",
			"ABS",
			"SQRT",
			"POW"
		};
		public static Value Sum(List<Numerics.BigRational> values) {
			Numerics.BigRational returnVal = 0;
			foreach (Numerics.BigRational val in values) {
				returnVal += val;
			}
			return new Value(returnVal, Restrictions.none);
		}

		internal static Value Sin(double value) {
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)MathNet.Numerics.Trig.Sine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value Cos(double value) {
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)MathNet.Numerics.Trig.Cosine(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
		internal static Value Tan(double value) {
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)MathNet.Numerics.Trig.Tangent(value));
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

		internal static Value Abs(double value) {
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)Math.Abs(value));
			return new Value(returnVal, Restrictions.none);
		}

		internal static Value Sqrt(double value) {
			System.Numerics.Complex complexVal = new System.Numerics.Complex(value, 0);
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)MathNet.Numerics.ComplexExtensions.SquareRoot(complexVal).Real);
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}

		internal static Value Pow(double p1, double p2) {
			System.Numerics.Complex complexVal1 = new System.Numerics.Complex(p1, 0);
			System.Numerics.Complex complexVal2 = new System.Numerics.Complex(p2, 0);
			Numerics.BigRational returnVal = new Numerics.BigRational((Decimal)MathNet.Numerics.ComplexExtensions.Power(complexVal1.Real, complexVal2.Real).Real);
			Value valToReturn = new Value(returnVal, Restrictions.none);
			valToReturn.primaryNumType = NumberType.irrational;
			return valToReturn;
		}
	}
}
