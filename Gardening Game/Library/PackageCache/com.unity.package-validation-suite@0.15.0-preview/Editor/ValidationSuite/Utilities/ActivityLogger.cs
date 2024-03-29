﻿using UnityEngine;

/// <summary>
/// Logger used to keep track of Validation Suite activity, for debugging
/// </summary>
internal class ActivityLogger
{
    public static void Log(string message, params object[] args)
    {
        var finalMessage = "[Package Validation Suite] " + string.Format(message, args);

#if UNITY_2019_1_OR_NEWER
        Debug.LogFormat(finalMessage, LogOption.NoStacktrace);
#else
        Debug.Log(finalMessage);
#endif
    }
}
