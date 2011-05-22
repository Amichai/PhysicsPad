using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using BigRationalNumerics;

namespace MathNet.Numerics {
	public enum NumberType2 { integer, rational, irrational };
	/// <summary>
	/// A single number, can be either integer, rational or irrational.
	/// Factors are stored for integers.
	/// </summary>
	public class BigNumber {
		public NumberType2 Type;
		//TODO: A constructor that takes a string would be nice
		#region Constructors
		public BigNumber(int val) {
			integerVal = new BigInt(val);
			Type = NumberType2.integer;
		}

		public BigNumber(BigInteger val) {
			integerVal = new BigInt(val);
			Type = NumberType2.integer;
		}

		public BigNumber(double val) {
			if (val == Math.Floor(val)) {
				integerVal = new BigInt((int)val);
				Type = NumberType2.integer;
			} else {
				rationalVal = new BigRational(val);
				Type = NumberType2.rational;
			}
		}

		public BigNumber(BigRational val){
			Type = NumberType2.rational;
			rationalVal =  val;
		}

		public BigNumber(double val, NumberType2 type) {
			Type = type;
			switch (type) {
				case NumberType2.integer:
					integerVal = new BigInt((int)val);
					break;
				case NumberType2.rational:
					rationalVal = new BigRational(val);
					break;
				case NumberType2.irrational:
					irrationalVal = new BigIrrational(val);
					break;
			}
		}
		public BigNumber(float val, NumberType2 type) {
			Type = type;
			switch (type) {
				case NumberType2.integer:
					integerVal = new BigInt((int)val);
					break;
				case NumberType2.rational:
					rationalVal = new BigRational(val);
					break;
				case NumberType2.irrational:
					irrationalVal = new BigIrrational(val);
					break;
			}
		}
		public BigNumber(Decimal val, NumberType2 type) {
			Type = type;
			switch (type) {
				case NumberType2.integer:
					integerVal = new BigInt((int)val);
					break;
				case NumberType2.rational:
					rationalVal = new BigRational(val);
					break;
				case NumberType2.irrational:
					irrationalVal = new BigIrrational((double)val);
					break;
			}
		}
		public BigNumber(BigInteger val, NumberType2 type) {
			Type = type;
			switch (type) {
				case NumberType2.integer:
					integerVal = new BigInt(val);
					break;
				case NumberType2.rational:
					rationalVal = new BigRational(val);
					break;
				case NumberType2.irrational:
					irrationalVal = new BigIrrational((double)val);
					break;
			}
		}
		public BigNumber(BigRational val, NumberType2 type) {
			Type = type;
			switch (type) {
				case NumberType2.integer:
					integerVal = new BigInt((int)val);
					break;
				case NumberType2.rational:
					rationalVal = val;
					break;
				case NumberType2.irrational:
					irrationalVal = new BigIrrational((double)val);
					break;
			}
		}
		#endregion

		private BigInt integerVal;
		private BigRational rationalVal;
		private BigIrrational irrationalVal;
		//TODO: Get rid of NumberType in favor of NumberType2
		//TODO: eliminate the old value and Factors class

		public double Eval() {
			switch (Type) {
				case NumberType2.integer:
					return (double)integerVal.Val;
				case NumberType2.rational:
					return (double)rationalVal.Numerator / (double)rationalVal.Denominator;
				case NumberType2.irrational:
					return irrationalVal.GetVal();
				default:
					throw new Exception("Type unknown");
			}
		}

		public BigInteger GetIntVal() {
			switch (Type) {
				case NumberType2.integer:
					return integerVal.Val;
				default:
					throw new Exception("Not an int");
			}
		}

		internal BigNumber Abs() {
			switch (Type) {
				case NumberType2.integer:
					if (integerVal.Val < 0)
						return new BigNumber((int)-integerVal.Val);
					else
						return this;
				case NumberType2.rational:
					if (rationalVal < 0)
						return new BigNumber(-rationalVal, NumberType2.rational);
					else
						return this;
				case NumberType2.irrational:
					if (irrationalVal.NumericalContent.ToString().Count() < irrationalVal.DecimalLocation) {
						this.irrationalVal.FlipSign();
					} 
					return this;
				default:
					throw new Exception("Type unknown");
			}
		}
		/*
		public BigNumber operator -(BigNumber subtrahend) {
			switch (Type) {
				case NumberType2.integer:
					return new BigNumber((int)-integerVal.Val);
				case NumberType2.rational:
					return new BigNumber(-rationalVal, NumberType2.rational);
				case NumberType2.irrational:
					this.irrationalVal.FlipSign();
					return this;
				default:
					throw new Exception("Type unknown");
			}
		}*/

		public static BigNumber Zero { get{return new BigNumber((int)0);
			} }

		internal BigNumber Negative() {
			switch (Type) {
				case NumberType2.integer:
					return new BigNumber((int)-integerVal.Val);
				case NumberType2.rational:
					return new BigNumber(-rationalVal, NumberType2.rational);
				case NumberType2.irrational:
					this.irrationalVal.FlipSign();
					return this;
				default:
					throw new Exception("Type unknown");
			}

		}
	}
}
