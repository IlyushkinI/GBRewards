using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsKeys
{
    public static readonly string LastDayTimeKey = "LastDailyRewardTime";
    public static readonly string LastWeekTimeKey = "LastWeeklyRewardTime";
    public static readonly string ActiveDailySlotKey = "ActiveDailySlot";
    public static readonly string ActiveWeeklySlotKey = "ActiveWeeklySlot";

    public static readonly string DayCountTimerKey = "Day";
    public static readonly string WeekCountTimerKey = "Week";

    public static readonly string WoodKey = "Wood";
    public static readonly string DiamondKey = "Diamond";

    public static readonly List<string> IntKeys;
    public static readonly List<string> StringKeys;
    static PrefsKeys()
    {
        IntKeys = new List<string>()
        {           
            ActiveDailySlotKey,
            ActiveWeeklySlotKey,
            WoodKey,
            DiamondKey
        };

        StringKeys = new List<string>()
        {
            LastDayTimeKey,
            LastWeekTimeKey,            
        };
    }       
}   
