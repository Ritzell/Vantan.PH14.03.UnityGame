using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
	private static Muzzle[] Muzzles = new Muzzle[2];
	private static BulletFactory Factory;

	void Awake ()
	{
		Factory = GameObject.Find ("GameManager").GetComponent<BulletFactory> ();
		Muzzles [0] = GameObject.Find ("muzzleA").GetComponent<Muzzle>();
		Muzzles [1] = GameObject.Find ("muzzleB").GetComponent<Muzzle>();
	}

	public IEnumerator Shoot ()
	{
		foreach (Muzzle muzzle in Muzzles) {
			yield return StartCoroutine(Factory.MakeBullet (muzzle.transform, muzzle.transform.position, muzzle.transform.rotation));
			StartCoroutine (muzzle.Ignition ());
			yield return null;
		}
	}

	public static IEnumerator MuzzuleLookTgt (Vector3 Tgt)
	{
		try{
			foreach (Muzzle muzzle in Muzzles) {
			muzzle.transform.LookAt (Tgt);
		}
		}catch{
			//Debug.Log ("マズルの方向転換でエラーが出てます。");
		}
		yield return null;
	}
}