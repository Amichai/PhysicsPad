using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MathNet.Numerics;
using System.Numerics;

namespace PhysicsEngine.Numbers {
	public enum Restrictions { dontFactorMe, dontSetToFraction, dontFactorDontSetFraction, none };
	public enum NumberType { integer, deci, fractional, imaginary, exponent };
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
				case NumberType.fractional:
					InitFraction(numerator, denomenator);
					break;
			}
		}
		#endregion

		private NumberType primaryNumType { get; set; }

		/// <summary>Decimal value</summary>
		public void InitDouble(Numerics.BigRational val, Restrictions restrictions) {
			this.deciValue = val;
			if (val == (BigInteger)(val)) {
				//this is done to avoid the accumulation of miniscule errors:
				deciValue = (BigInteger)val;
				primaryNumType = NumberType.integer;
				if (restrictions != Restrictions.dontFactorMe && restrictions != Restrictions.dontFactorDontSetFraction) {
					factors = new Factors((new List<BigInteger>(){(BigInteger)deciValue}));
				}
			}else if (restrictions != Restrictions.dontSetToFraction && restrictions != Restrictions.dontFactorDontSetFraction) {
				asAFraction = decimalToFraction(deciValue);
			}			
			this.primaryNumType = NumberType.deci;
		}
		public Factors factors;

		/// <summary>Complex Numbers</summary>
		public void InitComplexNum(Numerics.BigRational realPart, Numerics.BigRational imaginaryPart) {
			this.realPart = new Value(realPart, Restrictions.none);
			this.imaginaryPart = new Value(imaginaryPart, Restrictions.none);
			this.deciValue = realPart;
			this.primaryNumType = NumberType.imaginary;

		}

		/// <summary>Fraction</summary>
		public void InitFraction(BigInteger numerator, BigInteger denominator) {
			this.numerator = new Value(numerator, Restrictions.dontSetToFraction);
			this.denominator = new Value(denominator, Restrictions.dontSetToFraction);
			this.deciValue = ((Numerics.BigRational)numerator / denominator);
			this.primaryNumType = NumberType.fractional;
		}

		/// <summary>Exponent</summary>
		public void InitExp(Numerics.BigRational expBase, Numerics.BigRational expPower, Restrictions restrictionsToPass) {
			this.ExpBase = new Value(expBase, restrictionsToPass);
			this.ExpPower = new Value(expPower, restrictionsToPass);
			this.deciValue = (Numerics.BigRational.Pow(expBase, (BigInteger)expPower));
			primaryNumType = NumberType.exponent;
		}
		
		public Numerics.BigRational deciValue = double.MinValue;
		
		public Value asAFraction { get; set; }
		public Value realPart { get; set; }
		public Value imaginaryPart { get; set; }
		public Value numerator{ get; set; }
		public Value denominator{ get; set; }
		public Value ExpBase { get; set; }
		public Value ExpPower { get; set; }

		public string GetValueToString() {
			if (primaryNumType == NumberType.deci || primaryNumType == NumberType.integer) {
				return deciValue.ToString();
			} else if (primaryNumType == NumberType.fractional) {
				return numerator.GetValueToString() + "/" + denominator.GetValueToString();
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
			if (asAFraction != null) {
				output += "As a fraction: " + asAFraction.GetValueToString();
				output += " = " + asAFraction.deciValue.ToString();
				output += "\n     Numerator: "+ asAFraction.numerator.FullVisualization();
				output += "     Denomenator: " + asAFraction.denominator.FullVisualization();
			}
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
				return new Value(fractionDenominator, fractionNumerator, NumberType.fractional);
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
			return new Value(fractionNumerator, fractionDenominator, NumberType.fractional);
		}
	}
}
