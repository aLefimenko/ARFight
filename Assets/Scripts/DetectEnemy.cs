using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        Debug.Log("int trig");
        if(col.CompareTag("controller"))
        {
            Debug.Log("enem");
            BaseSDK.StopGame();
        }
    }
}
