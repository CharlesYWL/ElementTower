using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerClickHandler
{
    private BuildManager bm;
    // Start is called before the first frame update
    void Start()
    {
        bm = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        bm.UpgradeClicked();
    }


}
