using UnityEngine;
using System.Collections;

public class MapMissilePosition : MonoBehaviour {
	public IEnumerator UpdatePosition(Transform Missile){
		Transform Player = GameObject.Find ("AirPlain").transform;
		Transform Rader = GameObject.Find ("Map").transform;
		transform.parent = Rader;//GameObject.Find ("Map").transform;
		name = Missile.name + "Point";
		while (!GameManager.GameOver) {
			transform.position = new Vector3 (((Missile.position.x - Player.position.x)*0.025f)+Rader.position.x/*/Screen.width)*Rader.position.x)+Rader.position.x*/,((Missile.position.y - Player.position.y)*0.025f)+Rader.position.x,0);
			yield return null;
		}
	}
}
