using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
public class WaveSpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform spawnPoint;
    public float TimeBetweenWaves = 5f;
    private float countdown = 3f;
    private int waveNumber = 0;
    private int MaxWave = 10;
    
    private void Update()
    {
        if(countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = TimeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        for (int i = 0; i < waveNumber; i++)
        {

            if(waveNumber < MaxWave)
            {
                SpawnEnemy();
            }
            else
            {
                waveNumber = 0;
            }
            yield return new WaitForSeconds(1f);
        }        
    }

    public void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
