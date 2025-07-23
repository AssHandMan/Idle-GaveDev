using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour
{
    [SerializeField] private GameObject window, ButtonLanguage;
    [SerializeField] private Animator SoundsButton, HarpticButton;
    [SerializeField] private Text NowLanguageTxt, LanguageTxt;
    [SerializeField] private SkinsRoom skinn;
    private CanvasGroup group;
    private PlayerWallet wallet;
    private AudioSource Music;
    private bool music, vibrations;
    private Vector3 PlayerStartPos;
    private string youtubeChannelURL = "https://www.youtube.com/channel/UCe3gjJcCcC1Gc60ucWX_43g";
    private string RedditChannelURL = "https://www.reddit.com/r/technology/";
    private string TikTokChannelURL = "https://www.tiktok.com/@winxpower?_t=ZS-8xHXIZrfMfa";
    private bool ActiveChange;


    void Start()
    {
        group = window.GetComponent<CanvasGroup>();
        Music = GetComponent<AudioSource>();
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        PlayerStartPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        SetVibrate();
        SetMusic();
        skinn.ChangeLanguage();
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

    public void EmergencyEscape()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = PlayerStartPos;
    }
    public void SocialMediaOpen(int number)
    {
        if(number == 0)
        {
            Application.OpenURL(RedditChannelURL);
        }
        else if(number == 1)
        {
            Application.OpenURL(youtubeChannelURL);
        }
        else
        {
            Application.OpenURL(TikTokChannelURL);
        }
    }
    public void ShowLanguages()
    {
        if(ButtonLanguage.activeSelf) { ButtonLanguage.SetActive(false); }
        else { ButtonLanguage.SetActive(true); }
    }
    public void ChangeLanguage()
    {
        if (ActiveChange)
            return;
        string s = NowLanguageTxt.text;
        NowLanguageTxt.text = LanguageTxt.text;
        LanguageTxt.text = s;
        ShowLanguages();
        if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) { StartCoroutine(SetLocal(1)); }
        else { StartCoroutine(SetLocal(0)); }
        skinn.ChangeLanguage();
    }

    private IEnumerator SetLocal(int id)
    {
        ActiveChange = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
        ActiveChange = false;
    }
}
