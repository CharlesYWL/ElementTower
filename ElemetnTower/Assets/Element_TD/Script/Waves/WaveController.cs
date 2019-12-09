using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static int waveNumber = 0;
    [SerializeField]
    public GameObject Gate1;
    [SerializeField]
    public GameObject Gate2;
    [SerializeField]
    public GameObject Gate3;

    public float TimeBetweenWaves = 10f;
    private float countdown = 3f;
    private float TimeInBattle = 60.0f;
    private WaveSpawnerBot wb;
    private WaveSpawnerLeft wl;
    private WaveSpawnerTop wt;

    private void Start()
    {
        wb = Gate1.GetComponent<WaveSpawnerBot>();
        wl = Gate2.GetComponent<WaveSpawnerLeft>();
        wt = Gate3.GetComponent<WaveSpawnerTop>();
    }

    private void Update()
    {

        if (countdown <= 0f)
        {
            waveNumber++;
            Debug.Log("Wave: " + waveNumber);
            
            StartCoroutine(wb.SpawnWave());
            StartCoroutine(wl.SpawnWave());
            StartCoroutine(wt.SpawnWave());
            countdown = TimeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }


}
