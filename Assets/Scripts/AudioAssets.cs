using UnityEngine;
using System;
using System.Collections.Generic;



public class AudioAssets : MonoBehaviour {

	public SoundGroup[] soundGroups;

	Dictionary<string, AudioClip[]> soundGroupDictionary = new Dictionary<string, AudioClip[]> ();

	void Awake(){
		for (int i = 0; i < soundGroups.GetLength (0); i++) {
			soundGroupDictionary.Add (soundGroups[i].groupName, soundGroups[i].audioClips);
		}
	}

	public AudioClip GetClipFromName(string groupName){
		if (soundGroupDictionary.ContainsKey (groupName)) {
			AudioClip[] audioClips = soundGroupDictionary [groupName];
			return audioClips [UnityEngine.Random.Range (0, audioClips.GetLength (0))];
		}
		return null;
	}

	[System.Serializable]
	public class SoundGroup {
		public string groupName;
		public AudioClip[] audioClips;
	}
}
