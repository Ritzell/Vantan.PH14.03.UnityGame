using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	private static GameObject tgt;

	public static GameObject Tgt{
		set{
			tgt = value;
		}get{
			return tgt;
		}
	}

	void Start () {
		EnemyBase.Rest++;
		Tgt = GameObject.Find ("eurofighter");
	}
	void OnTriggerStay(Collider Col){
		EnemyBase.Rest--;
		Destroy (gameObject);
	}
}
