using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foot : MonoBehaviour {
    public bool mode;
    Rigidbody mrb;
    public Transform t;
    public Rigidbody ball;
    public Transform hitbox;
    public Vector3 onepos;
    public Vector3 twopos;
    public float speed = 1; 
	// Use this for initialization
	void Start () {
        mrb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hitbox) {
            
        }
	}

    private void FixedUpdate()
    {
        mrb.MovePosition(t.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mode)
        {
            Debug.Log("hitball");
            Debug.Log("" + GetComponent<Rigidbody>().velocity);
            onepos = transform.position;
            ball = other.GetComponent<Rigidbody>();
            Invoke("ballout", 0.05f);
           
        }
        else
        {
            Debug.Log("hitball");
            Debug.Log("" + GetComponent<Rigidbody>().velocity);
            other.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            
        }
    }
    
    void ballout()
    {
        twopos = transform.position;
        ball.AddForce((twopos - onepos) * speed);
       // ball.velocity = (twopos - onepos)*speed;
    }
}
