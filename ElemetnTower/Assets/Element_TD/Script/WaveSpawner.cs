﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform spawnPoint;
    public float TimeBetweenWaves = 5f;
    [SerializeField] private float countdown = 3f;
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int MaxWave = 10;
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
            SpawnEnemy();
            if(waveNumber > 10)
            {
                waveNumber = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
