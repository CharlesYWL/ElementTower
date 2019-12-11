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

    public float TimeBetweenWaves = 3f;
    private float countdown = 24f;
    private WaveSpawnerBot wb;
    private WaveSpawnerLeft wl;
    private WaveSpawnerTop wt;

    private GameObject[] EnemyList;

    private void Start()
    {
        wb = GateBot.GetComponent<WaveSpawnerBot>();
        wl = GateLeft.GetComponent<WaveSpawnerLeft>();
        wt = GateTop.GetComponent<WaveSpawnerTop>();
    }

    private void Update()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (EnemyList.Length != 0)
        {
            countdown = TimeBetweenWaves;
        }

        if (countdown <= 0f && EnemyList.Length == 0 && TimeRotation.Day == false)
        {
            waveNumber++;

            StartCoroutine(wb.SpawnWave());
            StartCoroutine(wl.SpawnWave());
            StartCoroutine(wt.SpawnWave());
            countdown = TimeBetweenWaves;

            return;
        }

        // Win Stage
        if (waveNumber >= 11)
        {
            BuildManager bm = BuildManager.instance;
            bm.WinGame();
        }

        countdown -= Time.deltaTime;
    }


}
