using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTransform : MonoBehaviour
{

    [SerializeField] private GameObject _pricelPosition;

	
	void Update () {
        //transform.position = Vector3.MoveTowards(transform.position, _pricelPosition.transform.forward, Time.deltaTime*30f);
	    transform.position = Vector3.Lerp(transform.position, _pricelPosition.transform.forward, Time.deltaTime * 10f);
	}
}
