using System;
using System.Runtime.InteropServices;

namespace LuaEdit.RemoteDebugger
{
	public static class LuaEditRemoteDebugger
	{
        /// <summary>
        /// Starts LuaEdit's remote debugger.
        /// </summary>
        /// <param name="port">The port number at which the remote debug server will be listening.</param>
        /// <param name="L">A pointer to the LuaState to use. (if null, a new one will be created)</param>
        [DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        public static extern void StartLuaEditRemoteDebugger(int port, IntPtr L);

        /// <summary>
        /// Stops LuaEdit's remote debugger. Must be called in order to properly uninitialize the debugger if StartLuaEditRemoteDebugger was called.
        /// </summary>
        [DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        public static extern void StopLuaEditRemoteDebugger();

        /// <summary>
        /// Gets the current version of Lua used by LuaEdit's remote debugger.
        /// </summary>
        /// <returns>The Lua version as a string value.</returns>
		[DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetCurrentLuaVersion();

        /// <summary>
        /// Validates the syntax of the specified Lua chunk.
        /// </summary>
        /// <param name="script">The Lua chunk to validate.</param>
        /// <param name="scriptName">The name used by the validator if there is an error in the specified chunk.</param>
        /// <param name="errMsg">The validator will fill the content of that StringBuilder with the error message if the is one or string.Empty if there is none.</param>
        /// <param name="errMsgLen">The length of the error message contained in errMsg.</param>
        /// <returns>Returns true if there was an error validating the specified Lua chunk or false if there was none.</returns>
        [DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        private static extern bool CheckLuaScriptSyntax(StringBuilder script, StringBuilder scriptName, StringBuilder errMsg, int errMsgLen);
	}
}
