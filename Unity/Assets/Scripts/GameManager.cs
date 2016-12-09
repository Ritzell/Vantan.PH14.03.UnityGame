using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static CameraSystem CameraS;
    private static DateTime StartTime;
    private static GameManager Manager;
    private static MissileFactory Factory;
    private static bool _IsGameOver = false;
    public static Scenes NowScene;

    public static bool IsGameOver {
        get {
            return _IsGameOver;
        }
        set {
            if (value == false) {
                FindObjectOfType<GameManager>().StopCoroutines();
            }
            _IsGameOver = value;
        }
    }
    public static List<Coroutine> StageCoroutine { get; set; }
    public static int MissileCount { get; set; }
    public static int EnemyMIssilesCount { get; set; }

    private static TimeSpan restTime;
    /// <summary>
    /// 時間が0を下回るとscene移行するプロパティ
    /// </summary>
    public static TimeSpan RestTime {
        set {
            restTime = value;
            if (restTime.Minutes + restTime.Seconds <= 0) {
                Manager.StartCoroutine(FinishGame(false));
            }
        }
        get {
            return restTime;
        }
    }

    public enum Scenes {
        title,
        customize,
        stage,
        result
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CameraS = FindObjectOfType<CameraSystem>();
    }

    void Start() {
        //		StartCoroutine (Resetf);
        //		StartCoroutine(EscapeGame());
        IsGameOver = false;
        NowScene = Scenes.title;
    }

    public float GetDegree(float x1, float x2, float y1, float y2) {
        float radian = Mathf.Atan2(y1 - y2, x1 - x2);
        return radian * Mathf.Rad2Deg;
    }

    public float AbsDistance(Vector3 p1, Vector3 p2) {
        return Mathf.Abs(Vector3.Distance(p1, p2));
    }

    public float AbsDistance(Vector2 p1, Vector2 p2) {
        return Mathf.Abs(Vector2.Distance(p1, p2));
    }

    public void StartStage()
    {
        //QualitySettings.vSyncCount = 0; // VSyncをOFFにする
        //QualitySettings.antiAliasing = 0;
        //FindObjectOfType<CameraSetting>().MyCamera.GetComponent<Camera>().renderingPath = RenderingPath.DeferredShading;
        CameraS = FindObjectOfType<CameraSystem>();
        Manager = GameObject.FindObjectOfType<GameManager>();
        Factory = GameObject.FindObjectOfType<MissileFactory>();
        StartTime = DateTime.Now;
        StartCoroutine(Timer());//タイマーを起動
        StartCoroutine(EscapeGame());
        QualitySettings.antiAliasing = GameSetting.GameSetDates[(int)GameSetting.DateNumber.AntiAliasing];//AntiAliasing
        QualitySettings.vSyncCount = GameSetting.GameSetDates[(int)GameSetting.DateNumber.Vsync];//vsync

        foreach (Light light in FindObjectsOfType<Light>())
        {//shadow
            switch (GameSetting.GameSetDates[(int)GameSetting.DateNumber.Shadow])
            {
                case 0:
                    light.shadows = LightShadows.None;
                    break;
                case 1:
                    light.shadows = LightShadows.Hard;
                    break;
                case 2:
                    light.shadows = LightShadows.Soft;
                    break;
            }
        }
    }

    public IEnumerator ReloadMissile(Vector3 StartPos, Quaternion StartRot)
    {
        yield return new WaitForSeconds(3f);
        Attack.PlayerMissiles.Enqueue(Factory.NewPlayerMissile(StartPos, StartRot, true));
        yield return null;
    }

    /// <summary>
    /// 制限時間の設定と残り時間を計算するメソッドの実行
    /// </summary>
    private IEnumerator Timer()
    {
        Text Timetext = GameObject.Find("Timer").GetComponent<Text>();
        TimeSpan LimitTime = new TimeSpan(00, 6, 00);
        while (!IsGameOver) {
            StartCoroutine(DisplayTime(Timetext, LimitTime));
            yield return null;
        }
    }

    /// <summary>
    /// 残り時間をString型に変換
    /// </summary>
    public static string TimeCastToString(TimeSpan Time)
    {
        return Time.Minutes.ToString("D2") + ":" + Time.Seconds.ToString("D2");//timeString;
    }

    /// <summary>
    /// GUITextに残り時間を表記する。
    /// </summary>
    /// <param name="Timetext">Timetext.</param>
    /// <param name="limitTime">Limit time.</param>
    private static IEnumerator DisplayTime(Text Timetext, TimeSpan limitTime)
    {
        TimeCalculation(limitTime);
        Timetext.text = TimeCastToString(RestTime);
        yield return null;
    }

    /// <summary>
    /// 残り時間を計算
    /// </summary>
    private static void TimeCalculation(TimeSpan limitTime)
    {
        TimeSpan elapsedTime = (TimeSpan)(DateTime.Now - StartTime);
        RestTime = limitTime - elapsedTime;
    }

    public static IEnumerator FlashLoadScene(Scenes scene)
    {
        bool isOut = false;
        CameraS = FindObjectOfType<CameraSetting>().MyCamera.GetComponent<CameraSystem>();
        CameraS.StartCoroutine(CameraS.Flash(3f, true, 1, GameObject.Find("Canvas"), fadeout => isOut = fadeout));
        while (!isOut) {
            yield return null;
        }
        NowScene = scene;
        if (scene == Scenes.title) {
            FindObjectOfType<Camera>().gameObject.SetActive(false);
        }
        SceneManager.LoadSceneAsync((int)scene);
        yield return null;
        if (scene == Scenes.title) {
            FindObjectOfType<GameManager>().gameObject.SetActive(false);//CameraSetting> ().MyCamera;//.SetActive (false);
        }
        yield return null;
    }

    //ゲームを途中で終了
    private IEnumerator EscapeGame() {
        while (!GameManager.IsGameOver) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ResetGame();
                CameraS.StartCoroutine(GameManager.FlashLoadScene(GameManager.Scenes.title));
            }
            yield return null;
        }
    }

    //	private static IEnumerator 

    /// <summary>
    /// ゲームをリセットして初期状態（タイトル画面）に戻る。
    /// </summary>
    public static void ResetGame()
    {
        IsGameOver = false;
        MissileCount = 0;
        EnemyMIssilesCount = 0;
        Destroy(GameObject.Find("GameManager"));
        // TODO: ゲームの初期化。
    }

    /// <summary>
    /// ゲームを終了してリザルト画面に移る
    /// </summary>
    /// <returns>The game.</returns>
    /// <param name="isWin">If set to <c>true</c> is window.</param>
    public static IEnumerator FinishGame(bool isWin)
    {
        StopGame();
        GameObject camera = VRMode.isVRMode ? GameObject.Find("[CameraRig]") : FindObjectOfType<CameraSetting>().MyCamera;
        camera.transform.parent = null;
        DontDestroyOnLoad(camera);
        try {
            File.WriteAllBytes(ImageCamera.ImagePath, ImageCamera.OutPutTexture2D.EncodeToPNG());
        } catch {
            Debug.Log("画像の保存に失敗しました.");
        }
        AudioSource AudioBox = FindObjectOfType<CameraSetting>().MyCamera.GetComponent<AudioSource>();
        Record.IsVictory = isWin;
        Manager.StartCoroutine(Manager.StopSounds());
        Manager.StartCoroutine(isWin ? Victory() : Defeat());
        Manager.StartCoroutine(ChangeMusic(AudioBox, isWin));
        yield return null;

    }

    /// <summary>
    /// すべてのサウンドを停止させる
    /// </summary>
    /// <returns>The sounds.</returns>
    public IEnumerator StopSounds()
    {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>()) {
            audio.Stop();
        }
        yield return null;
    }

    private static IEnumerator ChangeMusic(AudioSource AudioBox, bool isWin)
    {
        float TimeSpeed = 1 / Time.timeScale;

        while (AudioBox.volume > 0) {
            AudioBox.volume -= 0.25f * (Time.deltaTime * TimeSpeed);
            yield return null;
        }
        AudioBox.Stop();
        yield return null;
        NewMusicSet(AudioBox, isWin);
        StageResultText.DisplayResult(isWin);
        yield return null;
    }

    private static void NewMusicSet(AudioSource AudioBox, bool isWin)
    {
        AudioBox.clip = (AudioClip)(Resources.Load(isWin ? "Sounds/FromTheNewWorld" : "Sounds/Sarabande"));
        AudioBox.volume = isWin ? 0.65f : 0.5f;
        AudioBox.loop = false;
        AudioBox.Play();
    }

    public static IEnumerator Victory()
    {
        yield return new WaitForSeconds(5 * Time.timeScale);
        while (true) {
            if (isNext()) {
                yield return Manager.StartCoroutine(FlashLoadScene(GameManager.Scenes.result));
                yield break;
            }
            yield return null;
        }

    }

    public static IEnumerator Defeat()
    {
        CameraS.StartCoroutine(CameraS.CameraOut());
        yield return new WaitForSeconds(5 * Time.timeScale);
        while (true) {
            if (isNext()) {
                FindObjectOfType<CameraSystem>().StopCoroutine("CameraOut");
                yield return Manager.StartCoroutine(FlashLoadScene(GameManager.Scenes.result));
                yield break;
            }
            yield return null;
        }
    }

    private void StopCoroutines() {
        //StageCoroutine.ForEach (coroutine => StopCoroutine (coroutine));
    }

    private static bool isNext()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton9) || InputVRController.GetPress(InputVRController.InputPress.PressGrip,HandType.Right);
    }

    private static void StopGame()
    {
        IsGameOver = true;
        Time.timeScale = 0.015f;
    }

    public static GameObject FirstParent(GameObject child)
    {
        return child.transform.parent == null ? child : FirstParent(child.transform.parent.gameObject);
    }


    /// <summary>
    /// ベジェ曲線
    /// </summary>
    /// <param name="t">T.</param>
    /// <param name="p1">P1.</param>
    /// <param name="p2">P2.</param>
    /// <param name="p3">P3.</param>
    /// <param name="p4">P4.</param>
    public float Veje(float t, float p1, float p2, float p3, float p4) {
        float pos = (1 - t) * (1 - t) * (1 - t) * p1 + 3 * (1 - t) * (1 - t) * t * p2 + 3 * (1 - t) * t * t * p3 + t * t * t * p4;
        return pos;
    }

    /// <summary>
    /// 符号を維持した三平方の定理 
    /// 二次元ベクトルの計算の場合はベクトルごとに分けて計算
    /// </summary>
    /// <returns>The theorem.</returns>
    /// <param name="a">The alpha component.</param>
    /// <param name="b">The blue component.</param>
    public float PythagoreanTheorem(float a, float b) {
        return a + b < 0 ? -(Mathf.Pow(a, 2) + Mathf.Pow(b, 2)) : Mathf.Pow(a, 2) + Mathf.Pow(b, 2);
    }

    /// <summary>
    /// 指定された2つのオブジェクトの親子関係を制御。親子関係を解除された場合、親オブジェクトは破壊される。
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="parent"></param>
    /// <param name="isDocking"></param>
    public static void RemovableObject(GameObject origin, GameObject parent, bool isDocking)
    {
        origin.transform.parent = isDocking ? parent.transform : null;
        if (!isDocking)
        {
            Destroy(parent);
        }
    }


	/// <summary>
	/// 符号を維持した平方根
	/// </summary>
	/// <param name="sqrt">Sqrt.</param>
	/// <param name="origin">Origin.</param>
	public float ImaginarySqrt(float c, float sign){
		float sqrt = Mathf.Sqrt (Mathf.Abs(c));
		return sign == -1 ? -sqrt : sqrt;
	}
}