using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour {
	
	void OnTriggerEnter(Collider col){
		deth ();
	}

	private void deth(){
		//StartCoroutine(GameManager.loadScene ());
		GameManager.loadScene();
		PlayerSound.audioPlay ();
		Destroy (gameObject);
	}
}
