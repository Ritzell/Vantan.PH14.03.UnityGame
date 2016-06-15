using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private AudioSource CryBox;
	[SerializeField]
	private Material ArmorMaterial;
	[SerializeField]
	private CameraSystem CameraS;

	//	private Material Emission;
	private bool isLife = true;
	private float HP = 1000;
	private Material MyMaterial;
	private Color MaterialColor;
	private Coroutine Breth;
	private IEnumerator StateNotice;

	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>
	private static GameObject tgt;
	public static GameObject Tgt {
		set {
			tgt = value;
		}
		get {
			return tgt;
		}
	}

	private static ReticleSystem PlayerReticle;
	private static Airframe Frame;

	void Awake ()
	{
		PlayerReticle = GameObject.Find ("ReticleImage").GetComponent<ReticleSystem> ();
		Frame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		Tgt = GameObject.Find ("eurofighter");
		MyMaterial = gameObject.GetComponent<Renderer> ().material;

	}

	void Start ()
	{
		ArmorMaterial.color = new Color (0.3f, 0.3f, 0.3f, 1);
		MaterialColor = MyMaterial.GetColor ("_EmissionColor");
		EnemyBase.Rest = EnemyBase.Rest + 1;
		Breth = StartCoroutine (Respiration ());
		StateNotice = StateNotification();
		CryBox.pitch = Random.Range (0.65f, 1.3f);
	}

	void OnTriggerStay (Collider Col)
	{
		if (!isLife) {
			return;
		}
		if (Col.gameObject.layer == (int)PlayerAttackPower.bulletLayer) {
			HP -= (int)PlayerAttackPower.bullet;
		} else if (Col.gameObject.layer == (int)PlayerAttackPower.missileLayer) {
			HP -= (int)PlayerAttackPower.missile;
		}
		StateNotice.MoveNext ();
		DiedJudgment ();
	}
	private void DiedJudgment(){
		if (HP <= 0) {
			isLife = false;
			EnemyBase.Rest = EnemyBase.Rest - 1;
			PlayerReticle.DestoroyLockOnTgt (gameObject);
			StartCoroutine (Deth ());
		}
	}

	private IEnumerator StateNotification(){
		bool isPassing = false;
		while (!isPassing) {
			if (HP <= 500) {
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "の体力が著しく消耗しています。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= 250) {
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "が非常に弱っています"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= 50) {
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "敵がもう少しで撃破できます。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator Respiration ()
	{
		Material material;
		material = gameObject.GetComponent<Renderer> ().material;
		material.EnableKeyword ("_EMISSION");
		Color MaterialMaxColor = material.GetColor ("_EmissionColor");
		float Turning = (MaterialMaxColor.r + MaterialMaxColor.g + MaterialMaxColor.b) / 3;
		while (true) {

			while (0.05f < (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Brethes (material, MaterialMaxColor);
				yield return null;
			}
			while (Turning > (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Suck (material, MaterialMaxColor);
				yield return null;
			}
		}
	}

	private void Brethes(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r - (MaxColor.r * (Time.deltaTime))
			, mColor.g - (MaxColor.g * (Time.deltaTime))
			, mColor.b - (MaxColor.b * (Time.deltaTime))));
	}

	private void Suck(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r + (MaxColor.r * (Time.deltaTime))
			, mColor.g + (MaxColor.g * (Time.deltaTime))
			, mColor.b + (MaxColor.b * (Time.deltaTime))));
	}

	private IEnumerator Deth ()
	{
		CryBox.Play ();
		StopCoroutine (Breth);
		MyMaterial.EnableKeyword ("_EMISSION");
		MyMaterial.SetColor ("_EmissionColor",Color.red);
		StartCoroutine (StateChange (CryBox));
		StartCoroutine (ShakeBody());
		CameraS.StartCoroutine(CameraS.Flash(0.8f,true,0.35f,gameObject));
		while (true) {
			if (CryBox.isPlaying == false) {
				Frame.StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "を撃破しました！"));
				Destroy (gameObject);
			}
			yield return null;
		}
	}

	private IEnumerator StateChange(AudioSource AudioBox){
		var color = ArmorMaterial.color;
		while (AudioBox.isPlaying) {
			Expansion ();
			Hyperemia (color);
			yield return null;
		}
	}

	private IEnumerator ShakeBody(){
		Vector3 DefaultPos = transform.position;
		while (gameObject != null) {
			Vector3 RandomRange = new Vector3 (Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));
			transform.position = new Vector3 (DefaultPos.x + RandomRange.x,DefaultPos.y + RandomRange.y,DefaultPos.z + RandomRange.z);
			yield return null;
		}
	}

	private void Expansion ()
	{
		transform.localScale = new Vector3 (transform.localScale.x + (800 * (Time.deltaTime / 3)), transform.localScale.y + (800 * (Time.deltaTime / 3)), transform.localScale.z + (800 * (Time.deltaTime / 3)));
	}

	private void Hyperemia (Color startColor)
	{
		ArmorMaterial.color = new Color (ArmorMaterial.color.r + ((0.5f - startColor.r) * Time.deltaTime / 2), ArmorMaterial.color.g + ((0 - startColor.g) * Time.deltaTime / 2), ArmorMaterial.color.b + ((0 - startColor.b) * Time.deltaTime / 2));
	}
}
