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

	public static Vector3 AirFramePosition{ 
		get{
			
			return  AirFrame.transform.position;
		}
	}
	public static GameObject AirFrame { get;private set;}
	public static bool isLife { private set; get; }


	void Awake ()
	{
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		LightingSystem = FindObjectOfType<LightingControlSystem> ();
		isLife = true;
		AirFrame = gameObject;
	}

	void Start ()
	{
		StartCoroutine (NotificationSystem.UpdateNotification ("戦闘を開始します"));
		DamageEffectImage = GameObject.Find ("DamageEffectImage").GetComponent<Image>();
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

	private static Image DamageEffectImage;
	private static IEnumerator DamageEffect(){
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/HoneyCombMask");
		DamageEffectImage.color = new Color(214,0,0,DamageEffectImage.color.a);
		for (int i = 0; i < sprites.Length; i++) {
			DamageEffectImage.sprite = sprites [i];
			while (i == sprites.Length / 2 && !FirstAid) {
				yield return null;
			}
			yield return null;
		}
		DamageEffectImage.color = Color.blue;//new Color(70,100,200,DamageEffectImage.color.a);
		yield return null;
	}

	private static bool FirstAid = false;
	private void Bombed(){
		FirstAid = false;
		HP -= 1;
		LightingSystem.TurningOff (UIType.HP);
		StartCoroutine (CameraSystem.SwayCamera (b => FirstAid = b));
		StartCoroutine (DamageEffect ());
        StartCoroutine(InputVRController.ControllerPulse(2000, HandType.Both));
		PlayerSound.HitSound ();
	}

	private void DiedJudgment(GameObject Col){
		if (HP <= 0 || Col.layer == 10 || Col.layer == (int)Layers.EnemyArmor || Col.layer == (int)Layers.Enemy) {
			LightingControlSystem.ShatDown ();
			Instantiate (Resources.Load ("prefabs/Explosion"), AirFramePosition, Quaternion.identity);
			StartCoroutine (Death ());
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


	private IEnumerator Death ()
	{
        //		if (GameManager.IsGameOver) {
        //			yield break;
        //		}
        Destroy (gameObject);
		isLife = false;
		yield return StartCoroutine (GameManager.FinishGame (false));
	}
}