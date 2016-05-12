using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour {

	public void reload(Vector3 startPos,Quaternion startRot){
		StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
	}

	private void OnTriggerEnter(Collider col){
		Deth ();
	}

	private void Deth(){
		GameManager.loadScene();
		PlayerSound.AudioPlay ();
		Destroy (gameObject);
	}
}
