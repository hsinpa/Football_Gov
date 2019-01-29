using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFlag  {

	public class NetMessageID
	{
		public const string Restart = "net_id.restart";
		public const string SelectBackground = "net_id.select_background";
		public const string SelectDifficulty = "net_id.select_difficulty";
		public const string ReadyBTClick = "net_id.on_click_ready";

		public const string OnPositionReady = "net_id.on_position_ready";
		public const string GameOver = "net_id.gameover";

		public const short JSONMessageID = 1439;
	}

}
