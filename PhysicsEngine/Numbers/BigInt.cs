using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsEngine.Numbers {
	class BigInt {
		//Stores a number to variable and unlimited precision
		public double doubleValue;
		public BigInt(double val){
			doubleValue = val;
		}

		List<bool> leftOfDecimal = new List<bool>();
		List<bool> rightOfDecimal = new List<bool>();
		//Not yet being used...should be used
		//TODO: Implement this.
	}
}
