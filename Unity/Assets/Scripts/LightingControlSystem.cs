using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LightingControlSystem : MonoBehaviour
{
	private static List<Image> MissileIcons = new List<Image> ();
	private static Queue<Image> HPBarIcons = new Queue<Image> ();
	private static Image GunHitImage;
	private static RectTransform GunHitImageTransform;
	private static int _missileNumber = -1;
	private static int _missileOffNumber = -1;

//	private static Coroutine _rotateGunHit;
//	public static Coroutine RotateGunHit{
//		set{
//			_rotateGunHit = value;
//		}get{
//			return _rotateGunHit;
//		}
//	}

	// Use this for initialization
	void Awake ()
	{
		MissileIcons.Add (GameObject.Find ("MissileIconA").GetComponent<Image> ());
		MissileIcons.Add (GameObject.Find ("MissileIconB").GetComponent<Image> ());
		MissileIcons.Add (GameObject.Find ("MissileIconC").GetComponent<Image> ());
		MissileIcons.Add (GameObject.Find ("MissileIconD").GetComponent<Image> ());

		HPBarIcons.Enqueue (GameObject.Find ("HPBarA").GetComponent<Image> ());
		HPBarIcons.Enqueue (GameObject.Find ("HPBarB").GetComponent<Image> ());
		HPBarIcons.Enqueue (GameObject.Find ("HPBarC").GetComponent<Image> ());
		HPBarIcons.Enqueue (GameObject.Find ("HPBarD").GetComponent<Image> ());
		HPBarIcons.Enqueue (GameObject.Find ("HPBarE").GetComponent<Image> ());
		GunHitImage = GameObject.Find ("GunHitImage").GetComponent<Image> ();
		GunHitImageTransform = GunHitImage.GetComponent<RectTransform>();
	}

	public void TurningOff (UIType UI)
	{
		if (UI == UIType.Missile) {
			MissileIcons [Number (ref _missileNumber)].color = Color.gray;
		} else if (UI == UIType.HP) {
			HPBarIcons.Dequeue ().color = Color.black;
			if (HPBarIcons.Count == 2) {
				foreach (Image HPBar in HPBarIcons) {
					HPBar.color = Color.red;
				}
			}
		} else if (UI == UIType.GunHit) {
			GunHitImage.StartCoroutine (TurningOnGunHit ());
		}
	}

	//	private IEnumerator test (){
	//		while (true) {
	//			GunHitImage.StartCoroutine (TurningOnGunHit ());
	//			yield return null;
	//		}
	//	}
	public static IEnumerator TurningOnGunHit ()
	{
		GunHitImageTransform.Rotate (0, 0, -2f);
		GunHitImage.color = Color.red;
		yield return new WaitForSeconds (0.25f);
		GunHitImage.color = Color.green;
		yield return null;
		while (GunHitImageTransform.localEulerAngles.z > 3 && GunHitImageTransform.localEulerAngles.z < 357) {
			GunHitImageTransform.Rotate (0, 0, 4f);
			yield return null;
		}
		GunHitImageTransform.rotation = new Quaternion (0, 0, 0, 0);
		yield return null;
	}

	public static void ShatDown ()
	{
		while (HPBarIcons.Count > 0) {
			HPBarIcons.Dequeue ().color = Color.black;
		}
	}

	public void TurningOn (UIType UI)
	{
		if (UI == UIType.Missile) {
			MissileIcons [Number (ref _missileOffNumber)].color = Color.white;
		}
	}

	private int Number (ref int number)
	{
		number++;
		if (number >= MissileIcons.Count) {
			number = 0;
		}
		return number;
	}

	void OnDestroy(){
		MissileIcons.Clear ();
		HPBarIcons.Clear ();
	}
}
