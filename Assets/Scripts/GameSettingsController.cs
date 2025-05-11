using UnityEngine;

public class GameSettingsController : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private Animator SoundsButton, HarpticButton;
    private CanvasGroup group;
    private PlayerWallet wallet;
    private AudioSource Music;
    private bool music, vibrations;

    void Start()
    {
        group = window.GetComponent<CanvasGroup>();
        Music = GetComponent<AudioSource>();
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        SetVibrate();
        SetMusic();
    }

    void Update()
    {
        
    }

    public void OpenClose()
    {
        if(group.interactable) 
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        else 
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }
    public void SetMusic()
    {
        if (music) 
        { 
            music = false;
            Music.Stop();
        }
        else 
        { 
            music = true;
            Music.Play();
        }
        SoundsButton.SetBool("isOn", music);
    }
    public void SetVibrate()
    {
        if (vibrations) { vibrations = false; }
        else { vibrations = true; }
        wallet.SetVibrations(vibrations);
        HarpticButton.SetBool("isOn", vibrations);
    }
}
