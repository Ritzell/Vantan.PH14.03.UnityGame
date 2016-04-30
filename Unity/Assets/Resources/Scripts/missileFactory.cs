using UnityEngine;
using System.Collections;

public class missileFactory : MonoBehaviour{
	public GameObject newMissile;
	public GameObject NewMissile{
		get{
			newMissile = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissile.name = newMissile.name.Substring (0,7);
			return newMissile;
		}
	}
}
