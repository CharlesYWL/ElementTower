using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
public class WaveSpawnerBot : MonoBehaviour
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
                for (int i = 0; i < 5; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 2:
                for (int i = 0; i < 10; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 3:
                for (int i = 0; i < 10; i++)
                {
                    SpawnEnemyEasy();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyMedium();
                    yield return new WaitForSeconds(1f);
                }
                break;

            case 4:
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

            case 5:
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

            case 6:
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
        em.SetTargets(WayPoints.points);
    }

    private void SpawnEnemyMedium()
    {
        GameObject en = Instantiate(EnemyPrefabMedium, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement emMedium = en.GetComponent<EnemyMovement>();
        emMedium.SetTargets(WayPoints.points);
    }

    private void SpawnEnemyHard()
    {
        GameObject enHard = Instantiate(EnemyPrefabHard, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement emHard = enHard.GetComponent<EnemyMovement>();
        emHard.SetTargets(WayPoints.points);
    }

    private void SpawnEnemyHardPlus()
    {
        GameObject enHardPlus = Instantiate(EnemyPrefabHard, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement emHardPlus = enHardPlus.GetComponent<EnemyMovement>();
        emHardPlus.SetTargets(WayPoints.points);
    }

    private void SpawnEnemyBoss()
    {
        GameObject enHardBoss = Instantiate(EnemyPrefabHard, SpawnPoint.position, SpawnPoint.rotation);
        EnemyMovement emHardBoss = enHardBoss.GetComponent<EnemyMovement>();
        emHardBoss.SetTargets(WayPoints.points);
    }
}
