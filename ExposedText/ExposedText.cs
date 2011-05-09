using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PhysicsEngine.Expression;
using ICSharpCode.AvalonEdit;

namespace ExposedText {
	public static class ExposedText{
		public static string wholeText = string.Empty;
		public static List<string> charStack = new List<string>();
		public static string currentParagraph = string.Empty;
		public static List<string> paragraphs = new List<string>();

		public static int cursorParagraphIndex = int.MinValue;
		public static string currentSelection = string.Empty;
		public static string cursorLine = string.Empty;

		public static string outputLine = string.Empty;

		
		//Build a UI for extensible:
		//parsing syntax
		//Parse tree evaluation and manipulations
		//Markup language
		//Markup rendering
		//Autocomplete
		//Intellisense
		//Tool tips
		//Hotkeys
		//Syntax highlighting
	}
}
