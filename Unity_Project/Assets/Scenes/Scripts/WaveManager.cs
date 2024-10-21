using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float countdown;
    [SerializeField] GameObject spawnPoint;
    public Wave[] waves;
    int currentWaveIndex = 0;
    
    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            SpawnWave();
        }
    }
    void SpawnWave()
    {
        for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
        {
            Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);
        }
    }
}
[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
}
