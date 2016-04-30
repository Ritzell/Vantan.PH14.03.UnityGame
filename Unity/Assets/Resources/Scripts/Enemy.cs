using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	public static GameObject tgt;

	void Start () {
		EnemyBase.restChildren++;
		tgt = GameObject.Find ("eurofighter");
	}
	void OnTriggerStay(Collider col){
		EnemyBase.restChildren--;
		Destroy (gameObject);
	}
}
