using UnityEngine;
using System.Collections;

public class MissileFactory : MonoBehaviour
{
	public GameObject NewMissile (Vector3 StartPos, Quaternion StartRot)
	{
		GameObject newMissile = (GameObject)Instantiate (Resources.Load ("prefabs/missile"), Vector3.zero, Quaternion.identity);
		newMissile.name = newMissile.name.Substring (0, 7);
		newMissile.transform.transform.parent = GameObject.Find ("missiles").transform;
		newMissile.transform.localPosition = StartPos;
		newMissile.transform.localRotation = StartRot;
		return newMissile;
	}

	public GameObject NewMissileE (Vector3 Pos)
	{
		GameObject newMissileE = (GameObject)Instantiate (Resources.Load ("prefabs/missile"), Vector3.zero, Quaternion.identity);
		newMissileE.name = newMissileE.name.Substring (0, 7);
		newMissileE.transform.position = Pos;
		newMissileE.layer = 12;
		newMissileE.transform.FindChild ("Steam").gameObject.SetActive(true);
		newMissileE.transform.FindChild ("Afterburner").gameObject.SetActive(true);
		//newMissileE.transform.localScale = new Vector3 (100, 100, 100);
		return newMissileE;
	}
}
