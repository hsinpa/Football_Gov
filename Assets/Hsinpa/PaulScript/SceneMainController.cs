using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Utility;
using System.Net;
using System.Net.Sockets;

public abstract class SceneMainController : MonoBehaviour {
	
#region Inspector
	[SerializeField]
	private GameObject networkManagerPrefab;

	[SerializeField]
	private bool is_server;

	protected NetworkManager networkManager; 
	
	private string NetworkName = "Network";

	protected NetworkClient client;
	protected ClientRequester  clientRequester;

	protected string selfIP;
#endregion

	protected virtual void Start () {
		selfIP = LocalIPAddress();

		if (!IsNetworkInit()) {
			GameObject networkObject = Instantiate(networkManagerPrefab, Vector3.zero, Quaternion.identity);
			networkObject.name = NetworkName;
			networkManager = networkObject.GetComponent<NetworkManager>();
		} else {
			networkManager = GameObject.Find(NetworkName).GetComponent<NetworkManager>();
		}


		//As server
		if (is_server) {
			ConnectAsClient("localhost");
		} else if (!string.IsNullOrEmpty(networkManager.networkAddress)) {
			//For client reconnect
			ConnectAsClient(networkManager.networkAddress);
		}

		//Init();
	}
	
	protected virtual void Init() {
		Debug.Log("Game Start");
	}

	protected void SendMessageToServer(string p_event, string p_msg, string p_guid ) {
		var msg = NetUtility.BasicValueToStringMsg(p_event, p_msg, p_guid);
		this.client.Send(EventFlag.NetMessageID.JSONMessageID, msg);
	}

	private bool IsNetworkInit() {
		return GameObject.Find(NetworkName) != null;
	}

	public virtual void OnReceiveClientMsg(string p_event_id, string p_rawMsg) {

	}

	protected void ConnectAsClient(string p_ip_address) {
		Debug.Log("IP " + p_ip_address);
		if (networkManager != null) {
			networkManager.networkAddress = p_ip_address;
			clientRequester = new ClientRequester(networkManager, OnReceiveClientMsg);
			clientRequester.ConnectAsClient(networkManager.networkPort, (NetworkClient p_client) => {
				this.client = p_client;
				Init();
			});
		}
	}

	 private string LocalIPAddress()
     {
         IPHostEntry host;
         string localIP = "";
         host = Dns.GetHostEntry(Dns.GetHostName());
         foreach (IPAddress ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 localIP = ip.ToString();
                 break;
             }
         }
         return localIP;
     }
}
