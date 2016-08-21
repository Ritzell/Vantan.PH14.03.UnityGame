using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRMode : MonoBehaviour
{
    private static Toggle VRToggle;
    [SerializeField]
    private GameObject VRCameraOb, NormalCameraOb;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Camera VRCamera, NormalCamera;
    void Awake()
    {
        VRCameraOb.SetActive(false);
        VRToggle = gameObject.GetComponent<Toggle>();

    }
    public void isVR()
    {
        ChangePlayMode(VRToggle.isOn);
    }

    public void ChangePlayMode(bool isVR)
    {
        VRCameraOb.SetActive(isVR);
        NormalCameraOb.SetActive(!isVR);
        canvas.worldCamera = isVR ? VRCamera : NormalCamera;
    }
}
