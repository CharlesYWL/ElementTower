using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class FeaturesInspector : MonoBehaviour
{
	public List<AiFeature> featuresList = new List<AiFeature>();

	void OnEnable()
	{
		featuresList.Clear();
		foreach (AiFeature feature in GetComponentsInChildren<AiFeature>())
		{
			featuresList.Add(feature);
		}
	}

	public GameObject AddFeature(GameObject featurePrefab)
	{
		GameObject res = null;
		AiFeature feature = featurePrefab.GetComponent<AiFeature>();
		if (feature != null)
		{
			res = Instantiate(featurePrefab, transform);
			res.name = featurePrefab.name;
			featuresList.Add(res.GetComponent<AiFeature>());
		}
		return res;
	}

	public GameObject GetNextFeature(GameObject currentSelected)
	{
		return InspectorsUtil<AiFeature>.GetNext(transform, currentSelected);
	}

	public GameObject GetPreviousFeature(GameObject currentSelected)
	{
		return InspectorsUtil<AiFeature>.GetPrevious(transform, currentSelected);
	}
}
#endif
