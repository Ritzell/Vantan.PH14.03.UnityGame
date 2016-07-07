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
		HP -= 1;
		LightingSystem.TurningOff (UIType.HP);
		StartCoroutine (CameraSystem.SwayCamera ());
		PlayerSound.HitSound ();
	}

	private void DiedJudgment(GameObject Col){
		if (HP <= 0 || Col.layer == 10 || Col.layer == (int)Layers.EnemyArmor || Col.layer == (int)Layers.Enemy) {
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
	private static Coroutine AlertCotouine;

	private static bool _isAlert = false;
	public static bool isAlert{
		get{
			return _isAlert;
		}
	}
	private IEnumerator ObstacleWarning ()
	{
		RaycastHit hit;
		while (true) {
			Ray ray = new Ray (transform.position, transform.forward);
			if (!_isAlert && Physics.Raycast (ray, out hit, 1500, 1 << 10)) {
				AlertCotouine = StartCoroutine (Alert ());
				_isAlert = true;
				yield return null;
			} else if (_isAlert && !Physics.Raycast (ray, out hit, 1000, 1 << 10)) {
				AlertAudio.Stop ();
				AlertUI.color = new Color (1, 1, 1, 0);
				StopCoroutine (AlertCotouine);
				_isAlert = false;
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator Deth ()
	{
		if (GameManager.IsGameOver) {
			yield break;
		}
		GameObject.Find ("Main Camera").transform.parent = null;
		Destroy (gameObject);
		yield return StartCoroutine (GameManager.FinishGame (false));
	}
}