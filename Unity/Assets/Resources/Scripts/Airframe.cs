using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour {

	public void reload(Vector3 startPos,Quaternion startRot){
		StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
	}

	private void OnTriggerEnter(Collider col){
		deth ();
	}

	private void deth(){
		GameManager.loadScene();
		PlayerSound.audioPlay ();
		Destroy (gameObject);
	}
}
