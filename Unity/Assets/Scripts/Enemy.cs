using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	private static GameObject tgt;
	public static GameObject Tgt {
		set {
			tgt = value;
		}
		get {
			return tgt;
		}
	}

	private static ReticleSystem PlayerReticle;

	void Awake(){
		PlayerReticle = GameObject.Find("ReticleImage").GetComponent<ReticleSystem>();
		Tgt = GameObject.Find ("eurofighter");
	}

	void Start ()
	{
		EnemyBase.Rest = EnemyBase.Rest + 1;
	}

	void OnTriggerStay (Collider Col)
	{
		Debug.Log (Col);
		EnemyBase.Rest = EnemyBase.Rest - 1;
		PlayerReticle.DestoroyLockOnTgt (gameObject);
		Destroy (gameObject);
	}
}
