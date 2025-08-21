
[System.Serializable]
public class MenegersData
{
    public int[] lvl;
    public MenegersData(HrRoom room)
    {
        lvl = room.lvl;
    }
}
