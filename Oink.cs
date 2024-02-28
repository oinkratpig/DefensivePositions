using System;
using System.IO;

namespace DefensivePositions
{
	public static class Oink
	{
		private const bool DEBUG = false;

		private static readonly string _logPath = @"C:\Users\knila\Desktop\Mods\Lobotomy Corp\DefensivePosition_Attempt2\log.txt";
		private static int _logCount;

		/// <summary>
		/// Log message for debugging.
		/// </summary>
		public static void Log(string message)
		{
			#pragma warning disable CS0162 // Unreachable code detected
			if (DEBUG)
                using (StreamWriter writer = File.AppendText(_logPath))
                    writer.WriteLine($"{DateTime.Now} {{{_logCount++}}} {message}");
			#pragma warning restore CS0162 // Unreachable code detected

		} // end Log

	} // end Oink

} // end namespace
