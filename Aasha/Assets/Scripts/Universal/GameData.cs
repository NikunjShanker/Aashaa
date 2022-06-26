using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float[] savedPos;
    public int savedSceneIndex;
    public int maxHealth;
    public bool[] heartGained;
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public bool jumpBoost;
    public bool speedBoost;

    public GameData(UniversalScript script)
    {
        savedPos = new float[3];
        savedPos[0] = script.savedPos.x;
        savedPos[1] = script.savedPos.y;
        savedPos[2] = script.savedPos.z;

        savedSceneIndex = script.savedSceneIndex;
        maxHealth = script.maxHealth;
        heartGained = new bool[9];
        heartGained = script.heartGained;
        canDoubleJump = script.canDoubleJump;
        canWallJump = script.canWallJump;
        canDash = script.canDash;
        jumpBoost = script.jumpBoost;
        speedBoost = script.speedBoost;
    }
}
