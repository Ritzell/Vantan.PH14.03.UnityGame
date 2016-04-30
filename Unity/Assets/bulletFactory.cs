using UnityEngine;
using System.Collections;

public class bulletFactory : MonoBehaviour {
	public GameObject newbullet;

	public IEnumerator newBullet(Transform rootOB,Vector3 pos,Quaternion rot){
		newbullet = (GameObject)Instantiate (Resources.Load("prefabs/bullet"),pos,rot);
		newbullet.name = newbullet.name.Substring (0,6);
		newbullet.transform.parent = rootOB;
		yield return null;
	}
}
