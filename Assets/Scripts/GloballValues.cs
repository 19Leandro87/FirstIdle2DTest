using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;

public class GlobalValues
{
    public const double BASE_POLLUTION = 1000000d;

    public static List<UnitObject> BASE_UNITS = new List<UnitObject> { 
        new UnitObject { Enabled = true, Name = "Unit 0", Level = 0, Price = 50d, PriceFactor = 1.05d, PollutionClean = 5f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 0f },
        new UnitObject { Enabled = false, Name = "Unit 1", Level = 0, Price = 1000d, PriceFactor = 1.05d, PollutionClean = 10f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 2f },
        new UnitObject { Enabled = false, Name = "Unit 2", Level = 0, Price = 30000d, PriceFactor = 1.05d, PollutionClean = 20f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 4f }
    };

    //Format string numbers concerning money, showing 2 decimals (if there are any), use dot to separate the decimal part and comma for the thousands etc.
    //If the price is above 99,999$ use exponential format n*e^x, same for the available money but if it's higher than 999,999,999,999$
    public static string MoneyStringNumbersFormat(double money) {
        if (money < 999999999999) return "$ " + money.ToString(money % 1 == 0 ? "N0" : "N2", CultureInfo.InvariantCulture);
        else return "$ " + money.ToString("0.00E0");
    }    
    
    public static string PriceStringNumbersFormat(double price) {
        if (price < 99999) return "$ " + price.ToString(price % 1 == 0 ? "N0" : "N2", CultureInfo.InvariantCulture);
        else return "$ " + price.ToString("0.00E0");
    }

    public static long timeOnStart = 0;
    public static long timeSinceLast = 0;
    public static long basicOfflineProfitTime = 1800; //Idle profits will run for the first basicOfflineProfitTime * offlineProfitTimeMultiplier seconds. 1800 = 30'
    public static long offlineProfitTimeMultiplier = 1;
}
