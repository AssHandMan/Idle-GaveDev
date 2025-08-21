

using System.Collections.Generic;

[System.Serializable]
public class SkinData
{
    public bool[] isBuy, SkinTicks, Locks;
    public int SkinNow;
    public SkinData(SkinsRoom room) 
    {
        isBuy = room.isBuy;
        SkinNow = room.SkinNow;
        
        List<bool> list = new List<bool>();
        List<bool> list2 = new List<bool>();
        for (int i = 0; i < room.SkinNowSpr.Length; i++)
        {
            list.Add(room.SkinNowSpr[i].activeSelf);
            list2.Add(room.Locks[i].activeSelf);
        }
        SkinTicks = list.ToArray();
        Locks = list2.ToArray();
    }

}
