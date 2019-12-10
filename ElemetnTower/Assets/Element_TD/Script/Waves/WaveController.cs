using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static int waveNumber = 0;
    [SerializeField]
    public GameObject GateBot;
    [SerializeField]
    public GameObject GateLeft;
    [SerializeField]
    public GameObject GateTop;

    public float TimeBetweenWaves = 9f;
    private float countdown = 24f;
    private float TimeInBattle = 15f;
    private WaveSpawnerBot wb;
    private WaveSpawnerLeft wl;
    private WaveSpawnerTop wt;

    private void Start()
    {
        wb = GateBot.GetComponent<WaveSpawnerBot>();
        wl = GateLeft.GetComponent<WaveSpawnerLeft>();
        wt = GateTop.GetComponent<WaveSpawnerTop>();
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
