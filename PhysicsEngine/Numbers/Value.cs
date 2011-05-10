using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MathNet.Numerics;

namespace PhysicsEngine.Numbers {
	public enum Restrictions { dontFactorMe, dontSetToFraction, dontFactorDontSetFraction, none };
	public enum NumberType { integer, deci, fractional, imaginary, exponent };
	public class Value {
		#region constructors
		public Value(double doubleVal, Restrictions restrictions) {
				InitDouble(doubleVal, restrictions);
		}
		public Value(double doubleVal, Factors factors, Restrictions restrictions) {
			InitDouble(doubleVal, restrictions);
		}
		public Value(double realPart, double imaginaryPart, NumberType type) {
			switch (type) {
				case NumberType.imaginary:
					InitComplexNum(realPart, imaginaryPart);
					break;
			}
		}
		public Value(double baseVal, double exponent, NumberType type, Restrictions restrictions) {
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
		public void InitDouble(double val, Restrictions restrictions) {
			this.deciValue = val;
			if (val == Math.Floor(val)) {
				//this is done to avoid the accumulation of miniscule errors:
				deciValue = Math.Floor(val);
				primaryNumType = NumberType.integer;
				if (restrictions != Restrictions.dontFactorMe && restrictions != Restrictions.dontFactorDontSetFraction) {
					factors = new Factors((int)deciValue);
				}
			}else if (restrictions != Restrictions.dontSetToFraction && restrictions != Restrictions.dontFactorDontSetFraction) {
				asAFraction = decimalToFraction(deciValue);
			}			
			this.primaryNumType = NumberType.deci;
		}
		public Factors factors;

		/// <summary>Complex Numbers</summary>
		public void InitComplexNum(double realPart, double imaginaryPart) {
			this.realPart = new Value(realPart, Restrictions.none);
			this.imaginaryPart = new Value(imaginaryPart, Restrictions.none);
			this.deciValue = realPart;
			this.primaryNumType = NumberType.imaginary;

		}

		/// <summary>Fraction</summary>
		public void InitFraction(int numerator, int denominator) {
			this.numerator = new Value(numerator, Restrictions.dontSetToFraction);
			this.denominator = new Value(denominator, Restrictions.dontSetToFraction);
			this.deciValue = ((double)numerator / denominator);
			this.primaryNumType = NumberType.fractional;
		}

		/// <summary>Exponent</summary>
		public void InitExp(double expBase, double expPower, Restrictions restrictionsToPass) {
			this.ExpBase = new Value(expBase, restrictionsToPass);
			this.ExpPower = new Value(expPower, restrictionsToPass);
			this.deciValue = (Math.Pow(expBase, expPower));
			primaryNumType = NumberType.exponent;
		}
		
		public double deciValue = double.MinValue;
		
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
		private Value decimalToFraction(double Decimal) {
			int fractionNumerator = int.MaxValue;
			int fractionDenominator = 1;
			double accuracyFactor = .0000001;
			int decimalSign;
			double Z;
			int previousDenominator;
			int scratchValue;

			if (Decimal < 0) {
				decimalSign = -1;
			} else decimalSign = 1;
			Decimal = Math.Abs(Decimal);
			if (Decimal == Math.Floor(Decimal)) {
				fractionNumerator = (int)Decimal * decimalSign;
				fractionDenominator = 1;
				return new Value(fractionDenominator, fractionNumerator, NumberType.fractional);
			}
			Z = Decimal;
			previousDenominator = 0;
			while (!(Z == Math.Floor(Z)) && (Math.Abs((Decimal - ((double)fractionNumerator / fractionDenominator))) > accuracyFactor )) {
				Z = 1/(Z - (int)Z);
				scratchValue = fractionDenominator;
				fractionDenominator = fractionDenominator*(int)Z + previousDenominator;
				previousDenominator = scratchValue;
				fractionNumerator= (int)Math.Floor(Decimal*fractionDenominator + .5);
			} 
			fractionNumerator  = decimalSign * fractionNumerator;
			return new Value(fractionNumerator, fractionDenominator, NumberType.fractional);
		}
	}
}
