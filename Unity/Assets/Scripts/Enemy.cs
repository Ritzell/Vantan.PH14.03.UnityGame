using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private AudioSource CryBox;
	[SerializeField]
	private Material material;

//	private Material Emission;
	private bool isLife = true;
	private int HP = 4;

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

	void Awake(){
		PlayerReticle = GameObject.Find("ReticleImage").GetComponent<ReticleSystem>();
		Frame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		Tgt = GameObject.Find ("eurofighter");
	}

	void Start ()
	{
		EnemyBase.Rest = EnemyBase.Rest + 1;
		StartCoroutine (Breathing ());
		material.color = new Color (0.3f,0.3f,0.3f,1);
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

	private IEnumerator Breathing(){
		Material myMaterial;
		myMaterial = gameObject.GetComponent<Renderer> ().material;
		myMaterial.EnableKeyword("_EMISSION");
		Color MaterialMaxColor = myMaterial.GetColor("_EmissionColor");
		float Turning = (MaterialMaxColor.r + MaterialMaxColor.g + MaterialMaxColor.b)/3;
		while(true){

			while (0.05f < (myMaterial.GetColor("_EmissionColor").r + myMaterial.GetColor("_EmissionColor").g + myMaterial.GetColor("_EmissionColor").b) / 3) {
				Color mColor = myMaterial.GetColor ("_EmissionColor");
				myMaterial.SetColor("_EmissionColor", new Color (mColor.r - (MaterialMaxColor.r * (Time.deltaTime))
					, mColor.g - (MaterialMaxColor.g * (Time.deltaTime))
					, mColor.b - (MaterialMaxColor.b * (Time.deltaTime))));
				yield return null;
			}
			while (Turning > (myMaterial.GetColor("_EmissionColor").r + myMaterial.GetColor("_EmissionColor").g + myMaterial.GetColor("_EmissionColor").b) / 3) {
				Color mColor = myMaterial.GetColor ("_EmissionColor");
				myMaterial.SetColor("_EmissionColor", new Color (mColor.r + (MaterialMaxColor.r * (Time.deltaTime))
					, mColor.g + (MaterialMaxColor.g * (Time.deltaTime))
					, mColor.b + (MaterialMaxColor.b * (Time.deltaTime))));
				yield return null;
			}
		}
	}

	private IEnumerator Deth(){
		var color = material.color;
		CryBox.pitch = Random.Range (0.65f,1.3f);
		CryBox.Play ();
		yield return null;
		while(true){
			if (CryBox.isPlaying == false) {
				Frame.StartCoroutine (NotificationSystem.UpdateNotification(gameObject.name + "を撃破しました！"));
				Destroy (gameObject);
			} else {
				Expansion ();
				Hyperemia (color);
			}
			yield return null;
		}
	}
	private void Expansion(){
		transform.localScale = new Vector3 (transform.localScale.x + (800 * (Time.deltaTime / 3)), transform.localScale.y + (800 * (Time.deltaTime / 3)), transform.localScale.z + (800 * (Time.deltaTime / 3)));
	}
	private void Hyperemia(Color startColor){
		material.color = new Color (material.color.r + ((0.5f - startColor.r) * Time.deltaTime/2),material.color.g + ((0 - startColor.g) * Time.deltaTime/2),material.color.b + ((0 - startColor.b) * Time.deltaTime/2));
		Debug.Log (material.color);
	}
}
