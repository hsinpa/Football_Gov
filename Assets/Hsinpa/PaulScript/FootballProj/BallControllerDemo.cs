using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerDemo : MonoBehaviour {

	public Shoot shootAI;
	public GoalKeeper goalKeeper;
	[Range(0, 30)]
	public float force;

	private void Start() {
		GoalDetermine.EventFinishShoot += delegate(bool isGoal, Area area) {
			Debug.Log("Goal isIN " + isGoal +", "  + area.ToString("g"));
		};


	}

	private void Update() {
		if (shootAI != null && Input.GetMouseButtonUp(0)) {
			Vector3 dir = (goalKeeper.transform.position - shootAI._ball.transform.position).normalized;

			//KickBall(dir, this.force); 
		}
	}

	

	private void KickBall(Vector3 p_direction, float p_force) {
		Debug.Log(p_direction * p_force);
		shootAI._ball.velocity = p_direction * p_force;

		if(Shoot.EventShoot != null) {
			Shoot.EventShoot();
		}
	}

}
