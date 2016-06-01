using UnityEngine;
using System.Collections;

public class MapMissilePosition : MonoBehaviour
{

	private Transform Player;
	private Transform Rader;

	void Awake(){
		Player = GameObject.Find ("AirPlain").transform;
		Rader = GameObject.Find ("Map").transform;
		transform.parent = Rader;//GameObject.Find ("Map").transform;
	}

	public IEnumerator UpdatePosition (Transform Missile)
	{
		name = Missile.name + "Point";

		while (!GameManager.GameOver) {
			transform.position = new Vector3 (((Missile.position.x - Player.position.x) * 0.025f) + Rader.position.x/*/Screen.width)*Rader.position.x)+Rader.position.x*/, ((Missile.position.y - Player.position.y) * 0.025f) + Rader.position.x, 0);
			yield return null;
		}
	}
}
