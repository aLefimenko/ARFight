using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTransform : MonoBehaviour
{

    [SerializeField] private GameObject _pricelPosition;

	
	void Update () {
	    transform.position = Vector3.Lerp(transform.position, _pricelPosition.transform.position, Time.deltaTime * 10f);
	}
}
