using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour {

    private GameObject controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("controller");
    }

	void Update () {
        if (Vector3.Distance(transform.position, controller.transform.position) < 1f)
        {
           // gameObject.GetComponent<Animation>().Play("attack1");
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(controller.transform.position.z, controller.transform.position.y - 0.5f, controller.transform.position.z), Time.deltaTime * 0.2f);
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("controller"))
        {
            gameObject.GetComponent<Animation>().Play("attack1");
            StartCoroutine(Attack(col.gameObject));
           
        }
    }

    IEnumerator Attack(GameObject coll)
    {
        yield return new WaitForSeconds(1f);
        coll.GetComponent<ControllerScript>().Attack();
        //BaseSDK.Vibro();
        Repeat(coll);
    }

    void Repeat(GameObject colll)
    {
        StartCoroutine(Attack(colll));
    }
}
