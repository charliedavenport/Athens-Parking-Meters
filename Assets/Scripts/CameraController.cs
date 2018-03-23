using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform pivots_gameobj;
    [SerializeField]
    private GUIController gui;
    //[SerializeField]
    private List<Transform> cameraPivots;
    private int currentPivot_ind;
    private Transform currentPivot;
    private bool transitioning; // prevent coroutine being called multiple times 

    // initialize
    private void Awake() {
        
        cameraPivots = new List<Transform>();
        currentPivot_ind = 0;
        // get the transforms of all children under this gameObject
        for (int i=0; i<pivots_gameobj.childCount; i++) {
            cameraPivots.Add(pivots_gameobj.GetChild(i));
        }
        currentPivot = cameraPivots[currentPivot_ind];
        this.transform.position = currentPivot.position;
        this.transform.rotation = currentPivot.rotation;

        gui.setCurrentCameraView(currentPivot.gameObject.name);
    }

    // call this from other scripts to change camera view
    public void nextPivot() {
        
        if (!transitioning) { // this if-statement prevents coroutine from being interrupted
            currentPivot_ind = (currentPivot_ind + 1) % cameraPivots.Count; // next pivot (cycles back to first)
            Transform nextPivot = cameraPivots[currentPivot_ind];
            StartCoroutine(do_pivot(currentPivot, nextPivot));
            currentPivot = nextPivot;
            gui.setCurrentCameraView(currentPivot.gameObject.name);

        }

    }

    // coroutine
    // updates camera position/rotation every fixed update
    IEnumerator do_pivot(Transform start, Transform stop) {
        transitioning = true;
        int n_iter = 20;
        float incr = 1f / (float)n_iter;
        for (float t = 0; t < 1f; t += incr) {
            this.transform.position = Vector3.Lerp(start.position, stop.position, t);
            this.transform.rotation = Quaternion.Lerp(start.rotation, stop.rotation, t);
            yield return new WaitForFixedUpdate();
        }
        transitioning = false; 
    }


}//CameraController
