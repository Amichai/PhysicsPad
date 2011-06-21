using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Compiler {
	public static class SystemLog {
		private static List<object> log = new List<object>();
		private static List<Complex> returnValues = new List<Complex>();
		
		public static Tokens AddToLog(this Tokens val) {
			log.Add(val);
			return val;
		}

		public static void AddToReturnValues(Complex val) {
			returnValues.Add(val);
		}

		internal static Complex LastNumber() {
			return returnValues.Last();
		}
	}
}
