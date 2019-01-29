using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using SimpleJSON;

public class ClientRequester {
	private System.Action<NetworkClient> _onComplete;
	private System.Action<string, string> _onReceiveMessage;

	private NetworkClient _client;
	private NetworkManager _network;
	private int _port;

	public string guid;

	public ClientRequester(NetworkManager network, System.Action<string, string> p_OnReceiveMessage) {
		this._network = network;
		guid = System.Guid.NewGuid().ToString();
		_onReceiveMessage = p_OnReceiveMessage;
	}

	private void SetUpServer() {
		NetworkServer.UnregisterHandler(EventFlag.NetMessageID.JSONMessageID);
		NetworkServer.RegisterHandler(EventFlag.NetMessageID.JSONMessageID, delegate(NetworkMessage msg) {
			NetworkServer.SendToAll(EventFlag.NetMessageID.JSONMessageID, msg.ReadMessage<StringMessage>());
		});

		this._network.StartServer();
	}

	private void SetUpClient(int p_port) {
		if (_client == null)
			_client = new NetworkClient();

		RegisterEvent(_client, MsgType.Connect, OnConnected);
		RegisterEvent(_client, MsgType.Disconnect, OnError);
		RegisterEvent(_client, EventFlag.NetMessageID.JSONMessageID, OnReceiveMessage);
        _client.Connect(_network.networkAddress, p_port);
	}

	public void ConnectAsClient(int p_port, System.Action<NetworkClient> OnComplete) {
		_onComplete = OnComplete;
		_port = p_port;

		SetUpClient(_port);
	}
	
	private void OnConnected(NetworkMessage netMsg)
    {
		if (_onComplete != null && _client != null) {
			_onComplete(_client);
		}
    }

	private void OnError(NetworkMessage netMsg)
    {
		SetUpServer();
		SetUpClient(_port);
    }

	private void RegisterEvent(NetworkClient p_client , short msg_id, NetworkMessageDelegate p_callback) {
		if (_client == null) return;
		_client.UnregisterHandler(msg_id);
		_client.RegisterHandler(msg_id, p_callback);     
	}

    private void OnReceiveMessage(NetworkMessage netMsg)
    {
        var rawMessage = netMsg.ReadMessage<StringMessage>();

		try {
			var rawJSON = JSON.Parse(rawMessage.value);
			string _id = rawJSON["_id"].Value;
			string message = rawJSON["message"].Value;

			if (_onReceiveMessage != null && guid != rawJSON["player_id"].Value) {
				Debug.Log(rawMessage.value);
				_onReceiveMessage(_id, message);
			}

		} catch(System.Exception exp) {
			Debug.Log("ParseError " + exp.ToString());
		}
    }
}
