using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	void Start () {
		EnemyBase.restChildren++;
	}
	void OnTriggerEnter(Collider col){
		EnemyBase.restChildren--;
		Destroy (gameObject);
	}
}
