﻿using System;
using System.Diagnostics;

using static AntiDebugLib.Native.NativeDefs;
using static AntiDebugLib.Native.NtDll;

namespace AntiDebugLib.Check.DebugFlags
{
    /// <summary>
    /// <list type="bullet">
    /// <item>
    /// AntiCrack-DotNet :: https://github.com/AdvDebug/AntiCrack-DotNet/blob/91872f71c5601e4b037b713f31327dfde1662481/AntiCrack-DotNet/AntiDebug.cs#L138
    /// </item>
    /// <item>
    /// Anti-Debug-Collection :: https://github.com/MrakDev/Anti-Debug-Collection/blob/585e33cbe57aa97725b3f98658944b01f1844562/src/Flags/ProcessDebugObjectHandleFlag.cs#L13
    /// </item>
    /// <item>
    /// al-khaser :: https://github.com/LordNoteworthy/al-khaser/blob/master/al-khaser/AntiDebug/NtQueryInformationProcess_ProcessDebugObject.cpp
    /// </item>
    /// <item>
    /// The "Ultimate" Anti-Debugging Reference (by Peter Ferrie) :: 7.D.viii.b. NtQueryInformationProcess ProcessDebugObjectHandle
    /// </item>
    /// </list>
    /// </summary>
    public class ProcessDebugObject : CheckBase
    {
        public override string Name => "NtQueryInformationProcess: ProcessDebugObject";

        public override CheckReliability Reliability => CheckReliability.Perfect;

        public override bool CheckActive()
        {
            const uint ProcessDebugObjectHandle = 0x1E; // https://ntdoc.m417z.com/processinfoclass
            var size = (uint)(sizeof(uint) * (Environment.Is64BitProcess ? 2 : 1));
            var status = NtQueryInformationProcess_IntPtr(Process.GetCurrentProcess().SafeHandle, ProcessDebugObjectHandle, out var dbgObject, size, 0);
            if (!NT_SUCCESS(status) && status != NTSTATUS.STATUS_PORT_NOT_SET)
            {
                Logger.Warning("Unable to query ProcessDebugFlags process information. NtQueryInformationProcess returned NTSTATUS {status}.", status);
                return false;
            }

            Logger.Debug("ProcessDebugFlags is {value:X}.", dbgObject.ToInt64());
            return dbgObject != IntPtr.Zero;
        }
    }
}
