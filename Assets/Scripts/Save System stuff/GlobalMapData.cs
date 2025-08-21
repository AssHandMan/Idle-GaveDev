using UnityEngine;

[System.Serializable]
public class GlobalMapData
{
    public bool[] OpenShops;
    public GlobalMapData(GlobalMapMainController controller)
    {
        OpenShops = controller.OpenShops;
    }
}
