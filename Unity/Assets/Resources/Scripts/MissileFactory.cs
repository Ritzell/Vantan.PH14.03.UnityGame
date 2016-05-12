using UnityEngine;
using System.Collections;

public class MissileFactory : MonoBehaviour{
	public GameObject newMissile;
	public GameObject newMissileE;
	public GameObject NewMissile{
		get{
			newMissile = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissile.name = newMissile.name.Substring (0,7);
			return newMissile;
		}
	}
	public GameObject NewMissileE{
		get{
			newMissileE = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissileE.name = newMissileE.name.Substring (0,7);
			return newMissileE;
		}
	}
}
