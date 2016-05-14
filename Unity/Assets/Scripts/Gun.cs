using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
	private static GameObject[] Muzzules = new GameObject[2];
	private static BulletFactory Factory;
	// Use this for initialization

	void Start ()
	{
		Factory = GameObject.Find ("GameManager").GetComponent<BulletFactory> ();
		Muzzules [0] = GameObject.Find ("muzzleA");
		Muzzules [1] = GameObject.Find ("muzzleB");
	}

	public IEnumerator Shoot ()
	{
		foreach (GameObject ob in Muzzules) {
			Muzzle MuzzleScript = ob.GetComponent<Muzzle> ();
			StartCoroutine (MuzzleScript.Ignition ());
			StartCoroutine (Factory.MakeBullet (ob.transform, ob.transform.position, ob.transform.rotation));
			yield return null;
		}
		yield return null;
	}

	public static IEnumerator MuzzuleLookTgt (Vector3 Tgt)
	{
		try{
		foreach (GameObject Muzzule in Muzzules) {
			Muzzule.transform.LookAt (Tgt);
		}
		}catch{
			Debug.Log ("マズルの方向転換でエラーが出てます。");
		}
		yield return null;
	}
}
