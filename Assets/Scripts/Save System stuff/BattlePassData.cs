using System.Collections.Generic;

[System.Serializable]
public class BattlePassData 
{
    public List<bool> Free, Meneger, CEO;
    public BattlePassData(BatlPassParser parser)
    {
        Free = parser.FreeTake;
        Meneger = parser.MenegerTake;
        CEO = parser.CeoTake;
    }
}
