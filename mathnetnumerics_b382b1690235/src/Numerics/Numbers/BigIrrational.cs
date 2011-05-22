using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace MathNet.Numerics {
	public class BigIrrational {
		enum Sign{ positive, negative}
		Sign sign;
		public BigIrrational(double val) {
			if (val >= 0)
				sign = Sign.positive;
			else 
				sign = Sign.negative;
			val = Math.Abs(val);
			string numString = val.ToString();
			Debug.Print(numString);
			int numberOfDecimalPlaces = numString.Count();
			int counter = 0;
			while (Math.Floor(val) != val) {
				val *= 10;
				counter++;
			}
			NumericalContent = new BigInteger(val);
			DecimalLocation = counter;
			Debug.Print(NumericalContent.ToString());
			Debug.Print(DecimalLocation.ToString());
		}
		public int DecimalLocation;
		public BigInteger NumericalContent;
		public void FlipSign() {
			if(NumericalContent != 0){
				if (sign == Sign.negative)
					sign = Sign.positive;
				else
					sign = Sign.negative;
			}
		}

		internal double GetVal() {
			return (double)NumericalContent / (double)DecimalLocation;
		}
	}
}
