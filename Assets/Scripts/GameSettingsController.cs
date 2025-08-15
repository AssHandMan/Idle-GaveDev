using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour
{
    [SerializeField] private GameObject window, ButtonLanguage;
    [SerializeField] private Animator SoundsButton, HarpticButton;
    [SerializeField] private Text NowLanguageTxt, LanguageTxt;
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
        
        if (!PlayerPrefs.HasKey("Vibration")) { SetVibrate(); }
        else
        {
            if(PlayerPrefs.GetInt("Vibration") == 0) { vibrations = false; }
            else { vibrations = true; }
            wallet.SetVibrations(vibrations);
            HarpticButton.SetBool("isOn", vibrations);
        }

        if (!PlayerPrefs.HasKey("Music")) { SetMusic(); }
        else
        {
            if (PlayerPrefs.GetInt("Music") == 0) { music = true; }
            else { music = false; }
            SetMusic();
            
        }

        StartCoroutine(InitLanguage());
    }

    private IEnumerator InitLanguage()
    {
        yield return LocalizationSettings.InitializationOperation;

        string savedCode = PlayerPrefs.GetString("LanguageCode", "");
        if (!string.IsNullOrEmpty(savedCode) &&
            savedCode != LocalizationSettings.SelectedLocale.Identifier.Code)
        {
            foreach (var loc in LocalizationSettings.AvailableLocales.Locales)
            {
                if (loc.Identifier.Code == savedCode)
                {
                    LocalizationSettings.SelectedLocale = loc;

                    string s = NowLanguageTxt.text;
                    NowLanguageTxt.text = LanguageTxt.text;
                    LanguageTxt.text = s;

                    break;
                }
            }
        }
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
        SaveSettings();
    }
    public void SetVibrate()
    {
        if (vibrations) { vibrations = false; }
        else { vibrations = true; }
        wallet.SetVibrations(vibrations);
        HarpticButton.SetBool("isOn", vibrations);
        SaveSettings();
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

        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            StartCoroutine(SetLocal(1));
        else
            StartCoroutine(SetLocal(0));
    }

    private IEnumerator SetLocal(int id)
    {
        ActiveChange = true;
        yield return LocalizationSettings.InitializationOperation;

        var locale = LocalizationSettings.AvailableLocales.Locales[id];
        LocalizationSettings.SelectedLocale = locale;

        PlayerPrefs.SetString("LanguageCode", locale.Identifier.Code);
        PlayerPrefs.Save();

        ActiveChange = false;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("Vibration", vibrations ? 1 : 0);
        PlayerPrefs.SetInt("Music", music ? 1 : 0);
        PlayerPrefs.Save();
    }
}
