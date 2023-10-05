using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector2 enemyAreaRange;
    public Transform[] spawnPoints;
    public int spawnCount;

    void Start()
    {
        
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            
        }
    }
}
