using UnityEngine;
using System.Collections;

public class BulletFactory : MonoBehaviour
{
	
	public void MakeBullet (Transform RootOb, Vector3 Pos, Quaternion Rot)
	{
		GameObject newBullet = (GameObject)Instantiate (Resources.Load ("prefabs/bullet"), Pos, Rot);
		newBullet.name = newBullet.name.Substring (0, 6);
		newBullet.transform.parent = RootOb;
	}
}
