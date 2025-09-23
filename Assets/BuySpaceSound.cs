using UnityEngine;

public class BuySpaceSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clipBuyEnd, clipBuyProcesse;
    [SerializeField] private float volume;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayBuyEnd()
    {
        source.pitch = 1;
        source.PlayOneShot(clipBuyEnd, volume);
    }

    public void PlayBuyProcesse()
    {
        source.pitch += 0.05f;
        source.PlayOneShot(clipBuyProcesse, volume);
    }
    public void ResetPitch()
    {
        source.pitch = 1;
    }
}
