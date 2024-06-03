using System.Collections.Generic;

public class GlobalValues
{
    public static readonly float BASE_POLLUTION = 100000.0f;

    public static readonly List<UnitObject> BASE_UNITS = new List<UnitObject> { 
        new UnitObject { Enabled = true, Name = "Unit 0", Level = 0, Price = 50f, PriceFactor = 1.08f, PollutionClean = 5f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 0f },
        new UnitObject { Enabled = false, Name = "Unit 1", Level = 0, Price = 250f, PriceFactor = 1.08f, PollutionClean = 10f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 2f },
        new UnitObject { Enabled = false, Name = "Unit 2", Level = 0, Price = 500f, PriceFactor = 1.08f, PollutionClean = 20f, PollutionCleanFactor = 1f, PollutionUnlockPercentage = 4f }
    };
}
