using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FootballViewCtrl : MonoBehaviour {
	[SerializeField]
	Transform footballField;

	public GameObject gameOverPanel;
	public GameObject scorePanel;
	public GameObject loadingPanel;

	private int _score = 0;
	private int _round = 0;

	[SerializeField]
	private int _maxRound = 10;

	FootballMain footballMain;

	public void SetUp(FootballMain footballMain)
	{
		this.footballMain = footballMain;
	}

	public void UpdateScore(int addScore, int addRound) {
		_score += addScore;
		_round += addRound;

		Text scoreUI = scorePanel.transform.Find("score").GetComponent<Text>();
		Text remainingUI = scorePanel.transform.Find("remaining").GetComponent<Text>();

		scoreUI.text = _score + "分";
		remainingUI.text = "剩餘"+ (_maxRound - _round) + "球";

		if (_round == _maxRound) {
			//End game
			Text endGameText = gameOverPanel.GetComponentInChildren<Text>();
			endGameText.text = "總計: "+ _score +"分\n請脫下VR頭盔 並放回原位上";

			this.footballMain.OnReceiveClientMsg(EventFlag.NetMessageID.GameOver, "");
		}

	}

	public void SwitchFootballVisibility(bool isVisible) {
		int maxFarPlane = 100;
		footballField.gameObject.SetActive(isVisible);

		// if (footcourtFieldCamera != null) {
		// 	DOTween.KillAll();

		// 	footcourtFieldCamera.gameObject.SetActive(isVisible);
		// 	footcourtFieldCamera.DOFarClipPlane((isVisible) ? maxFarPlane : 0.31f, 1.3f);
		// }
	}

	public void Reset() {
		_score = 0;
		_round = 0;
	}
}
