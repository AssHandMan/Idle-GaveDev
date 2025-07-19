using UnityEngine;
using UnityEngine.UI;

public class LvlRombe : MonoBehaviour
{
    [SerializeField] private GameObject Line, Rombe;
    [SerializeField] private Image BlueLine;
    public Text text;
    public void SetRomb() { Rombe.SetActive(true); }
    public void SetLine() { Line.SetActive(true); }
    public void HideBlueLine() { BlueLine.enabled = false; }
}
