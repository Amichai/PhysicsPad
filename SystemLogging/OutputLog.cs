using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemLogging {
	public static class OutputLog {
		public static void AddToLog(object val) {
			returnValues.Add(val);
		}
		public static List<object> returnValues = new List<object>();
	}

	public static class ErrorLog {
		public static void Add(ErrorMessage message) {
			errorMessages.Add(message);
		}
		private static List<ErrorMessage> errorMessages = new List<ErrorMessage>();

	}
}
