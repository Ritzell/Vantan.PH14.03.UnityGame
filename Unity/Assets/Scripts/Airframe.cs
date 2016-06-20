using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Airframe : MonoBehaviour
{
	private static int HP = 5;
	private static GameManager Manager;
	private LightingControlSystem LightingSystem;
	[SerializeField]
	private Image AlertUI;
	[SerializeField]
	private AudioSource AlertAudio;


	void Awake ()
	{
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		LightingSystem = FindObjectOfType<LightingControlSystem> ();
	}

	void Start ()
	{
//		NotificationSystem.Announce = "戦闘を開始します";
		StartCoroutine (NotificationSystem.UpdateNotification ("戦闘を開始します"));
		StartCoroutine (ObstacleWarning ());
	}

	public IEnumerator Reload (Vector3 StartPos, Quaternion StartRot)
	{
		yield return StartCoroutine (Manager.ReloadMissile (StartPos, StartRot));
		LightingSystem.TurningOn (UIType.Missile);
	}

	private void OnTriggerEnter (Collider Col)
	{
		Bombed ();
		DiedJudgment (Col.gameObject);
	}

	private void Bombed(){
		//HP -= 1;
		LightingSystem.TurningOff (UIType.HP);
		StartCoroutine (CameraSystem.SwayCamera ());
		PlayerSound.HitSound ();
	}

	private void DiedJudgment(GameObject Col){
		if (HP <= 0 || Col.layer == 10 || Col.layer == 15 || Col.layer == 11) {
			LightingControlSystem.ShatDown ();
			Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
			StartCoroutine (Deth ());
		}
	}

	private IEnumerator Alert ()
	{
		int Sign = 1;
		AlertAudio.Play ();
		while (true) {
			AlertUI.color = new Color (1, 1, 1, AlertUI.color.a + (1.5f * Time.deltaTime * Sign));
			if ((AlertUI.color.a > 0.9f && Sign == 1) || (AlertUI.color.a < 0.1f && Sign == -1)) {
				Sign *= -1;
			}
			yield return null;

		}
	}

	/// <summary>
	/// 障害物の接近を検知、報告する
	/// </summary>
	/// <returns>The warning.</returns>
	private static Coroutine alert;

	private IEnumerator ObstacleWarning ()
	{
		RaycastHit hit;
		var isAlert = false;
		while (true) {
			Ray ray = new Ray (transform.position, transform.forward);
			if (!isAlert && Physics.Raycast (ray, out hit, 1500, 1 << 10)) {
				alert = StartCoroutine (Alert ());
				isAlert = true;
				yield return null;
			} else if (isAlert && !Physics.Raycast (ray, out hit, 1000, 1 << 10)) {
				AlertAudio.Stop ();
				AlertUI.color = new Color (1, 1, 1, 0);
				StopCoroutine (alert);
				isAlert = false;
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator Deth ()
	{
		if (GameManager.GameOver) {
			yield break;
		}
		Destroy (gameObject);
		yield return StartCoroutine (GameManager.GameEnd (false));
	}
}