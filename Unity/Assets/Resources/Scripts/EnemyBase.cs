using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {
	public static byte restChildren = 0;

	void Start () {
	
	}

	void OnTriggerStay(Collider col){
		Destroy(restChildren <= 0 ? gameObject : null);
	}
}
