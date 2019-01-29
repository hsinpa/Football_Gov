using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UpdateUnityEvent : MonoBehaviour {
    public UnityEvent Event;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Event.Invoke();
	}
}
