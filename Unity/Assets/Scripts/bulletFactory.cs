using UnityEngine;
using System.Collections;

public class BulletFactory : MonoBehaviour
{
	private GameObject _bullet;

	public GameObject Bullet {
		get {
            if (_bullet == null)
            {
                _bullet = (GameObject)Resources.Load("prefabs/bullet");
            }
			return _bullet;
		}
	}

	public GameObject MakeBullet (Transform RootOb, Vector3 Pos, Quaternion Rot)
	{
		var newBullet = (GameObject)Instantiate(Bullet, Pos, Rot);
		newBullet.name = newBullet.name.Substring (0, 6);
		newBullet.transform.parent = RootOb;
        return newBullet;
	}
}
