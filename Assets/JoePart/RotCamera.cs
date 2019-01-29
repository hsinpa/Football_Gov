using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCamera : MonoBehaviour {
    public float speed; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("RL") != 0)
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("RL")*speed, 0));
        }
	}
}
