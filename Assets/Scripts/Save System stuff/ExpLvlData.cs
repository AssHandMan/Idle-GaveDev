using UnityEngine;

[System.Serializable]
public class ExpLvlData
{
    public int exp, expMax, lvl;
    public bool Meneger, CEO;
    public ExpLvlData(ExpLvlController controller) 
    {
        exp = controller.CountExP;
        expMax = controller.MaxExPLvl;
        lvl = controller.lvl;
        Meneger = controller.isMeneger;
        CEO = controller.isCEO;
    }
}
