using UnityEngine;
using System.Collections;

public class bulletFactory : MonoBehaviour {
	public GameObject NewBullet;

	public IEnumerator MakeBullet(Transform RootOb,Vector3 Pos,Quaternion Rot){
		NewBullet = (GameObject)Instantiate (Resources.Load("prefabs/bullet"),Pos,Rot);
		NewBullet.name = NewBullet.name.Substring (0,6);
		NewBullet.transform.parent = RootOb;
		yield return null;
	}
}
