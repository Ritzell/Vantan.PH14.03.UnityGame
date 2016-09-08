using UnityEngine;
using System.Collections;

public class AudioSetting : MonoBehaviour {
	
	public void OnValueChanged(float value){
		Debug.Log (value);
		AudioListener.volume = value;
	}
}
