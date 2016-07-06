using UnityEngine;
using System.Collections;

public class MapMissilePosition : MonoBehaviour
{

	private Transform Player;
	private Transform Rader;

	private Transform _Tgt;
	public Transform Tgt {
		get {
			return _Tgt;
		}
	}


	void Awake(){
		Player = GameObject.Find ("AirPlain").transform;
		Rader = GameObject.Find ("Map").transform;
		transform.SetParent(Rader.transform,false);//GameObject.Find ("Map").transform;
	}

	public IEnumerator UpdatePosition (Transform Missile)
	{
		name = Missile.name + "Point";
		_Tgt = Missile;
		while (!GameManager.GameOver) {
			try{
			transform.position = new Vector3 (((Missile.position.x - Player.position.x) * 0.025f) + Rader.position.x, ((Missile.position.y - Player.position.y) * 0.025f) + Rader.position.x, 0);
			}catch{
				Destroy (gameObject);
			}
			yield return null;
		}
	}
}
