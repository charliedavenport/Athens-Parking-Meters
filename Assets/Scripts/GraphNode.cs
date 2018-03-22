using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour {

    [SerializeField]
    private Material NodeMat;
    [SerializeField]
    private Material SelectedNodeMat;

    public bool selected;

    private void Awake() {
        deselect();
    }

    public void select() {
        selected = true;
        GetComponent<Renderer>().material = SelectedNodeMat;
    }

    public void deselect() {
        selected = false;
        GetComponent<Renderer>().material = NodeMat;
    }
    

}
