using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

/// Перед использованием - несколько моментов.
/// 
/// Задумывается, что startColumn всегда равен 1.
/// Данные находятся только в листе "Парсер".

public class ExcelParser : MonoBehaviour
{
    private string filePath;
    private ExcelWorksheet sheet;
    private List<CharacterUpgrade> characterUpgrades;
    private List<CharacterUpgradePrice> characterUpgradesPrices;
    private List<WorkerUpgrade> workerUpgrades;
    private List<WorkerPrice> workersPrices;
    private List<BuyerEffect> buyerEffects;
    private List<ShopInfo> shopsInfo; // Глобальная карта
    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "Game - баланс.xlsx");
        ParseExcelFile(filePath);
    }

    void ParseExcelFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Excel file not found: " + path);
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(new FileInfo(path)))
        {
            sheet = package.Workbook.Worksheets["Парсер"];
            if (sheet == null)
            {
                Debug.LogError("Sheet 'Парсер' not found");
                return;
            }

            characterUpgrades = ParseCharacterUpgrades(4, GetEndRow(4));
        }
    }

    private List<CharacterUpgrade> ParseCharacterUpgrades(int startRow, int endRow)
    {   
        var result = new List<CharacterUpgrade>();

        for (int row = startRow; row <= endRow; row++)
        {
            int level = Convert.ToInt32(sheet.Cells[row, 1].Value);
            float speed = Convert.ToSingle(sheet.Cells[row, 2].Value);
            int capacity = Convert.ToInt32(sheet.Cells[row, 3].Value);
            int profit = Convert.ToInt32(sheet.Cells[row, 4].Value);

            Debug.Log($"{level}, {speed}, {capacity}, {profit}");

            result.Add(new CharacterUpgrade(level, speed, capacity, profit));
        }

        Debug.Log("Parsed " + result.Count + " character upgrades");
        return result;
    }

    private int GetEndRow(int startRow)
    {
        int endRow = startRow;
        while (true)
        {
            if (sheet.Cells[endRow, 1].Value == null)
            {
                return endRow - 1;
            }
            endRow++;
        }
    }

    private int GetEndColumn(int startColumn)
    {
        int endColumn = startColumn;
        while (true)
        {
            if (sheet.Cells[1, endColumn].Value == null)
            {
                return endColumn - 1;
            }
            endColumn++;
        }
        
    }

}

// TODO: разделить логику парсера от классов для данных

public class CharacterUpgrade
{
    public int Level;
    public float Speed;
    public int Capacity;
    public int Profit;

    public CharacterUpgrade(int level, float speed, int capacity, int profit)
    {
        Level = level;
        Speed = speed;
        Capacity = capacity;
        Profit = profit;
    }
}

public class CharacterUpgradePrice
{
    public int Level;
    public int SpeedPrice;
    public int CapacityPrice;
    public int ProfitPrice;

    public CharacterUpgradePrice(int level, int speedPrice, int capacityPrice, int profitPrice)
    {
        Level = level;
        SpeedPrice = speedPrice;
        CapacityPrice = capacityPrice;
        ProfitPrice = profitPrice;
    }
}

public class WorkerUpgrade
{
    public int Level;
    public float Speed;
    public int Capacity;
    public int Profit;

    public WorkerUpgrade(int level, float speed, int capacity, int profit)
    {
        Level = level;
        Speed = speed;
        Capacity = capacity;
        Profit = profit;
    }
}

public class WorkerPrice
{
    public int Level;
    public string Name;
    public int Price;

    public WorkerPrice(int level, string name, int price)
    {
        Level = level;
        Name = name;
        Price = price;
    }
}

public class BuyerEffect
{
    public string Name;
    public float Value;

    public BuyerEffect(string name, float value)
    {
        Name = name;
        Value = value;
    }
}

public class ShopInfo 
{
    public int ShopID;
    public float Income;
    public int Time;
    public int Capacity;

    public ShopInfo(int shopID, float income, int time)
    {
        ShopID = shopID;
        Income = income;
        Time = time;
    }
}