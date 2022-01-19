using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class Assistance
{
    public static void FloatToTime(float value, out int min, out int sec, out int msec)
    {
        min = Mathf.FloorToInt(value / 60);
        sec = Mathf.FloorToInt(value % 60);
        msec = Mathf.FloorToInt(((value % 1) * 1000));
    }

    public static string FloatToTimeString(float value)
    {
        FloatToTime(value, out int min, out int sec, out int msec);

        return $"{min:D2}:{sec:D2}:{msec:D2}";
    }
}