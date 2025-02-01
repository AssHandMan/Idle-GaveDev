using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

public class ExcelParser : MonoBehaviour
{
    private string filePath;
    private ExcelWorksheet sheet;
    
    private readonly Dictionary<Type, object> parsedData = new();

    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "Game - баланс.xlsx");
        ParseExcelFile(filePath);
    }

    void ParseExcelFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"Excel file not found: {path}");
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(path));
        sheet = package.Workbook.Worksheets["Парсер"] ?? throw new Exception("Sheet 'Парсер' not found");

        parsedData[typeof(CharacterUpgrade)] = ParseData<CharacterUpgrade>(4);
        parsedData[typeof(CharacterUpgradePrice)] = ParseData<CharacterUpgradePrice>(14);
        parsedData[typeof(WorkerUpgrade)] = ParseData<WorkerUpgrade>(24);
        parsedData[typeof(WorkerPrice)] = ParseWorkerPrices(32, 2);
        parsedData[typeof(BuyerEffect)] = ParseData<BuyerEffect>(45);
        parsedData[typeof(ShopInfo)] = ParseData<ShopInfo>(53);
    }

    private List<T> ParseData<T>(int startRow) where T : IDataParsable, new()
    {
        var result = new List<T>();
        int endRow = GetEndRow(startRow);
        
        for (int row = startRow; row <= endRow; row++)
        {
            var obj = new T();
            obj.ParseFromRow(sheet, row);
            result.Add(obj);
        }
        
        Debug.Log($"Parsed {result.Count} {typeof(T).Name} entries");
        return result;
    }

    private List<WorkerPrice> ParseWorkerPrices(int startRow, int startColumn)
    {
        var result = new List<WorkerPrice>();
        int endRow = GetEndRow(startRow);
        int endColumn = GetEndColumn(startRow, startColumn);

        // Debug.Log($"Parsing Worker Prices: startRow={startRow}, endRow={endRow}, startColumn={startColumn}, endColumn={endColumn}");

        for (int column = startColumn; column <= endColumn; column++)
        {
            string name = sheet.Cells[startRow - 1, column].Value?.ToString()?.Trim() ?? string.Empty;
            // Debug.Log($"Processing column {column}, worker name: {name}");

            for (int row = startRow, level = 0; row <= endRow; row++, level++)
            {
                object cellValue = sheet.Cells[row, column].Value;
                int price = ConvertCell(cellValue);

                // Debug.Log($"Row {row}, Column {column}, Level {level}, Price {price}");

                // Debug.Log($"Name: {name}, Price: {price}, Level: {level}");
                result.Add(new WorkerPrice(level, name, price));
            }
        }

        Debug.Log($"Parsed {result.Count} WorkerPrice entities");
        return result;
    }


    private int GetEndRow(int startRow)
    {
        while (sheet.Cells[startRow, 1].Value != null)
            startRow++;
        return startRow - 1;
    }

    private int GetEndColumn(int row, int startColumn)
    {
        while (sheet.Cells[row, startColumn].Value != null)
            startColumn++;
        return startColumn - 1;
    }

    private int ConvertCell(object value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || value.ToString() == "+")
            return 0;

        if (int.TryParse(value.ToString(), out int result))
            return result;

        Debug.LogWarning($"Unexpected value format: {value}");
        return 0;
    }

}

public interface IDataParsable
{
    void ParseFromRow(ExcelWorksheet sheet, int row);
}

public class CharacterUpgrade : IDataParsable
{
    public int Level;
    public float Speed;
    public int Capacity;
    public int Profit;
    
    public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        Level = Convert.ToInt32(sheet.Cells[row, 1].Value);
        Speed = Convert.ToSingle(sheet.Cells[row, 2].Value);
        Capacity = Convert.ToInt32(sheet.Cells[row, 3].Value);
        Profit = Convert.ToInt32(sheet.Cells[row, 4].Value);
    }
}

public class CharacterUpgradePrice : IDataParsable
{
    public int Level;
    public int SpeedPrice;
    public int CapacityPrice;
    public int ProfitPrice;

    public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        Level = Convert.ToInt32(sheet.Cells[row, 1].Value);
        SpeedPrice = ConvertCell(sheet.Cells[row, 2].Value);
        CapacityPrice = ConvertCell(sheet.Cells[row, 3].Value);
        ProfitPrice = ConvertCell(sheet.Cells[row, 4].Value);
    }

    private int ConvertCell(object value) => value?.ToString() == "+" ? 0 : Convert.ToInt32(value);
}

public class WorkerUpgrade : IDataParsable
{
    public int Level;
    public float Speed;
    public int Capacity;
    public int Profit;

    public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        Level = Convert.ToInt32(sheet.Cells[row, 1].Value);
        Speed = Convert.ToSingle(sheet.Cells[row, 2].Value);
        Capacity = Convert.ToInt32(sheet.Cells[row, 3].Value);
        Profit = Convert.ToInt32(sheet.Cells[row, 4].Value);
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

public class BuyerEffect : IDataParsable
{
    public string Name;
    public float Value;
    public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        Name = sheet.Cells[row, 1].Value.ToString();
        Value = Convert.ToSingle(sheet.Cells[row, 2].Value);
    }
}

public class ShopInfo : IDataParsable
{
    public int ShopID;
    public float Income;
    public int Time;
    public int Capacity;

    public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        ShopID = Convert.ToInt32(sheet.Cells[row, 1].Value);
        Income = Convert.ToSingle(sheet.Cells[row, 2].Value);
        Time = Convert.ToInt32(sheet.Cells[row, 3].Value);
        Capacity = Convert.ToInt32(sheet.Cells[row, 4].Value);
    }
    
}
