using System;

enum ActionType
{
    Buy = "Покупка",
    Expand = "Расширение",
    Upgrade = "Улучшение",
}
public class StationSettingsData
{
    public string Id { get; set; } // потому что тут промежуток, думаю, так легче
    public string Name { get; set; }
    public ActionType ActionType { get; set; }
    public float ActionCost { get; set; }
    public int RequiredUserLevel { get; set; } = null;
    
    // не понял про что последний столбец в таблице димы

};