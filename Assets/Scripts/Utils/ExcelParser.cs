using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Models.GameData;
using Models.StationSettingsData;

public static class ExcelParser 
{
    public GameData ParseAll(string filePath)
    {
        var gameData = new GameData();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // нужно зачем-то, но хз зачем

        // тут чатгпт пошел
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Берем первый лист
            int rows = worksheet.Dimension.Rows;

            // Предполагается, что первая строка - это заголовки
            for (int row = 2; row <= rows; row++) // Начинаем со второй строки
            {
                var stationSettingsData = new StationSettingsData
                {
                    Id = worksheet.Cells[row, 1].Text,
                    Name = worksheet.Cells[row, 2].Text,
                    ActionType = worksheet.Cells[row, 3].Text,
                    ActionCost = float.Parse(worksheet.Cells[row, 4].Text),
                    RequiredUserLevel = int.Parse(worksheet.Cells[row,5].Text)
                };

                gameData.stationsSettings.Add(stationSettingsData);
            }
        }
    }
}