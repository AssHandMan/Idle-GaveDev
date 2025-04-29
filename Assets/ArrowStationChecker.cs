using UnityEngine;

public class ArrowStationChecker : MonoBehaviour
{
    [SerializeField] private ArrowPosController arrow;
    [SerializeField] private bool CheckToInscrease;
    [SerializeField] private int lvl;
    [SerializeField] private ArcadeMachine arcadeFirst;
    private StationObjectCollection station;

    public int count;
    void Start()
    {
        station = GetComponent<StationObjectCollection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow.posLvl == lvl)
        {

            if (!CheckToInscrease)
            {
                if (arcadeFirst == null)
                {
                    if (count < station.ObjectsList.Count)
                    {
                        count = station.ObjectsList.Count;
                    }
                    if (count > station.ObjectsList.Count)
                    {
                        arrow.MoveNextToStation();
                        Destroy(GetComponent<ArrowStationChecker>());
                    }
                }
                else
                {
                    if(arcadeFirst.station.ObjectsList.Count > 0)
                    {
                        arrow.MoveNextToStation();
                        Destroy(GetComponent<ArrowStationChecker>());
                    }
                }
            }
            else
            {
                if (station.ObjectsList.Count > 0)
                {
                    arrow.MoveNextToStation();
                    Destroy(GetComponent<ArrowStationChecker>());
                }
            }
        }
    }
}
