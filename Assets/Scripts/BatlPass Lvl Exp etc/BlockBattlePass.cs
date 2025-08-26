using UnityEngine;
using TMPro;

public class BlockBattlePass : MonoBehaviour
{
    [SerializeField] private PlayerWallet wallet;
    [SerializeField] private GameObject[] Money, Diamonds, Skins;
    [SerializeField] private GameObject LockSpr, GetBtn;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private BatlPassParser parser;
    private int count;
    private char type;
    [Header("Audio")]
    public AudioSource _audio;
    public AudioClip _clipBtn;
    public void SetValue(string input)
    {
        string[] parts = input.Split(' ');
        int number = int.Parse(parts[0]);
        char letter = parts[1][0];
        count = number;
        type = letter;
        textCount.text = number.ToString();

        if (letter == 'b')
        {
            if(number < 100) { Money[0].SetActive(true); }
            else { Money[1].SetActive(true);}
        }
        else if(letter == 'k')
        {
            if (number <= 2) { Diamonds[0].SetActive(true); }
            else if(number > 2 && number <= 5) { Diamonds[1].SetActive(true); }
            else { Diamonds[2].SetActive(true); }
        }
        else
        {
            if (number < 100) { Skins[0].SetActive(true); }
            else { Skins[1].SetActive(true); }
        }
    }
    public void Get()
    {
        if(type == 'b') { wallet.ChangeCount(count, "Money"); }
        else if (type == 'k') { wallet.ChangeCount(count, "Diamonds"); }
        else { wallet.ChangeCount(count, "Skins"); }
        GetBtn.SetActive(false);
        parser.CheckForAttention();
        _audio.PlayOneShot(_clipBtn, 0.4f);

    }
    public bool GetBtnStatus()
    {
        return GetBtn.activeSelf;
    }
    public void HideLock() { LockSpr.SetActive(false); }
    public void ShowButton() { GetBtn.SetActive(true); }
}
