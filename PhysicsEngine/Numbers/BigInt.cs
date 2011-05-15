using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace PhysicsEngine.Numbers {
	public class BigInt {
		public BigInt(int value) {
			Val = new BigInteger(value);
		}
		public BigInt(BigInteger value) {
			Val = value;
		}

		public BigInteger Val;
		public Factors factors;
	}	
}
