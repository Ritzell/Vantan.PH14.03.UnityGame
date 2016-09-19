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

	private static List<Transform> outRangeMissiles = new List<Transform> ();
	public static List<Transform> OutRangeMissiles {
		get {
			return outRangeMissiles;
		}
	}
	private static List<Transform> _inRangeMissiles = new List<Transform>();
	public static List<Transform> InRangeMissiles{
		get{
			return _inRangeMissiles;
		}
	}

	private GameManager Manager;

	[SerializeField]
	private GameObject PointOb;

	void Start () {
		Manager = FindObjectOfType<GameManager> ();
		StartCoroutine (OutRaderMissileDistance());
		StartCoroutine (InRaderMissileDistance ());
		StartCoroutine (RotateRader());
	}

	private IEnumerator RotateRader(){
		while(!GameManager.IsGameOver && !VRMode.isVRMode){
			transform.rotation = new Quaternion (0,0,Airframe.AirFrame.transform.localRotation.y,transform.rotation.w);
			yield return null;
		}
	}

	private IEnumerator OutRaderMissileDistance(){
		while (!GameManager.IsGameOver) {
			MissileAddList(true);
			for(int i = 0; i <= outRangeMissiles.Count-1; i++){
                try
                {
                    if (Airframe.isLife && Manager.AbsDistance(outRangeMissiles[i].position, Airframe.AirFramePosition) <= 2000)
                    {
                        ToInRange(outRangeMissiles[i]);
                    }
                }
                catch
                {
                    continue;
                }
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator InRaderMissileDistance(){
		Vector2 RaderPos = new Vector2 (transform.position.x, transform.position.y);
		GameManager Manager = FindObjectOfType<GameManager> ();
		while(!GameManager.IsGameOver){
			MissileAddList(false);
			for(int i = 0; i <= _inRangeMissiles.Count-1; i++){
				float distance;
				try{
				distance  = Manager.AbsDistance (new Vector2 (_inRangeMissiles [i].position.x, _inRangeMissiles [i].position.y), RaderPos);
				}catch{
					continue;
				}
				if (distance > RaderPos.x) {
					ToOutRange (_inRangeMissiles [i]);
				}
				yield return null;
			}
			yield return null;
		}
	}

	private void MissileAddList(bool isOutRange){
		if (isOutRange) {
			addOutRangeMissile.ForEach (addMissile => outRangeMissiles.Add (addMissile));
			addOutRangeMissile.Clear ();
		} else {
			addInRangeMissile.ForEach (addMissile => _inRangeMissiles.Add(addMissile));
			addInRangeMissile.Clear ();
		}
	}

	private  void ToInRange(Transform Missile){
		addInRangeMissile.Add (NewMissilePointUI (Missile).transform);
		outRangeMissiles.Remove (Missile);
	}

	private  void ToOutRange(Transform MissilePoint){
		addOutRangeMissile.Add (MissilePoint.GetComponent<MapMissilePosition>().Tgt);
		Destroy (GameObject.Find(MissilePoint.name));
		_inRangeMissiles.Remove (MissilePoint);
	}
		
	public static void DestroyMissile(Transform Missile){
		GameObject MissilePoint = GameObject.Find (Missile.name + "Point");
		try{
		if(OutRangeMissiles.Contains(Missile)){
			OutRangeMissiles.Remove (Missile);
		}else if(_inRangeMissiles.Contains(MissilePoint.transform)){
				_inRangeMissiles.Remove (MissilePoint.transform);
		}
		}catch{
		}
		Destroy (MissilePoint);
	}

	private  GameObject NewMissilePointUI(Transform Missile){
		GameObject MissileUI = Instantiate (PointOb);
		MissileUI.GetComponent<MapMissilePosition> ().StartCoroutine (MissileUI.GetComponent<MapMissilePosition> ().UpdatePosition (Missile));
		return MissileUI;
	}

	void OnDestroy(){
		ListsClear ();
		StopAllCoroutines ();
	}

	private void ListsClear(){
		addInRangeMissile.Clear ();
		InRangeMissiles.Clear ();
		OutRangeMissiles.Clear ();
		AddOutRangeMissile.Clear ();
	}
}
