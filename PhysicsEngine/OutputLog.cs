using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;

namespace PhysicsEngine {
	public static class OutputLog {
		public static void Add(Value val) {
			returnValues.Add(val);
		}
		public static List<Value> returnValues = new List<Value>();
	}

	public static class ErrorLog {
		public static void Add(ErrorMessage message) {
			errorMessages.Add(message);
		}
		private static List<ErrorMessage> errorMessages = new List<ErrorMessage>();

	}
}
