﻿using System;
using System.IO;
using System.Text;

using static AntiDebugLib.Native.Kernel32;

namespace AntiDebugLib.Check.Exploits
{
    /// <summary>
    /// <list type="bullet">
    /// <item>
    /// Checkpoint AntiDebug Research :: https://anti-debug.checkpoint.com/techniques/object-handles.html#createfile
    /// </item>
    /// <item>
    /// ShowStopper :: https://github.com/CheckPointSW/showstopper/blob/4e6b8dbef35724d7eb987f61cf72dff7a6abfe49/src/not_suspicious/Technique_HandlesValidation.cpp#L17
    /// </item>
    /// </list>
    /// </summary>
    public class ModuleFileOpen : CheckBase
    {
        public override string Name => "CreateFile (open myself)";

        public override CheckReliability Reliability => CheckReliability.Great;

        public override bool CheckActive()
        {
            var builder = new StringBuilder(32768);
            if (GetModuleFileNameW(IntPtr.Zero, builder, 32768) <= 0)
                return false; // The path of myself is not available

            try
            {
                var stream = File.Open(builder.ToString(), FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                Logger.Information(ex, "CreateFile() failed. (possible being debugged)");
                return true; // CreateFile will return INVALID_IntPtr_VALUE
            }
        }
    }
}
