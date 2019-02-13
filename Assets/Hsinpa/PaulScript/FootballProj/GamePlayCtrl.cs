using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class GamePlayCtrl : MonoBehaviour {
	[SerializeField]
	GameObject FootballCore;
	GoalKeeperLevel goalKeeperLevel;

	FootballMain footballMain;

	public Shoot shootAI;
	public GoalKeeper goalKeeper;

	[Range(0, 35)]
	public float force = 1;

	private float maxBallFlightTime = 2;
	private float recordBallFlightTime;

	public Cubemap[] cubeMaps;
	public GameObject cubeMapsHolder;


	public void SetUp(FootballMain footballMain)
	{
		if (FootballCore == null) return;
		this.footballMain = footballMain;
		recordBallFlightTime = 0;

		goalKeeperLevel = FootballCore.GetComponentInChildren<GoalKeeperLevel>();

		GoalDetermine.EventFinishShoot += OnGoalEvent;

        Reset();
	}

	public void SetDifficulty(int p_difficulty) {
		if (goalKeeperLevel != null) {
			goalKeeperLevel.setLevel(p_difficulty);
		}
	}

	public void SetFootballField(string p_field) {

		for (int i = 0 ; i < cubeMaps.Length; i++) {
			if (cubeMaps[i].name == p_field) {
				MeshRenderer mesh = cubeMapsHolder.GetComponent<MeshRenderer>();
				mesh.material.SetTexture("_Tex", cubeMaps[i]);

				return;
			}
		}
	}

	private void Update() {
		if (recordBallFlightTime != 0 && recordBallFlightTime < Time.time) {
			recordBallFlightTime = 0;
			//this.footballMain.footballViewCtrl.UpdateScore(0);
			//Reset();
		}
		
		if (this.footballMain != null && this.footballMain.gameState == FootballMain.GameState.InGame) {
			// if (shootAI != null && Input.GetMouseButtonUp(0) ) {
			// 	Vector3 dir = (goalKeeper.transform.position - shootAI._ball.transform.position).normalized;

			// 	KickBall(dir, this.force); 
			// }
			if (shootAI != null && Input.GetKeyDown(KeyCode.S) ) {
				Vector3 dir = (goalKeeper.transform.position - shootAI._ball.transform.position).normalized;

				KickBall(dir, 4f); 
			}

		}
	}

	private void OnGoalEvent (bool isGoal, Area area) {
        StartCoroutine(
            FootballMain.LateExecution(1, Reset)
        );
        bool isMaxRound = this.footballMain.footballViewCtrl.UpdateScore((isGoal) ? 1 : 0, 1);


        if (isGoal && !isMaxRound)
            AudioManager.instance.PlayAudio(this.gameObject, EventFlag.Audio.Hurrah, 1);
	}
    
	private void Reset() {
        ShootAI.shareAI.reset();                // used this method to reset new randomised ball's position

        SlowMotion.share.reset();                   // reset the slowmotion logic

        GoalKeeperHorizontalFly.share.reset();      // reset goalkeeperhorizontalfly logic
        GoalKeeper.share.reset();                   // reset goalkeeper logic
        GoalDetermine.share.reset();                // reset goaldetermine logic so that it's ready to detect new goal

        // if (Wall.share != null)                     // if there is wall in this scene
        // {
        //     Wall.share.IsWall = IsWallKick;         // set is wall state
        //     if (IsWallKick)                         // if we want wall kick
        //         Wall.share.setWall(Shoot.share._ball.transform.position);       // set wall position with respect to ball position
        // }

        AudioManager.instance.PlayAudio(this.gameObject, EventFlag.Audio.Whistle, 1f);
    }

    public void KickBall(Vector3 p_direction, float p_force) {
		if (this.footballMain != null && this.footballMain.gameState == FootballMain.GameState.InGame) {
				shootAI._ball.velocity += p_direction * force;
			if (p_force < 1) {
				//shootAI._ball.velocity *= 1.2f; 
			}

			if(Shoot.EventShoot != null) {
				Shoot.EventShoot();
			}
			recordBallFlightTime = Time.time + maxBallFlightTime;

            AudioManager.instance.PlayAudio(this.gameObject, EventFlag.Audio.KickBall, 0.8f);
        }
    }

	void OnDestroy()
	{
		GoalDetermine.EventFinishShoot -= OnGoalEvent;
	}

}
