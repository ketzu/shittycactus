using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFormatter : MonoBehaviour
{
    public static string Format(int ms)
    {
        var mms = ms % 1000;
        var sec = (ms - mms) / 1000 % 60;
        var min = (ms - 1000 * sec - mms) / 60000;

        return min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0') + "." + mms.ToString().PadLeft(3, '0');
    }
}
