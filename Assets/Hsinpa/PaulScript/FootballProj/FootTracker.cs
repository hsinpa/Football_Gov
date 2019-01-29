using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FootTracker : MonoBehaviour {

	[SerializeField]
	private string InteractObjectTag = "Ball";

	[SerializeField]
	private GamePlayCtrl GamePlayCtrl;

	private Vector3 currentPos, previousPos, kickDir;
	private float velocity;

	private void Update () {
        if (GamePlayCtrl != null && previousPos != Vector3.zero) {
			
			currentPos = this.transform.position;
			velocity = (currentPos - previousPos).sqrMagnitude;
			kickDir = (currentPos - previousPos).normalized;
        }
		previousPos = this.transform.position;
	}

	// private void OnTriggerEnter(Collider other) {
	// 	if (other.tag == InteractObjectTag && GamePlayCtrl != null) {
	// 		GamePlayCtrl.KickBall(kickDir, velocity);
	// 	}
	// }

    void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == InteractObjectTag && GamePlayCtrl != null) {
			GamePlayCtrl.KickBall(kickDir, velocity);
		}
	}

	
}
