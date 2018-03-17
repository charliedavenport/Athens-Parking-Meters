using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: Draw edges with multiplicity > 1
 *       Show 'Eulerized' edges as dotted
 */


[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour {

    [SerializeField]
    private Transform nodeA;
    [SerializeField]
    private Transform nodeB;
    [SerializeField]
    private int multiplicity;

    private LineRenderer lr;

    public void setNodes(Transform A, Transform B, int m) {
        nodeA = A;
        nodeB = B;
        multiplicity = m;
        lr.positionCount = 2;
        lr.SetPosition(0, nodeA.position);
        lr.SetPosition(1, nodeB.position);
    }

    private void Awake() {
        multiplicity = 0;
        lr = GetComponent<LineRenderer>();
        lr.SetPositions(null); // empty any default values
    }

    private void Start() {
        if (multiplicity > 0) {
            lr.positionCount = 2;
            lr.SetPosition(0, nodeA.position);
            lr.SetPosition(1, nodeB.position);
        }
    }

}
