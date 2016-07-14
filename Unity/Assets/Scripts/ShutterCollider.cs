using UnityEngine;
using System.Collections;

public class ShutterCollider : MonoBehaviour {
	[SerializeField]
	private GameObject player;
	void OnTriggerExit(Collider col){
		if(GameManager.IsGameOver || Airframe.isAlert || !Airframe.isLife){
			return;
		}
		FindObjectOfType<ImageCamera> ().StartCoroutine (ImageCamera.CaptureResultImage (player.transform.position,col.gameObject.transform.position));
	}
}
