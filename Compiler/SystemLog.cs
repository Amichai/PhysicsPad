using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler {
	public static class SystemLog {
		public static List<object> returnValues = new List<object>();
		public static Tokens AddToLog(this Tokens val) {
			returnValues.Add(val);
			return val;
		}
	}
}
