using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	[CreateAssetMenu (menuName = "Audio/Array")]
	public class AudioArray : ScriptableObject {		
		
		[SerializeField]
		public List<AudioSet> audio_combination;

		[System.Serializable]
		public class AudioSet {
			public AudioClip audio;
			public string _id;
		}
	}
}
