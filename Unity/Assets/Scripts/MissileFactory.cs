using UnityEngine;
using System.Collections;

public class MissileFactory : MonoBehaviour
{
	private static int Numbering = 0;

	public GameObject NewMissile (Vector3 StartPos, Quaternion StartRot)
	{
		GameObject newMissile = (GameObject)Instantiate (Resources.Load ("prefabs/missile"), Vector3.zero, Quaternion.identity);
		newMissile.name = newMissile.name.Substring (0, 7);
		newMissile.transform.transform.parent = GameObject.Find ("missiles").transform;
		newMissile.transform.localPosition = StartPos;
		newMissile.transform.localRotation = StartRot;
		return newMissile;
	}

	/// <summary>
	/// 適用のミサイルを生成、引数は初期位置
	/// </summary>
	/// <returns>The missile e.</returns>
	/// <param name="Pos">Position.</param>
	public GameObject NewMissileE (Vector3 Pos)
	{
		Numbering++;
		GameObject newMissileE = (GameObject)Instantiate (Resources.Load ("prefabs/missile"), Pos, Quaternion.identity);
		newMissileE.name = newMissileE.name.Substring (0, 7)+Numbering;
		newMissileE.layer = 12;
		newMissileE.transform.FindChild ("Steam").gameObject.SetActive(true);
		newMissileE.transform.FindChild ("Afterburner").gameObject.SetActive(true);
		return newMissileE;
	}

	public GameObject NewMissileE (Vector3 Pos,Vector3 Rot)
	{
		Numbering++;
		GameObject newMissileE = (GameObject)Instantiate (Resources.Load ("prefabs/missile"), Pos, Quaternion.identity);
		newMissileE.transform.Rotate(Rot);
		newMissileE.name = newMissileE.name.Substring (0, 7)+Numbering;
		newMissileE.layer = 12;
		newMissileE.transform.FindChild ("Steam").gameObject.SetActive(true);
		newMissileE.transform.FindChild ("Afterburner").gameObject.SetActive(true);
		return newMissileE;
	}
}
