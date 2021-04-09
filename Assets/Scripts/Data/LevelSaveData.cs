using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public SingleTile[] savedTiles;
    public Enemy[] savedEnemies;
    
    public int leftTopX;
    public int leftTopY;
    public int rightDownX;
    public int rightDownY;
    public int sizeX;
    public int sizeY;
    
    public int enemiesSize;
    public Vector3 playerPosition;
    public Vector3 winPointPosition;
    public int attack;
    public int hp;
    public float speed;
    public float jump;

}