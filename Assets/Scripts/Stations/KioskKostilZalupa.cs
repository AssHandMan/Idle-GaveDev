using UnityEngine;

public class KioskKostilZalupa : MonoBehaviour
{
    [SerializeField] private ParticleSystem circleArea;

    public void OnEnable()
    {
        circleArea.Play();
    }
}
