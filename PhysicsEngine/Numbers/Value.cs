using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MathNet.Numerics;
using System.Numerics;

namespace PhysicsEngine.Numbers {
	public enum Restrictions { dontFactorMe, none };
	public enum NumberType { integer, rational, irrational, imaginary, exponent };
	//TODO: implement exponential form for saving and displaying factors
	public class Value {
		#region constructors
		public Value(Numerics.BigRational doubleVal, Restrictions restrictions) {
			InitDouble(doubleVal, restrictions);
		}
		public Value(Numerics.BigRational doubleVal, Factors factors, Restrictions restrictions) {
			InitDouble(doubleVal, restrictions);
			this.factors = factors;
		}
		public Value(Numerics.BigRational realPart, Numerics.BigRational imaginaryPart, NumberType type) {
			switch (type) {
				case NumberType.imaginary:
					InitComplexNum(realPart, imaginaryPart);
					break;
			}
		}
		public Value(Numerics.BigRational baseVal, Numerics.BigRational exponent, NumberType type, Restrictions restrictions) {
			switch (type) {
				case NumberType.exponent:
					InitExp(baseVal, exponent, restrictions);
					break;
			}
		}
		public Value(int numerator, int denomenator, NumberType type) {
			switch (type) {
				case NumberType.rational:
					InitFraction(numerator, denomenator);
					break;
			}
		}
		#endregion

		public NumberType primaryNumType { get; set; }
		public double rationalEvaluated;

		/// <summary>Decimal value</summary>
		public void InitDouble(Numerics.BigRational val, Restrictions restrictions) {
			this.RationalValue = val;
			this.primaryNumType = NumberType.rational;
			if (val == (BigInteger)(val)) {
				//this is done to avoid the accumulation of miniscule errors:
				RationalValue = (BigInteger)val;
				primaryNumType = NumberType.integer;
				if (restrictions != Restrictions.dontFactorMe) {
					factors = new Factors((new List<BigInteger>(){(BigInteger)RationalValue}));
				}
			}
			rationalEvaluated = (double)RationalValue.Numerator / (double)RationalValue.Denominator;
		}
		public Factors factors;

		/// <summary>Complex Numbers</summary>
		public void InitComplexNum(Numerics.BigRational realPart, Numerics.BigRational imaginaryPart) {
			this.realPart = new Value(realPart, Restrictions.none);
			this.imaginaryPart = new Value(imaginaryPart, Restrictions.none);
			this.RationalValue = realPart;
			this.primaryNumType = NumberType.imaginary;

		}

		/// <summary>Fraction</summary>
		public void InitFraction(BigInteger numerator, BigInteger denominator) {
			RationalValue = new Numerics.BigRational(numerator, denominator);
			this.primaryNumType = NumberType.rational;
		}

		/// <summary>Exponent</summary>
		public void InitExp(Numerics.BigRational expBase, Numerics.BigRational expPower, Restrictions restrictionsToPass) {
			this.ExpBase = new Value(expBase, restrictionsToPass);
			this.ExpPower = new Value(expPower, restrictionsToPass);
			this.RationalValue = (Numerics.BigRational.Pow(expBase, (BigInteger)expPower));
			primaryNumType = NumberType.exponent;
		}
		
		public Numerics.BigRational RationalValue = double.MinValue;
		
		public Value realPart { get; set; }
		public Value imaginaryPart { get; set; }
		public Value ExpBase { get; set; }
		public Value ExpPower { get; set; }

		public string GetValueToString() {
			if (primaryNumType == NumberType.rational || primaryNumType == NumberType.integer) {
				return RationalValue.ToString();
			} else if (primaryNumType == NumberType.irrational) {
				return rationalEvaluated.ToString();
			} else if (primaryNumType == NumberType.imaginary) {
				return realPart.GetValueToString() + " " + imaginaryPart.GetValueToString() + "i";
			} else if (primaryNumType == NumberType.exponent) {
				return ExpBase.GetValueToString() + "^" + ExpPower.GetValueToString();
			}
			throw new Exception("Unkonwn number type");
		}

		public string FullVisualization() {
			string output = string.Empty;
			output += GetValueToString() + " ";
			if(factors != null)
				output += factors.Visualize();
			output += "\n";
			return output;
		}

		private enum Sign { positive, negative };
		private Value decimalToFraction(Numerics.BigRational Decimal) {
			BigInteger fractionNumerator = int.MaxValue;
			BigInteger fractionDenominator = 1;
			Numerics.BigRational accuracyFactor = .0000001;
			int decimalSign;
			Numerics.BigRational Z;
			BigInteger previousDenominator;
			BigInteger scratchValue;

			if (Decimal < 0) {
				decimalSign = -1;
			} else decimalSign = 1;
			Decimal = Numerics.BigRational.Abs(Decimal);
			if (Decimal == (BigInteger)Decimal) {
				fractionNumerator = (int)Decimal * decimalSign;
				fractionDenominator = 1;
				return new Value(fractionDenominator, fractionNumerator, NumberType.rational);
			}
			Z = Decimal;
			previousDenominator = 0;
			while (!(Z == (BigInteger)Z) && (Numerics.BigRational.Abs((Decimal - ((Numerics.BigRational)fractionNumerator / fractionDenominator))) > accuracyFactor)) {
				Z = 1/(Z - (BigInteger)Z);
				scratchValue = fractionDenominator;
				fractionDenominator = fractionDenominator*(BigInteger)Z + previousDenominator;
				previousDenominator = scratchValue;
				fractionNumerator = (BigInteger)(Decimal*fractionDenominator + .5);
			} 
			fractionNumerator  = decimalSign * fractionNumerator;
			return new Value(fractionNumerator, fractionDenominator, NumberType.rational);
		}
	}
}
