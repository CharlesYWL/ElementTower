using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class EnemiesInspector : MonoBehaviour
{
	public GameObject enemyPrefab;

	void OnEnable()
	{
		Debug.Assert(enemyPrefab, "Wrong stuff settings");
	}

	public GameObject AddEnemy()
	{
		GameObject enemy = Instantiate(enemyPrefab);
		enemy.name = enemyPrefab.name;
		enemy.transform.SetAsLastSibling();
		return enemy;
	}
}
#endif
