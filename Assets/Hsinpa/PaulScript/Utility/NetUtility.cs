using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Utility
{
	public class NetUtility {

		public static StringMessage BasicValueToStringMsg(string p_event_name, string p_value, string p_playerID) {
			SimpleJSON.JSONObject newJSON = new SimpleJSON.JSONObject();
			newJSON["_id"] = p_event_name;
			newJSON["player_id"] = p_playerID;
			newJSON["message"] = p_value;

			return new StringMessage(newJSON.ToString());
		}

	}	
}

