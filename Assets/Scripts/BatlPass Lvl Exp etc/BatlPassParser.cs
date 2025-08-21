using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BatlPassParser : MonoBehaviour
{
    [SerializeField] private string[] Rewards;
    [SerializeField] private string[] FreeRewards;
    [SerializeField] private string[] MenegerRewards;
    [SerializeField] private string[] CEORewards;
    [SerializeField] private string[] NewLvlOpens;

    [SerializeField] private GameObject lvlBlock, FreeBlock, MenegerBlock, CeoBlock, menu, MenegerBtn, CEOBtn;
    [SerializeField] private RectTransform container, content;

    [SerializeField] private List<LvlRombe> Romb;
    [SerializeField] private List<BlockBattlePass> FreeBlocks, MenegerBlocks, CeoBlocks;
    [SerializeField] private ExpLvlController expController;
    private RectTransform bottomElement;
    private float bottomPadding = 220f;
    private bool isMeneger, isCEO;

    [SerializeField] private Text LvlvMenuNewTxt;
    [SerializeField] private GameObject menuNewLvl;
    [SerializeField] private OpenNewLvlCircle[] circles;
    private int CountReward;
    private char TypeReward;
    [SerializeField] private GameObject[] MoneyReward, DiamondsReward;
    [SerializeField] private PlayerWallet wallet;
    [SerializeField] private Text RewardNewLvlCount;
    public GameObject AdsScreen, BtnReward1, BtnReward2, AttentionSymbol, RewardPanel, AvailablePanel;
    public GameSettingsController settings;
    public VideoPlayer adsPlayer;
    public RectTransform CirclesMain;

    [ContextMenu("Parse CSV")]
    private void Parse()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "battlepass.csv");

        if (!File.Exists(path))
        {
            Debug.LogError("Файл не найден: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);
        List<string> rewards = new(), free = new(), manager = new(), ceo = new(), opens = new();

        for (int i = 2; i < lines.Length; i++) // начиная с 3-й строки
        {
            var cols = ParseCsvLine(lines[i]);

            if (cols.Length == 0 || string.IsNullOrWhiteSpace(cols[0]))
                break;

            rewards.Add(GetValue(cols, 1));
            free.Add(GetValue(cols, 2));
            manager.Add(GetValue(cols, 3));
            ceo.Add(GetValue(cols, 4));
            opens.Add(Normalize(GetValue(cols, 5))); // сохраняем как один элемент
        }

        Rewards = rewards.ToArray();
        FreeRewards = free.ToArray();
        MenegerRewards = manager.ToArray();
        CEORewards = ceo.ToArray();
        NewLvlOpens = opens.ToArray();

    }
    private void SetValue()
    {
        Vector2 basePos = lvlBlock.GetComponent<RectTransform>().anchoredPosition;
        Romb.Add(lvlBlock.GetComponent<LvlRombe>());
        for (int i = 1; i < FreeRewards.Length; i++)
        {
            GameObject obj = Instantiate(lvlBlock.gameObject, container);

            RectTransform newRect = obj.GetComponent<RectTransform>();

            newRect.anchoredPosition = basePos + new Vector2(0, -360 * i);
            newRect.localScale = Vector3.one;

            obj.GetComponent<LvlRombe>().text.text = (i + 1).ToString();
            obj.name = (i + 1).ToString();
            Romb.Add(obj.GetComponent<LvlRombe>());
        }
        Romb[Romb.Count-1].HideBlueLine();

        basePos = FreeBlock.GetComponent<RectTransform>().anchoredPosition;
        FreeBlocks.Add(FreeBlock.GetComponent<BlockBattlePass>());
        for (int i = 1; i < FreeRewards.Length; i++)
        {
            GameObject obj = Instantiate(FreeBlock.gameObject, container);

            RectTransform newRect = obj.GetComponent<RectTransform>();

            newRect.anchoredPosition = basePos + new Vector2(0, -360 * i);
            newRect.localScale = Vector3.one;

            obj.name = "FreeBlock" + (i + 1).ToString();
            FreeBlocks.Add(obj.GetComponent<BlockBattlePass>());
            obj.GetComponent<BlockBattlePass>().SetValue(FreeRewards[i]);
        }
        FreeBlocks[0].SetValue(FreeRewards[0]);

        basePos = MenegerBlock.GetComponent<RectTransform>().anchoredPosition;
        MenegerBlocks.Add(MenegerBlock.GetComponent<BlockBattlePass>());
        for (int i = 1; i < MenegerRewards.Length; i++)
        {
            GameObject obj = Instantiate(MenegerBlock.gameObject, container);

            RectTransform newRect = obj.GetComponent<RectTransform>();

            newRect.anchoredPosition = basePos + new Vector2(0, -360 * i);
            newRect.localScale = Vector3.one;

            obj.name = "MenegerBlock" + (i + 1).ToString();
            MenegerBlocks.Add(obj.GetComponent<BlockBattlePass>());
            obj.GetComponent<BlockBattlePass>().SetValue(MenegerRewards[i]);
        }
        MenegerBlocks[0].SetValue(MenegerRewards[0]);

        basePos = CeoBlock.GetComponent<RectTransform>().anchoredPosition;
        CeoBlocks.Add(CeoBlock.GetComponent<BlockBattlePass>());
        for (int i = 1; i < CEORewards.Length; i++)
        {
            GameObject obj = Instantiate(CeoBlock.gameObject, container);

            RectTransform newRect = obj.GetComponent<RectTransform>();

            newRect.anchoredPosition = basePos + new Vector2(0, -360 * i);
            newRect.localScale = Vector3.one;

            obj.name = "CEOBLOCK" + (i + 1).ToString();
            CeoBlocks.Add(obj.GetComponent<BlockBattlePass>());
            obj.GetComponent<BlockBattlePass>().SetValue(CEORewards[i]);
        }
        CeoBlocks[0].SetValue(CEORewards[0]);
    }

    private void SetContentVieweSize()
    {
        bottomElement = CeoBlocks[CeoBlocks.Count - 1].GetComponent<RectTransform>();
        float bottomY = Mathf.Abs(bottomElement.anchoredPosition.y);
        float elementHeight = bottomElement.sizeDelta.y;

        float newHeight = bottomY + elementHeight + bottomPadding;

        Vector2 size = content.sizeDelta;
        size.y = newHeight;
        content.sizeDelta = size;


        for (int i = 0; i < FreeBlocks.Count; i++)
        {
            Romb[i].transform.parent = content;
            FreeBlocks[i].transform.parent = content;
            MenegerBlocks[i].transform.parent = content;
            CeoBlocks[i].transform.parent = content;
        }
    }

    public void GetNewLvl(int lvl)
    {
        if(lvl == 1)
        {
            Romb[0].SetRomb();
        }
        else if(lvl >= Romb.Count)
        {
            Romb[Romb.Count - 1].SetRomb();
        }
        else
        {
            Romb[lvl - 1].SetRomb();
            Romb[lvl - 2].SetLine();
        }
        FreeBlocks[lvl - 1].ShowButton();
        if (isMeneger) { MenegerBlocks[lvl-1].ShowButton(); }
        if (isCEO) { CeoBlocks[lvl - 1].ShowButton(); }
        AttentionSymbol.SetActive(true);
    }
    public void CheckForAttention()
    {
        int a = 0;
        for(int i = 0; i < FreeBlocks.Count; i++)
        {
            if (FreeBlocks[i].GetBtnStatus() || MenegerBlocks[i].GetBtnStatus() || CeoBlocks[i].GetBtnStatus())
            {
                a++;
                break;
            }
        }
        if(a == 0) { AttentionSymbol.SetActive(false); }
    }

    public void ShowHide()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }

    public void ButNewTypeAccount(string type)
    {
        if(type == "m")
        {
            isMeneger = true;
            expController.isMeneger = true;
            MenegerBtn.SetActive(false);
            for (int i = 0; i < Romb.Count;  i++)
            {
                MenegerBlocks[i].HideLock();
                if (i < expController.lvl)
                {
                    MenegerBlocks[i].ShowButton();
                }
            }
        }
        else if (type == "c")
        {
            isCEO = true;
            expController.isCEO = true;
            CEOBtn.SetActive(false);
            for (int i = 0; i < Romb.Count; i++)
            {
                CeoBlocks[i].HideLock();
                if(i < expController.lvl)
                {
                    CeoBlocks[i].ShowButton();
                }
            }
        }
        AttentionSymbol.SetActive(true);
        expController.Save();
    }

    private void Start()
    {
        SetValue();
        SetContentVieweSize();
    }

    private string GetValue(string[] cols, int index)
    {
        return index < cols.Length ? cols[index].Trim() : "";
    }

    private string Normalize(string s)
    {
        return s.Trim();
    }

    // CSV-парсер с поддержкой кавычек (если в ячейке есть запятые)
    private string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        values.Add(current); // последняя колонка
        return values.Select(s => s.Trim().Trim('"')).ToArray();
    }

    public void ShowNewLvlMenu(int lvl)
    {
        LvlvMenuNewTxt.text = lvl.ToString();

        circles[0].gameObject.SetActive(false);
        circles[1].gameObject.SetActive(false);
        circles[2].gameObject.SetActive(false);
        AvailablePanel.SetActive(false);
        string input = NewLvlOpens[lvl-1];
        if (input != "")
        {


            string[] result = input
                .Split(',')
                .Select(s => s.Trim())
                .ToArray();

            AvailablePanel.SetActive(true);
            if (result.Length == 1)
            {
                circles[1].gameObject.SetActive(true);
                circles[1].Open(result[0]);
            }
            else if (result.Length == 2)
            {
                circles[0].gameObject.SetActive(true);
                circles[1].gameObject.SetActive(true);
                circles[0].Open(result[0]);
                circles[1].Open(result[1]);
                Debug.Log(result[0]);
                Debug.Log(result[1]);
                CirclesMain.localPosition = new Vector3(120, CirclesMain.localPosition.y, 0);
            }
            else
            {
                circles[0].gameObject.SetActive(true);
                circles[1].gameObject.SetActive(true);
                circles[2].gameObject.SetActive(true);
                circles[0].Open(result[0]);
                circles[1].Open(result[1]);
                circles[2].Open(result[2]);
            }
        }
        input = Rewards[lvl-1];
        Debug.Log(Rewards[lvl-1]);
        BtnReward1.SetActive(false); BtnReward2.SetActive(false);
        RewardPanel.SetActive(false);
        if (input != "" && input != " ")
        {
            BtnReward1.SetActive(true); BtnReward2.SetActive(true);
            string[] parts = input.Split(' ');
            int number = int.Parse(parts[0]);
            char letter = parts[1][0];
            CountReward = number;
            if(CountReward > 0) { RewardPanel.SetActive(true); }
            TypeReward = letter;
            RewardNewLvlCount.text = "+" + CountReward.ToString();
            if (letter == 'b')
            {
                if (number < 100) { MoneyReward[0].SetActive(true); }
                else { MoneyReward[1].SetActive(true); }
            }
            else if (letter == 'k')
            {
                if (number <= 2) { DiamondsReward[0].SetActive(true); }
                else if (number > 2 && number <= 5) { DiamondsReward[1].SetActive(true); }
            }
        }
        menuNewLvl.SetActive(true);
    }

    public void GetRewardNewLvl(int t)
    {
        if (t == 0)
        {
            if (TypeReward == 'b') { wallet.ChangeCount(CountReward, "Money"); }
            else { wallet.ChangeCount(CountReward, "Diamonds"); }
            TypeReward = ' ';
            CountReward = 0;
        }
        else if(t == 1 && wallet.Diamonds >= 3)
        {
            wallet.ChangeCount(-3, "Diamonds");
            if (TypeReward == 'b') { wallet.ChangeCount(CountReward*2, "Money"); }
            else { wallet.ChangeCount(CountReward*2, "Diamonds"); }
            TypeReward = ' ';
            CountReward = 0;
            HideNewLvlMenu();
        }
        else
        {
            settings.SetMusic();
            AdsScreen.SetActive(true);
            adsPlayer.Play();
            StartCoroutine(TimeToEndAds());
        }
    }
    private IEnumerator TimeToEndAds()
    {
        yield return new WaitForSeconds(21);
        AdsScreen.SetActive(false);
        settings.SetMusic();
        if (TypeReward == 'b') { wallet.ChangeCount(CountReward * 2, "Money"); }
        else { wallet.ChangeCount(CountReward*2, "Diamonds"); }
        TypeReward = ' ';
        CountReward = 0;
        HideNewLvlMenu();
    }

    public void HideNewLvlMenu() 
    {
        if(CountReward > 0) { GetRewardNewLvl(0); } 
        circles[0].Cleare();
        circles[1].Cleare();
        circles[2].Cleare();
        for(int i = 0; i < MoneyReward.Length; i++)
        {
            MoneyReward[i].SetActive(false);
            DiamondsReward[i].SetActive(false);
        }
        menuNewLvl.SetActive(false);
        CirclesMain.localPosition = new Vector3(0, CirclesMain.localPosition.y, 0);
    }
}
