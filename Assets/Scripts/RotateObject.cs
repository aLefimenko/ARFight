using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    [SerializeField] private GameObject camera;

    private float speed;

	void Start () {
        StartCoroutine(ChangeSpeed());
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(camera.transform.position, Vector3.up, Time.deltaTime * 40f);
	}

    IEnumerator ChangeSpeed()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        speed = Random.Range(10f, 100f);
        Repeat();
    }

    void Repeat()
    {
        StartCoroutine(ChangeSpeed());
    }
}
