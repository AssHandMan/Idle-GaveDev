using UnityEngine;

public class ObjectShowHider : MonoBehaviour
{
    public GameObject[] ShowObjs, HideObjs;

    public void ShowHide()
    {
        for (int i = 0; i < ShowObjs.Length; i++)
        {
            ShowObjs[i].SetActive(true);
        }
        for (int i = 0; i < HideObjs.Length; i++)
        {
            HideObjs[i].SetActive(false);
        }
    }
}
