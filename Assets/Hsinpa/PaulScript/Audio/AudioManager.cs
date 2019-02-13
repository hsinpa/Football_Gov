using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	public class AudioManager : MonoBehaviour {
		[SerializeField]
		private AudioArray audioArray;

        public static AudioManager instance = null;     //Allows other scripts to call functions from SoundManager.             
        void Awake ()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy (gameObject);
        }


		public void SetUp() {

		}

		public void PlayAudio(GameObject p_audio_sourceObject, string p_audio_id, float p_volume) {
			AudioSource audioSource = p_audio_sourceObject.GetComponent<AudioSource>();
			if (audioSource == null)
				audioSource = p_audio_sourceObject.AddComponent<AudioSource>();

			if (audioSource.isPlaying) {
				if (audioSource.time < 0.25f)
					return;
			}

			AudioClip p_findClip = SearchClipByID(p_audio_id);
			audioSource.Stop();
			audioSource.clip = p_findClip;
			audioSource.volume = p_volume;
			audioSource.loop = false;

			audioSource.Play();
		}

		private AudioClip SearchClipByID(string p_clip_id) {
			if (audioArray == null) return null;
			foreach(AudioArray.AudioSet set in audioArray.audio_combination) {
				if (set._id == p_clip_id) return set.audio;
			}
			return null;
		}

	}
}
