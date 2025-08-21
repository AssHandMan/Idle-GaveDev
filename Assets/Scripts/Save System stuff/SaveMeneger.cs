using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMeneger
{
    public static void SavePlayer(UpgreadeRoom room)
    {
        var formatter = new BinaryFormatter();
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/player_{sceneName}.fun2";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new PlayerData(room);
            formatter.Serialize(stream, data);
        }
    }

    public static PlayerData LoadPlayer()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/player_{sceneName}.fun2";

        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as PlayerData;
        }
    }

    public static void SaveMapProgress(UpgreadeSceneMeneger scene)
    {
        var formatter = new BinaryFormatter();
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/map_{sceneName}.fun";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new UpgreadeMapData(scene);
            formatter.Serialize(stream, data);
        }
    }

    public static UpgreadeMapData LoadMapProgress()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/map_{sceneName}.fun";

        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as UpgreadeMapData;
        }
    }

    public static void SaveMenegers(HrRoom room)
    {
        var formatter = new BinaryFormatter();
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/menegers_{sceneName}.fun4";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new MenegersData(room);
            formatter.Serialize(stream, data);
        }
    }

    public static MenegersData LoadMenegers()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string path = Application.persistentDataPath + $"/menegers_{sceneName}.fun4";

        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as MenegersData;
        }
    }

    public static void SaveWallet(PlayerWallet wallet)
    {
        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun3";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new WalletData(wallet);
            formatter.Serialize(stream, data);
        }
    }

    public static WalletData LoadWallet()
    {
        string path = Application.persistentDataPath + "/player.fun3";
        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as WalletData;
        }
    }

    public static void SaveExpLvl(ExpLvlController controller)
    {
        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun5";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new ExpLvlData(controller);
            formatter.Serialize(stream, data);
        }
    }

    public static ExpLvlData LoadExpLvl()
    {
        string path = Application.persistentDataPath + "/player.fun5";
        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as ExpLvlData;
        }
    }

    public static void SaveSkin(SkinsRoom room)
    {
        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun6";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new SkinData(room);
            formatter.Serialize(stream, data);
        }
    }

    public static SkinData LoadSkin()
    {
        string path = Application.persistentDataPath + "/player.fun6";
        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as SkinData;
        }
    }

    public static void SaveGlobalMap(GlobalMapMainController controller)
    {
        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/globalmap.fun7";

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var data = new GlobalMapData(controller);
            formatter.Serialize(stream, data);
        }
    }

    public static GlobalMapData LoadGlobalMap()
    {
        string path = Application.persistentDataPath + "/globalmap.fun7";
        if (!File.Exists(path)) return null;

        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(stream) as GlobalMapData;
        }
    }
}
