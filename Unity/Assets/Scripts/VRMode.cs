using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;

public class VRMode : MonoBehaviour
{
    private static Toggle VRToggle;
    [SerializeField]
    private GameObject VRCameraOb, NormalCameraOb,SkyImage;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Camera VRCamera, NormalCamera;
    [SerializeField]
    private Text checkboxLabel;
    
    public static bool isVRMode
    {
        get
        {
            return VRToggle.isOn;
        }
    }
    void Awake()
    {
        VRCameraOb.SetActive(false);
        VRToggle = gameObject.GetComponent<Toggle>();
    }
    void Start()
    {
        VRConnectionConfirmation(VRDevice.isPresent);
    }
    public void isVR()
    {
        ChangePlayMode(VRToggle.isOn);
        FindObjectOfType<CameraSetting>().ChangeCamera(VRToggle.isOn);
    }

    public void ChangePlayMode(bool isVR)
    {
        VRCameraOb.SetActive(isVR);
        NormalCameraOb.SetActive(!isVR);
        canvas.worldCamera = isVR ? VRCamera : NormalCamera;
        canvas.GetComponent<Canvas>().renderMode = isVR ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera;
        SkyImage.SetActive(!isVR);
        canvas.GetComponent<RectTransform>().localScale = isVR ? new Vector3(0.0005f, 0.0005f, 0.05f) : new Vector3(0.1069f, 0.1069f, 1);
        canvas.GetComponent<RectTransform>().position = isVR ? new Vector3(0.15f,1,0.31f) : new Vector3(0, 100, 2);
    }

    public void VRConnectionConfirmation(bool isConnected)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((isConnected ? 50 : 255), 20);
        checkboxLabel.text = isConnected ? "VR" : "VR(VR機器が接続されていません。)";
        VRToggle.interactable = isConnected;

        //255
        //  VRTypeConfirmation(VRDevice.family)
    }

    public void VRTypeConfirmation(bool isSupporVR)
    {
        checkboxLabel.text = isSupporVR? "VR" : "VR(対応していないVR機器です。)";
    }
}
