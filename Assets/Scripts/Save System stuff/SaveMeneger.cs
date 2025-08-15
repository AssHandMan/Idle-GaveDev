using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMeneger
{
    private static string path = Application.persistentDataPath + "/player.fun";

    public static void SaveMapProgress(UpgreadeSceneMeneger scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            UpgreadeMapData data = new UpgreadeMapData(scene);
            formatter.Serialize(stream, data);
        }
    }
    public static UpgreadeMapData LoadMapProgress()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                UpgreadeMapData data = formatter.Deserialize(stream) as UpgreadeMapData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }
}
