using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertController : MonoBehaviour
{

    [SerializeField]public Image alertIcon;
    // Start is called before the first frame update
    void Start()
    {
        alertIcon = GetComponent<Image>();
        DeActivateAlert();
    }

    void Update(){
		if (Input.GetKeyDown(KeyCode.R)) {
            DeActivateAlert();
		}
        else if (Input.GetKeyDown(KeyCode.B)){
            ActivateAlert();
        }
	}

    public void ActivateAlert(){
        alertIcon.color = new Color(alertIcon.color.r, alertIcon.color.g, alertIcon.color.b, 1f);
    }

    public void DeActivateAlert(){
        alertIcon.color = new Color(alertIcon.color.r, alertIcon.color.g, alertIcon.color.b, 0.1f);
    }
}
