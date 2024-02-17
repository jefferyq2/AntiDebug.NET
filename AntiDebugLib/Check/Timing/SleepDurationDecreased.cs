﻿using System;
using System.Threading;

namespace AntiDebugLib.Check.Timing
{
    /// <summary>
    /// <list type="bullet">
    /// <item>
    /// AntiCrack-DotNet :: https://github.com/AdvDebug/AntiCrack-DotNet/blob/91872f71c5601e4b037b713f31327dfde1662481/AntiCrack-DotNet/AntiDebug.cs#L224
    /// </item>
    /// <item>
    /// Anti-Debug-Collection :: https://github.com/MrakDev/Anti-Debug-Collection/blob/585e33cbe57aa97725b3f98658944b01f1844562/src/Misc/Timer.cs#L12
    /// </item>
    /// </list>
    /// </summary>
    public class SleepDurationDecreased : CheckBase
    {
        public override string Name => "Sleep Ignorance - TickCount delta too short than expected";

        public override CheckReliability Reliability => CheckReliability.Perfect;

        public override bool CheckActive()
        {
            var prev = Environment.TickCount;
            Thread.Sleep(500);
            return Environment.TickCount - prev < 500L;
        }
    }
}
