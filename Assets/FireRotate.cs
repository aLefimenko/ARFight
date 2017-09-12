using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRotate : MonoBehaviour
{

    private float[] _coordinats;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    /*//_coordinats = BaseSDK.GetAxel();
		//transform.RotateAround(Camera.main.transform.position,new Vector3(_coordinats[0],_coordinats[1],_coordinats[2]),Time.deltaTime*5f);
        //transform.Rotate(_coordinats);
        //transform.eulerAngles=new Vector3(_coordinats[0]*-180,_coordinats[1]*-180f,0);
        transform.Rotate(new Vector3(_coordinats[0],_coordinats[2],-_coordinats[2])*0.5f);*/
	}

}
