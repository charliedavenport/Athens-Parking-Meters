using System.Collections;
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
                    node.GetComponent<GraphNode>().select();
                    graph.addNodeToPath(node);
                    //}
                }
            }
        }
	}
}
