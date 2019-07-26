using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string savePersistentDataPath = Application.persistentDataPath + "/savepersistentdata.mbepus";
    private static string saveGlobalMoneyDataPath = Application.persistentDataPath + "/saveglobalmoneydata.mbepus";
    private static string savePlayerLootPoolDataPath = Application.persistentDataPath + "/saveplayerlootpooldata.mbepus";
    private static string saveVendorLootPoolDataPath = Application.persistentDataPath + "/savevendorlootpooldata.mbepus";


    public static void SavePersistentData(PersistentGameData PersistentGameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePersistentDataPath, FileMode.Create);
        SavePersistentData data = new SavePersistentData(PersistentGameData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGlobalMoneyData(PersistentGameData PersistentGameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveGlobalMoneyDataPath, FileMode.Create);
        SaveGlobalMoney data = new SaveGlobalMoney(PersistentGameData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePlayerLootPoolData(PersistentGameData PersistentGameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePlayerLootPoolDataPath, FileMode.Create);
        SavePlayerLootPool data = new SavePlayerLootPool(PersistentGameData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveVendorLootPoolData(PersistentGameData PersistentGameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveVendorLootPoolDataPath, FileMode.Create);
        SaveVendorLootPool data = new SaveVendorLootPool(PersistentGameData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavePersistentData LoadPersistentData()
    {
        if (File.Exists(savePersistentDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePersistentDataPath, FileMode.Open);
            SavePersistentData data = formatter.Deserialize(stream) as SavePersistentData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + savePersistentDataPath);
            return null;
        }
    }

    public static SaveGlobalMoney LoadMoneyData()
    {
        if (File.Exists(saveGlobalMoneyDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveGlobalMoneyDataPath, FileMode.Open);
            SaveGlobalMoney data = formatter.Deserialize(stream) as SaveGlobalMoney;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + saveGlobalMoneyDataPath);
            return null;
        }
    }

    public static SavePlayerLootPool LoadPlayerLootPoolData()
    {
        if (File.Exists(savePlayerLootPoolDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePlayerLootPoolDataPath, FileMode.Open);
            SavePlayerLootPool data = formatter.Deserialize(stream) as SavePlayerLootPool;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + savePlayerLootPoolDataPath);
            return null;
        }
    }

    public static SaveVendorLootPool LoadVendorLootPoolData()
    {
        if (File.Exists(saveVendorLootPoolDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveVendorLootPoolDataPath, FileMode.Open);
            SaveVendorLootPool data = formatter.Deserialize(stream) as SaveVendorLootPool;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + saveVendorLootPoolDataPath);
            return null;
        }
    }

    public static void ResetGlobalMoneyData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveGlobalMoneyDataPath, FileMode.Create);
        SaveGlobalMoney data = new SaveGlobalMoney();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void ResetPlayerLootPoolData(LootLedger LootLedger)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePlayerLootPoolDataPath, FileMode.Create);
        SavePlayerLootPool data = new SavePlayerLootPool(LootLedger);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void ResetVendorLootPoolData(LootLedger LootLedger)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveVendorLootPoolDataPath, FileMode.Create);
        SaveVendorLootPool data = new SaveVendorLootPool(LootLedger);
        formatter.Serialize(stream, data);
        stream.Close();
    }
}
