using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]public Canvas miniMap;

    void Update(){
		if (Input.GetKeyDown(KeyCode.M)) {
            showMiniMap(true);
		}
	}

    public void showMiniMap(bool show){
        miniMap.enabled = show;
    }
}
