using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Utility;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class FootballMain : SceneMainController {
	public FootballViewCtrl footballViewCtrl;
	
	[SerializeField]
	GamePlayCtrl gamePlayCtrl;

	public enum GameState {
		InGame,
		Staging
	}
	public GameState gameState {
		get {
			return _gameState;
		}
		set {
			if (OnGameStateChange != null)
				OnGameStateChange(value);

			_gameState = value;
		}
	}
	public GameState _gameState;

	public System.Action<GameState> OnGameStateChange = delegate {};

	protected override void Start() {
		gameState = GameState.Staging;
		base.Start();
		footballViewCtrl.loadingPanel.SetActive(true);
		footballViewCtrl.gameOverPanel.SetActive(false);
		footballViewCtrl.scorePanel.SetActive(false);
		footballViewCtrl.SwitchFootballVisibility(false);

		//Init();
	}

	protected override void Init() {
		base.Init();
		footballViewCtrl.loadingPanel.SetActive(false);

		footballViewCtrl.SetUp(this);
		gamePlayCtrl.SetUp(this);

		gamePlayCtrl.SetDifficulty(2);

		PerformGameReadyAction();
	}

	private void PerformGameReadyAction() {
		gameState = GameState.InGame;
		footballViewCtrl.Reset();

		footballViewCtrl.gameOverPanel.SetActive(false);
		footballViewCtrl.scorePanel.SetActive(true);
		footballViewCtrl.SwitchFootballVisibility(true);

		footballViewCtrl.UpdateScore(0,0);
		LateExecution(1, () => EnableCamera(false) );
	}

	private void EnableCamera(bool isOn) {

	}

	private void Reset() {
		gameState = GameState.Staging;

		footballViewCtrl.SwitchFootballVisibility(false);
	}
	
	public override void OnReceiveClientMsg(string p_event_id, string p_rawMsg) {
		switch (p_event_id)
		{
			case EventFlag.NetMessageID.SelectBackground:{
				JSONNode json = JSON.Parse(p_rawMsg);
				gamePlayCtrl.SetFootballField(json["message"].Value);
			}
			break;

			case EventFlag.NetMessageID.SelectDifficulty : {
				JSONNode json = JSON.Parse(p_rawMsg);
				int difficulty = int.Parse(json["message"].Value);
				gamePlayCtrl.SetDifficulty(difficulty);
			}
			break;

			case EventFlag.NetMessageID.ReadyBTClick:{
				//Enable Collider
				PerformGameReadyAction();
			}
			break;

			case EventFlag.NetMessageID.GameOver:
				footballViewCtrl.scorePanel.SetActive(false);
				footballViewCtrl.gameOverPanel.SetActive(true);
				StartCoroutine(
					LateExecution(4, () => {
						PerformGameReadyAction();
						// SendMessageToServer(EventFlag.NetMessageID.Restart, "", clientRequester.guid);
						// SceneManager.LoadScene("Part1LightingDemo", LoadSceneMode.Single);
					})
				);

				Reset();
			break;
		}
	}

	public static IEnumerator LateExecution(float p_delay_time, System.Action p_callback) {
		yield return new WaitForSeconds(p_delay_time);
		if (p_callback != null)
			p_callback();
	}

	public void OnIPSubmit(InputField p_input) {
		ConnectAsClient(p_input.text);
	}

}
