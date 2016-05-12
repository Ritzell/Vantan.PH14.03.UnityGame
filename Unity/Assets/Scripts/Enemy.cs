using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	public static GameObject Tgt;

	void Start () {
		EnemyBase.RestChildren++;
		Tgt = GameObject.Find ("eurofighter");
	}
	void OnTriggerStay(Collider Col){
		EnemyBase.RestChildren--;
		Destroy (gameObject);
	}
}
