using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heal : MonoBehaviour
{
    public Image healBar; 
    float healStatus;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "shoot")
        {
            healStatus = healBar.GetComponent<Image>().fillAmount;
            healStatus -= 0.1f;
            healBar.GetComponent<Image>().fillAmount = healStatus;
        }
        if (healStatus <= 0) 
        {
            healStatus = 1;
            healBar.GetComponent<Image>().fillAmount = healStatus;
        }
        
    }
}
