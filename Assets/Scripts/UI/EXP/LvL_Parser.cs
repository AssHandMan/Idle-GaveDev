using System;
using System.Data;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class LvL_Parser : MonoBehaviour
{
    private int Level;
    private int Price;
    private int Gift;
    private int Gift_Free;
    private int Gift_Manager;
    private int Gift_SEO;
}
   /* public void ParseFromRow(ExcelWorksheet sheet, int row)
    {
        Level = Convert.ToInt32(sheet.Cells[row, 1].Value);
       
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
*/