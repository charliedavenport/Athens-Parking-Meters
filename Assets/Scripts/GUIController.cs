using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    [SerializeField]
    private Text cameraViewButtonText;
    private string currentCameraView;

    private void Awake() {
        //currentCameraView = "default";
    }

    private void Update() {
        cameraViewButtonText.text = "Camera view: " + currentCameraView;

    }

    public void setCurrentCameraView(string s) { currentCameraView = s; }


}
