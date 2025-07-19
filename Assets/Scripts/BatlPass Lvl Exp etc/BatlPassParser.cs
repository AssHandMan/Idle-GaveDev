using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatlPassParser : MonoBehaviour
{
    [SerializeField] private string[] Rewards;
    [SerializeField] private string[] FreeRewards;
    [SerializeField] private string[] MenegerRewards;
    [SerializeField] private string[] CEORewards;
    [SerializeField] private string[] NewLvlOpens;

    [SerializeField] private GameObject lvlBlock, FreeBlock, MenegerBlock, CeoBlock;
    [SerializeField] private RectTransform container, content;

    [SerializeField] private List<LvlRombe> Romb;
    [SerializeField] private List<BlockBattlePass> FreeBlocks, MenegerBlocks, CeoBlocks;
    private RectTransform bottomElement;
    private float bottomPadding = 220f;

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

    void SetContentVieweSize()
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
}
