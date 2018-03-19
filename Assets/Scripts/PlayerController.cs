﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GraphManager graph;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                //Debug.Log("hit...");
                if (hit.collider.gameObject.tag == "Node") {
                    //Debug.Log("hit node...");
                    Transform node = hit.collider.gameObject.transform;
                    //if (!node.GetComponent<Node>().selected) { // commented out so user can select a node more than once
                        node.GetComponent<Node>().select();
                        // place playercube on this node if it is the first selected
                        if (graph.CurrentPath.Count == 0) {
                            graph.InstantiateCube(node.transform);
                        }
                        graph.CurrentPath.Add(node.transform);
                    //}
                }
            }
        }
	}
}
