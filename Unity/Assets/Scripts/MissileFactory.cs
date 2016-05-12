using UnityEngine;
using System.Collections;

public class MissileFactory : MonoBehaviour{
	public GameObject newMissileE;

	public GameObject NewMissile(Vector3 StartPos,Quaternion StartRot){
			GameObject newMissile = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissile.name = newMissile.name.Substring (0,7);
			newMissile.transform.transform.parent = GameObject.Find ("missiles").transform;
			newMissile.transform.localPosition = StartPos;
			newMissile.transform.localRotation = StartRot;
		return newMissile;
	}
		
	public GameObject NewMissileE{
		get{
			newMissileE = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissileE.name = newMissileE.name.Substring (0,7);
			return newMissileE;
		}
	}
}
