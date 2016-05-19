using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileRader : MonoBehaviour {
	private static List<Transform> addOutRangeMissile = new List<Transform> ();
	public static List<Transform> AddOutRangeMissile{
		get{
			return addOutRangeMissile;
		}
	}
	private static List<Transform> addInRangeMissile = new List<Transform>();
	public static List<Transform> AddInRangeMissile {
		get{
			return addInRangeMissile;
		}
	}
	private static List<Transform> outRangeMissiles = new List<Transform> ();
	public static List<Transform> OutRangeMissiles{
		get{
			return outRangeMissiles;
		}
	}
	private static List<Transform> InRangeMissile = new List<Transform>();

	[SerializeField]
	private Transform Player;
	[SerializeField]
	private GameObject PointOb;
	private static MissileRader Rader;

	void Start () {
		Rader = gameObject.GetComponent<MissileRader>();
		StartCoroutine (OutRaderMissileDistance());
		StartCoroutine (RotateRader());
	}

	private IEnumerator RotateRader(){
		while(!GameManager.GameOver){
			transform.rotation = new Quaternion (0,0,Player.localRotation.y,transform.rotation.w);
			yield return null;
		}
	}

	private IEnumerator OutRaderMissileDistance(){
		while (!GameManager.GameOver) {
			addOutRangeMissile.ForEach (addMissile => outRangeMissiles.Add(addMissile));
			addOutRangeMissile.Clear ();
			for(int i = 0; i < outRangeMissiles.Count; i++){
				if (Mathf.Abs (Vector3.Distance (outRangeMissiles[i].position, Player.position)) <= 2000) {
					Debug.Log (outRangeMissiles[i]);
					GetInRange (outRangeMissiles [i]);
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
	}
	private IEnumerator InRaderMissileDistance(){
		while(!GameManager.GameOver){
			addInRangeMissile.ForEach (addMissile => InRangeMissile.Add(addMissile));
			addInRangeMissile.Clear ();
			for(int i = 0; i < InRangeMissile.Count; i++){
				if (Mathf.Abs (Vector3.Distance (InRangeMissile[i].position, Player.position)) > 2000) {
					Debug.Log (outRangeMissiles[i]);
					GetOutRange(InRangeMissile[i]);
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
	}

	private  void GetInRange(Transform Missile){
		addInRangeMissile.Add (Missile);
		NewMissilePointUI (Missile);
		outRangeMissiles.Remove (Missile);
	}

	private  void GetOutRange(Transform Missile){
		addOutRangeMissile.Add (Missile);
		//Debug.Log (GameObject.Find(Missile.name + "Point").gameObject);
		Destroy (GameObject.Find(Missile.name + "Point").gameObject);
		InRangeMissile.Remove (Missile);
	}
		
	public static void DestroyMissile(Transform Missile){
		if(GameObject.Find(Missile.name + "Point").gameObject){
		Destroy (GameObject.Find(Missile.name + "Point").gameObject);
		InRangeMissile.Remove (Missile);
		}
	}

	private  void NewMissilePointUI(Transform Missile){
		GameObject MissileUI = Instantiate (PointOb);
		MissileUI.GetComponent<MapMissilePosition> ().StartCoroutine (MissileUI.GetComponent<MapMissilePosition> ().UpdatePosition (Missile));
	}
}
