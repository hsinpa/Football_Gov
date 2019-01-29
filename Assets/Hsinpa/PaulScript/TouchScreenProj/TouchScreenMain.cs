using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Utility;

public class TouchScreenMain : SceneMainController {
	public GameObject loadingPage;

	public Transform readyButton;
	public Transform nextPageButton;
	[HideInInspector]
	public string currentSelectedField = "";

	public Text ip_address;
	public Transform difficulty_holderbox;

	public Text difficultyText;
	public TouchSerialPort torchSerialPort;


	public bool isDebugMode = false;
	public int port_number = 1;

	private LongPressButton[] longPressButtons;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		loadingPage.SetActive(true);
		torchSerialPort = new TouchSerialPort(port_number, isDebugMode);	
	}

	protected override void Init() {
		base.Init();

		loadingPage.SetActive(false);
		ip_address.text = selfIP;
        //Default value
        ChangeFramePage("page1");
        SetInGameBGModel("grass");
        SetDifficulty(1);

        SetReady(false);
		AllButtonSwitcher(true);
	}

	public void SetInGameBGModel(string p_model) { 
		if (this.client == null) return;

		SendMessageToServer(EventFlag.NetMessageID.SelectBackground, p_model, clientRequester.guid);

		//
		if (nextPageButton != null) {
			Text nextPageText = nextPageButton.GetComponentInChildren<Text>();
			nextPageText.text = "地形 "+ p_model.ToUpper() + ", 點選下一頁";
		}
	}

	public void SetDifficulty(int p_difficulty) {
		difficultyText.text = "遊戲等級 : " + p_difficulty;
		SendMessageToServer(EventFlag.NetMessageID.SelectDifficulty, p_difficulty.ToString(), clientRequester.guid);

		SetDifficultyUI(p_difficulty);
	}

	private void SetDifficultyUI(int p_difficulty) {
		if (difficulty_holderbox != null) {
			foreach (Transform c in difficulty_holderbox) {
				Image checkBox = c.Find("button/checkbox").GetComponent<Image>();
				checkBox.enabled = (c.name == ("difficulty_panel_" + p_difficulty));
			}
		}
	}

	public void SetReady(bool isReady) {
		if (readyButton != null) {
			Text buttonText = readyButton.GetComponentInChildren<Text>();
			buttonText.text = "準備";

			if (isReady) {
				buttonText.text = "請站到安全準備區開始遊戲";
				AllButtonSwitcher(false);
				SendMessageToServer(EventFlag.NetMessageID.ReadyBTClick, "", clientRequester.guid);
			}
		}
	}

	public void ChangeFramePage(string p_frame_name) {
		Transform rootFrame = transform.Find("frame");
		foreach (Transform child in rootFrame)
		{			
			child.gameObject.SetActive(false);

			if (child.name == p_frame_name)
				child.gameObject.SetActive(true);
		}

        longPressButtons = GetComponentsInChildren<LongPressButton>();
    }

    public override void OnReceiveClientMsg(string p_event_id, string p_rawMsg) {
		switch (p_event_id)
		{
			case EventFlag.NetMessageID.Restart:{
				Init();
			}
			break;

			case EventFlag.NetMessageID.OnPositionReady:{
				
			}
			break;
		}
	}

	private void AllButtonSwitcher(bool isOn) {
		for (int i = 0; i < longPressButtons.Length; i++) {
			longPressButtons[i].enabled = isOn;
		}
	}

    private void OnApplicationQuit()
    {
        torchSerialPort.Disconnect();
    }
}
