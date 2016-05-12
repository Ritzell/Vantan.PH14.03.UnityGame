using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {
	public static byte RestChildren = 0;

	void Start () {
	
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("win");
		Destroy(RestChildren <= 0 ? gameObject : null);
	}

	void OnDestroy(){
		GameManager.loadScene ();
	}
}
