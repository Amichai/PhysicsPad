using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using MathNet.Numerics;
using System.Numerics;

namespace Compiler {
	public static class Functions {
		//TODO: If the function typed doesn't exist in the library than don't evaluate anything
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"SUM","SIN","COS","TAN","ABS","SQRT","POW","INVCOS","INVSIN","INVTAN","CONVERT"
		};
		public static Complex Sum(List<Complex> values) {
			Complex returnVal = 0;
			foreach (Complex val in values) {
				returnVal += val;
			}
			return returnVal;
		}
	}
	public interface IFunction {
		Complex Compute(List<Complex> values);
	}

	public class Sum : IFunction {
		public Complex Compute(List<Complex> values) {
			Complex returnVal = 0;
			foreach (Complex val in values) {
				returnVal += val;
			}
			return returnVal;
		}
	}
	public class Sin : IFunction {
		 public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.Sine(value.First());			
		}
	}
	public class Cos : IFunction {
		 public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.Cosine(value.First());
		}
	}
	public class Tan : IFunction {
		public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.Tangent(value.First());
		}
	}
	public class InvSin : IFunction{
		 public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.InverseSine(value.First());
		}
	}
	public class InvCos : IFunction {
		public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.InverseCosine(value.First());
		}
	}
	public class InvTan : IFunction {
		public Complex Compute(List<Complex> value) {
			return MathNet.Numerics.Trig.InverseTangent(value.First());
		}
	}
	public class Abs : IFunction {
		public Complex Compute(List<Complex> value) {
			return Complex.Abs(value.First());
		}
	}
	public class Sqrt : IFunction {
		 public Complex Compute(List<Complex> value) {
			return Complex.Sqrt(value.First());
		}
	}
	public class Pow : IFunction {
		 public Complex Compute(List<Complex> values) {
			return values[1].Power(values[0]);
		}
	}
	public class Convert : IFunction {
		public Complex Compute(List<Complex> values) {
			throw new NotImplementedException();
		}
	}
}
