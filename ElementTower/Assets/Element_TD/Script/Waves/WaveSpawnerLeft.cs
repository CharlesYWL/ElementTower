using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
public class WaveSpawnerLeft : MonoBehaviour
{
    public GameObject EnemyPrefabEasy;
    public GameObject EnemyPrefabMedium;
    public GameObject EnemyPrefabHard;
    public GameObject EnemyPrefabHardPlus;
    public GameObject EnemyPrefabBoss;

    public Transform SpawnPoint;

    public IEnumerator SpawnWave()
    {
        switch (WaveController.waveNumber)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                for (int i = 0; i < 5; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 5:
                for (int i = 0; i < 10; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 6:
                for (int i = 0; i < 10; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 7:
                for (int i = 0; i < 8; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 8:
                for (int i = 0; i < 8; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 9:
                for (int i = 0; i < 8; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 10:
                for (int i = 0; i < 8; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyBoss();
                    yield return new WaitForSeconds(1f);
                }
                break;


            default:
                Debug.Log("Exceeding Max Waves");
                break;
        }

    }

    private void SpawnEnemyEasy()
    {
        GameObject en = Instantiate(EnemyPrefabEasy, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement em = en.GetComponent<EnemyMovement>();
        em.SetTargets(WayPointsLeft.Leftpoints);
    }

    private void SpawnEnemyMedium()
    {
        GameObject en = Instantiate(EnemyPrefabMedium, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement em = en.GetComponent<EnemyMovement>();
        em.SetTargets(WayPointsLeft.Leftpoints);
    }

    private void SpawnEnemyHard()
    {
        GameObject en = Instantiate(EnemyPrefabHard, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement em = en.GetComponent<EnemyMovement>();
        em.SetTargets(WayPointsLeft.Leftpoints);
    }

    private void SpawnEnemyHardPlus()
    {
        GameObject en = Instantiate(EnemyPrefabHardPlus, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement em = en.GetComponent<EnemyMovement>();
        em.SetTargets(WayPointsLeft.Leftpoints);
    }

    private void SpawnEnemyBoss()
    {
        GameObject en = Instantiate(EnemyPrefabBoss, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement em = en.GetComponent<EnemyMovement>();
        em.SetTargets(WayPointsLeft.Leftpoints);
    }
}
