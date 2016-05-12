using UnityEngine;
using System.Collections;

public class BulletFactory : MonoBehaviour {
	
	private GameObject newBullet;

	public GameObject NewBullet{
		get{
			return newBullet;
		}
	}

	public IEnumerator MakeBullet(Transform RootOb,Vector3 Pos,Quaternion Rot){
		newBullet = (GameObject)Instantiate (Resources.Load("prefabs/bullet"),Pos,Rot);
		newBullet.name = newBullet.name.Substring (0,6);
		newBullet.transform.parent = RootOb;
		yield return null;
	}
}
