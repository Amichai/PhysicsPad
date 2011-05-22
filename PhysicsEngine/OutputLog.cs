using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using System.Numerics;

namespace PhysicsEngine {
	public static class OutputLog {
		public static void Add(Complex val) {
			returnValues.Add(val);
		}
		public static List<Complex> returnValues = new List<Complex>();
	}

	public static class ErrorLog {
		public static void Add(ErrorMessage message) {
			errorMessages.Add(message);
		}
		private static List<ErrorMessage> errorMessages = new List<ErrorMessage>();

	}
}
