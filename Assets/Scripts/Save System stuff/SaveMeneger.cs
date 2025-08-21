using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMeneger
{
    private static string path = Application.persistentDataPath + "/player.fun";
    private static string pathUpgreade = Application.persistentDataPath + "/player.fun2";
    private static string pathWallet = Application.persistentDataPath + "/player.fun3";
    private static string pathMenegers = Application.persistentDataPath + "/player.fun4";
    private static string pathExpLvl = Application.persistentDataPath + "/player.fun5";
    private static string pathSkin = Application.persistentDataPath + "/player.fun6";
    private static string pathMap = Application.persistentDataPath + "/player.fun6";
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

    public static void SavePlayer(UpgreadeRoom room)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathUpgreade, FileMode.Create))
        {
            PlayerData data = new PlayerData(room);
            formatter.Serialize(stream, data);
        }
    }
    public static PlayerData LoadPlayer()
    {
        if (File.Exists(pathUpgreade))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathUpgreade, FileMode.Open))
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SaveWallet(PlayerWallet wallet)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathWallet, FileMode.Create))
        {
            WalletData data = new WalletData(wallet);
            formatter.Serialize(stream, data);
        }
    }
    public static WalletData LoadWallet()
    {
        if (File.Exists(pathWallet))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathWallet, FileMode.Open))
            {
                WalletData data = formatter.Deserialize(stream) as WalletData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SaveMenegers(HrRoom room)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathMenegers, FileMode.Create))
        {
            MenegersData data = new MenegersData(room);
            formatter.Serialize(stream, data);
        }
    }
    public static MenegersData LoadMenegers()
    {
        if (File.Exists(pathMenegers))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathMenegers, FileMode.Open))
            {
                MenegersData data = formatter.Deserialize(stream) as MenegersData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SaveExpLvl(ExpLvlController controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathExpLvl, FileMode.Create))
        {
            ExpLvlData data = new ExpLvlData(controller);
            formatter.Serialize(stream, data);
        }
    }
    public static ExpLvlData LoadExpLvl()
    {
        if (File.Exists(pathExpLvl))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathExpLvl, FileMode.Open))
            {
                ExpLvlData data = formatter.Deserialize(stream) as ExpLvlData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SaveSkin(SkinsRoom room)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathSkin, FileMode.Create))
        {
            SkinData data = new SkinData(room);
            formatter.Serialize(stream, data);
        }
    }
    public static SkinData LoadSkin()
    {
        if (File.Exists(pathSkin))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathSkin, FileMode.Open))
            {
                SkinData data = formatter.Deserialize(stream) as SkinData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SaveGlobalMap(GlobalMapMainController controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(pathMap, FileMode.Create))
        {
            GlobalMapData data = new GlobalMapData(controller);
            formatter.Serialize(stream, data);
        }
    }
    public static GlobalMapData LoadGlobalMap()
    {
        if (File.Exists(pathMap))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathMap, FileMode.Open))
            {
                GlobalMapData data = formatter.Deserialize(stream) as GlobalMapData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }
}
