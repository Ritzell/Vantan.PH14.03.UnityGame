using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstimationSystem : MonoBehaviour
{
	private static List<GameObject> missiles = new List<GameObject> ();
	public static GameObject Missiles{
		set{
			missiles.Add (value);
		}
	}
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (CountMissile ());
	}

	private IEnumerator CountMissile ()
	{
		float time = 0f;
		while (!GameManager.IsGameOver) {
			time += Time.deltaTime;
			if (time >= 1) {
				//StartCoroutine (SiftMissile ());
			}
			yield return null;
		}

	}

	private IEnumerator SiftMissile ()
	{
		const float MaxDistance = 500;
		foreach (GameObject Missile in missiles) {
			Vector3 PlayerPos = gameObject.transform.position;
			Vector3 MissilePos = Missile.transform.position;
			if (Mathf.Abs (Vector3.Distance (PlayerPos, MissilePos)) > MaxDistance) {
				missiles.Remove (Missile);
			}
			EstimationLine ();
		}
		yield return null;
	}

	private void EstimationLine(){
		LineRenderer LineRender = gameObject.GetComponent<LineRenderer> ();
		LineRender.SetVertexCount (missiles.Count*2);
		int LineNomber = 0;
		foreach(GameObject Missile in missiles){
			Vector3 MissilePos = Missile.transform.position;
			LineRender.SetPosition (LineNomber,MissilePos);
			LineRender.SetPosition (LineNomber+1,new Vector3(MissilePos.x,MissilePos.y,Missile.transform.localPosition.z+2500));
			LineNomber += 2;
		}
	}

	public static void RemoveList(GameObject Missile){
		missiles.Remove (Missile);
	}
}