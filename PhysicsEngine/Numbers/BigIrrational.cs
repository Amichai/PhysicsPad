using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace PhysicsEngine.Numbers {
	public class BigIrrational {
		public BigIrrational(double val) {
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
	}
}
