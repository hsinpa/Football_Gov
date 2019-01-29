using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAT : MonoBehaviour {
    public Transform lookT;
    public Vector3 vector3 = new Vector3(0,90,0);
    public Transform posT;
    public Transform rotaObj;
    public bool UseNewUP;
    public Vector3 NewUp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (lookT)
        {
            if (UseNewUP)
            {
                transform.LookAt(lookT, NewUp);
            }
            else
            {
                transform.LookAt(lookT, ReadFrame.svf.forward);
            }
            transform.Rotate(vector3);
        }
        if (posT)
            transform.position = posT.position;

        if (rotaObj)
        {
            transform.rotation = rotaObj.rotation;
        }

	}
}
