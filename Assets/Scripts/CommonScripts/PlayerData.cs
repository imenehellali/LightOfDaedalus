using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int currentLevel;

    public string[] unlockedColors;

    public string[] unlockedObstacles;

    public PlayerData (int currentLevel, string[] unlockedColors, string[] unlockedObstacles)
    {
        this.currentLevel = currentLevel;
        this.unlockedColors = new string[unlockedColors.Length];
        this.unlockedObstacles=new string[unlockedObstacles.Length];

        for(int i=0; i < unlockedColors.Length; i++)
            this.unlockedColors[i] = unlockedColors[i];
        for (int j = 0; j < unlockedObstacles.Length; j++)
            this.unlockedObstacles[j] = unlockedObstacles[j];
    }

    
}
