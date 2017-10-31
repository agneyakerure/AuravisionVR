using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    // number of enemies on z
    public int numZ;

    // number of enemies on x
    public int numX;

    // number of layers
    public int numY;

    // separation
    public float separation;

    // enemy prefab
    public GameObject enemyPrefab;

    // number of enemies
    public int numEnemies;

    // creation of an enemy wave
    public void CreateEnemyWave()
    {
        // calculate number of enemies
        numEnemies = numZ * numX * numY;

        Vector3 startPos = transform.position;

        for (int k = 0; k < numY; k++)
        {
            for (int j = 0; j < numX; j++)
            {
                for (int i = 0; i < numZ; i++)
                {
                    // spawn enemy
                    GameObject newEnemy = Instantiate(enemyPrefab);

                    // set enemy position
                    newEnemy.transform.position = new Vector3(startPos.x + separation * j, startPos.y + separation * k, startPos.z + separation * i);
                }
            }
        }          
    }

    public void KillAll()
    {
        //find all the enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //iterate through these enemies and destroy them
        for(int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }

}
