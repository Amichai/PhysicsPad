using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using BigRationalNumerics;

namespace PhysicsEngine.Numbers {
	public enum NumberType2 { integer, rational, irrational };
	/// <summary>
	/// A single number, can be either integer, rational or irrational.
	/// Factors are stored for integers.
	/// </summary>
	public class BigNumber {
		NumberType2 Type;
		//TODO: A constructor that takes a string would be nice
		#region Constructors
		public BigNumber(int val) {
			integerVal = new BigInt(val);
			Type = NumberType2.integer;
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

		public BigInt integerVal;
		public BigRational rationalVal;
		public BigIrrational irrationalVal;
		//TODO: Get rid of NumberType in favor of NumberType2
		//TODO: eliminate the old value and Factors class
	}
}
