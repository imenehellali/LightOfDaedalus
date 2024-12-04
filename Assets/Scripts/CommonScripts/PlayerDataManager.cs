using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerDataManager 
{

    public static List<string> _unlockedColors = new List<string>();
    public static List<string> _unlockedObstacles = new List<string>();
    public static int _currentLevel = 1;

    private static PlayerData _playerData;

    public static int GetCurrentLevel=>_playerData.currentLevel;
    
    public static List<string> GetUnlockedObstacles => new List<string>(_playerData.unlockedObstacles);
    public static List<string> GetUnlockedColors => new List<string>(_playerData.unlockedColors);
    public static void UpdateUnlockedObstacles(string newObstacleUnlocked)
    {
        _playerData.unlockedObstacles = null;
        _playerData.unlockedObstacles = new string[_unlockedObstacles.Count];
        _unlockedObstacles.ForEach(a => _playerData.unlockedObstacles[_unlockedObstacles.IndexOf(a)] = a);
    }

    public static void UpdateUnlockedColors(string newColorUnlocked)
    {
        _playerData.unlockedColors = null;
        _playerData.unlockedColors = new string[_unlockedColors.Count];
        _unlockedColors.ForEach(a => _playerData.unlockedColors[_unlockedColors.IndexOf(a)] = a);
    }

    public static void UpdateCurrentLevel(int newCurrentLevel)
    {
        _currentLevel = newCurrentLevel;
        _playerData.currentLevel= newCurrentLevel;
    }
    public static void CreateDataAsset()
    {
        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + "/playerData.txt";
        Debug.Log("Path to playerData from create data asset:            "+_path);
        FileStream _stream = new FileStream(_path, FileMode.Create);

        _unlockedColors.Add("Yellow");
        _unlockedColors.Add("Blue");
        _unlockedObstacles.Add("ReflectiveSurface");
        _currentLevel = 1;

        _playerData = new PlayerData(_currentLevel,_unlockedColors.ToArray(),_unlockedObstacles.ToArray());


        _formatter.Serialize(_stream, _playerData);
        _stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        Debug.Log("entered PlayerDataManager");
        string _path = Application.persistentDataPath + "/playerData.txt";
        //if(File.Exists(_path)&&_playerData!=null)
        //{
        //    return _playerData;
        //}
        if (File.Exists(_path))
        {
            Debug.Log("path exists:      " + _path);
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            _playerData = _formatter.Deserialize(_stream) as PlayerData;
            UpdateCurrentLevel(_currentLevel);
            UpdateUnlockedColors("");
            UpdateUnlockedObstacles("");

            _stream.Close();
            return _playerData;
        }
        else
        {
            Debug.Log("path doesn't exist cogga create new player data");
            CreateDataAsset();
            return _playerData;
        }
    }
}
