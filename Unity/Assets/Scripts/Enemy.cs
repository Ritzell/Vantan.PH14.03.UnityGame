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
	private int HP = 4;
	private Material MyMaterial;
	private Color MaterialColor;
	private Coroutine Breth;

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
		Breth = StartCoroutine (Breathing ());
	}

	void OnTriggerStay (Collider Col)
	{
		if (!isLife) {
			return;
		}
		HP--;
		if (HP <= 0) {
			isLife = false;
			EnemyBase.Rest = EnemyBase.Rest - 1;
			PlayerReticle.DestoroyLockOnTgt (gameObject);
			StartCoroutine (Deth ());
		}
	}

	private IEnumerator Breathing ()
	{
		float Turning = (MaterialColor.r + MaterialColor.g + MaterialColor.b) / 3;
		while (true) {

			while (0.05f < (MyMaterial.GetColor ("_EmissionColor").r + MyMaterial.GetColor ("_EmissionColor").g + MyMaterial.GetColor ("_EmissionColor").b) / 3) {
				Color mColor = MyMaterial.GetColor ("_EmissionColor");
				MyMaterial.SetColor ("_EmissionColor", new Color (mColor.r - (MaterialColor.r * (Time.deltaTime))
					, mColor.g - (MaterialColor.g * (Time.deltaTime))
					, mColor.b - (MaterialColor.b * (Time.deltaTime))));
				yield return null;
			}
			while (Turning > (MyMaterial.GetColor ("_EmissionColor").r + MyMaterial.GetColor ("_EmissionColor").g + MyMaterial.GetColor ("_EmissionColor").b) / 3) {
				Color mColor = MyMaterial.GetColor ("_EmissionColor");
				MyMaterial.SetColor ("_EmissionColor", new Color (mColor.r + (MaterialColor.r * (Time.deltaTime))
					, mColor.g + (MaterialColor.g * (Time.deltaTime))
					, mColor.b + (MaterialColor.b * (Time.deltaTime))));
				yield return null;
			}
		}
	}

	private IEnumerator Deth ()
	{
		CryBox.pitch = Random.Range (0.65f, 1.3f);
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
		Vector3 DefaltPos = transform.position;
		while (gameObject != null) {
			Vector3 RandomRange = new Vector3 (Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));
			transform.position = new Vector3 (DefaltPos.x + RandomRange.x,DefaltPos.y + RandomRange.y,DefaltPos.z + RandomRange.z);
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
