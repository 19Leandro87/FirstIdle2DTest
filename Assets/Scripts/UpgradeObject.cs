/*
UPGRADES EXPLAINED
There are three groups (types) of upgrades: unit-related, progress-related or special.
The former are connected to a unit and its level, the second to the % of pollution cleaned and the latter have special unlock conditions.
Their behavior is coded in the UnitsManager class.
 */

public class UpgradeObject
{
    public bool Enabled;
    public GlobalValues.UpgradeTypes Type;
    public string Name;
    public int ConnectedUnitIndex;
    public float PollutionUnlockPercentage;
    public long clickMultiplier;
    public int TimesBought;
    public double Price;
    public string ShortDescription;
    public string FullDescription;
    public int LineIndex;
}
