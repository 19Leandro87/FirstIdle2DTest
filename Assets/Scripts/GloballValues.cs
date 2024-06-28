using System.Collections.Generic;
using System.Globalization;

public class GlobalValues
{
    public const double BASE_POLLUTION = 1000000d;

    public static List<UnitObject> BASE_UNITS = new List<UnitObject> {
        new UnitObject { Enabled = true, Name = "Unit 0", Level = 0, Price = 50d, PriceFactor = 1.05d, PollutionClean = 5f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 0f, Description = "This is Unit 0" },
        new UnitObject { Enabled = false, Name = "Unit 1", Level = 0, Price = 1000d, PriceFactor = 1.05d, PollutionClean = 10f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 2f, Description = "This is Unit 1" },
        new UnitObject { Enabled = false, Name = "Unit 2", Level = 0, Price = 30000d, PriceFactor = 1.05d, PollutionClean = 20f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 4f, Description = "This is Unit 2" }
    };

    public enum UpgradeTypes {
        UnitConnected,
        PollutionRelated,
        Special
    }

    public static List<UpgradeObject> BASE_UNIT_CONN_UPGRADES = new List<UpgradeObject> {
        new UpgradeObject { Enabled = false, Type = UpgradeTypes.UnitConnected, Name = "Unit 0 Upgrade", ConnectedUnitIndex = 0, TimesBought = 0, ShortDescription = "Clean x2", FullDescription = "Unit 0 will clean twice as fast! (it stacks)" },
        new UpgradeObject { Enabled = false, Type = UpgradeTypes.UnitConnected, Name = "Unit 1 Upgrade", ConnectedUnitIndex = 1, TimesBought = 0, ShortDescription = "Clean x2", FullDescription = "Unit 1 will clean twice as fast! (it stacks)" },
        new UpgradeObject { Enabled = false, Type = UpgradeTypes.UnitConnected, Name = "Unit 2 Upgrade", ConnectedUnitIndex = 2, TimesBought = 0, ShortDescription = "Clean x2", FullDescription = "Unit 2 will clean twice as fast! (it stacks)" },

    };

    public static List<UpgradeObject> BASE_POLLUTION_REL_UPGRADES = new List<UpgradeObject> {
        new UpgradeObject { Enabled = false, Type = UpgradeTypes.PollutionRelated, Name = "Tap Upgrade", PollutionUnlockPercentage = 1f, TimesBought = 0, ShortDescription = "Tap x2", FullDescription = "Tapping cleans twice as much! (stacking)" },

    };

    public static List<UpgradeObject> BASE_SPECIAL_UPGRADES = new List<UpgradeObject> {
        new UpgradeObject { Enabled = false, Type = UpgradeTypes.Special, Name = "Combo Unit 0 - 1", TimesBought = 0, ShortDescription = "Super Unit 0", FullDescription = "Unit 0 uses Unit 1 to become more powa!" },

    };
    


    //Format string numbers concerning money, showing 2 decimals (if there are any), use dot to separate the decimal part and comma for the thousands etc.
    //If the price is above 99,999$ use exponential format n*e^x, same for the available money but if it's higher than 999,999,999$
    public static string MoneyStringNumbersFormat(double money) {
        if (money < 999999999) return "$ " + money.ToString(money % 1 == 0 ? "N0" : "N2", CultureInfo.InvariantCulture);
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
