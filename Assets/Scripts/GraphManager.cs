using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour {

    [SerializeField]
    private Transform nodePrefab;
    [SerializeField]
    private Transform edgePrefab;
    [SerializeField]
    private Transform cubePrefab;
    [SerializeField]
    private Material nodeMat;
    [SerializeField]
    private Material selectedNodeMat;

    private Transform cube;
    [SerializeField]
    private Transform NodesObj;
    [SerializeField]
    private Transform EdgesObj;
    [SerializeField]
    private Canvas gui;
    [SerializeField]
    private Button walkBtn;
    private List<Transform> Nodes;
    private List<Transform> Edges;

    public List<Transform> CurrentPath;

    private bool walking;

    private void Awake() {
        Nodes = new List<Transform>();
        Edges = new List<Transform>();
        CurrentPath = new List<Transform>();

        walking = false;
        walkBtn.interactable = false;

        /*
        for (int x=0; x<5; x++) {
            for (int y=0; y<2; y++) {
                Nodes.Add(Instantiate(nodePrefab, new Vector3(x, 0, y), Quaternion.identity));
            }
        }
        //*/
        /*
        for (int i = 1; i < Nodes.Count; i++) {
            Edges.Add(Instantiate(edgePrefab, Vector3.zero, Quaternion.identity));
            Edge currentEdge = Edges[i - 1].GetComponent<Edge>();
            currentEdge.setNodes(Nodes[i], Nodes[i - 1], 1);
            
        }
        //*/

        foreach (Transform childNode in NodesObj) {
            Nodes.Add(childNode);
        } 

        //cube = Instantiate(cubePrefab, Nodes[0].position + Vector3.up * 0.5f, Quaternion.identity);

    }//Awake

    private void Start() {
        //StartCoroutine(delayed_walk());
        loadAdjacencyList();
    }

    public void InstantiateCube(Transform startNode) {
        cube = Instantiate(cubePrefab, startNode.position + Vector3.up * 0.5f, Quaternion.identity);
    }

    private void InstantiateEdge(int A, int B) {
        Transform newEdge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
        newEdge.GetComponent<Edge>().setNodes(Nodes[A], Nodes[B], 2);
        newEdge.SetParent(EdgesObj);
    }

    // not used anymore - was in initial prototype
    IEnumerator delayed_walk() {
        Nodes[0].GetComponent<Renderer>().material = selectedNodeMat;
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < Nodes.Count; i++) {
            Nodes[i - 1].GetComponent<Renderer>().material = nodeMat;
            Nodes[i].GetComponent<Renderer>().material = selectedNodeMat;
            //lerp to next node
            int n_iter = 20;
            Vector3 start_pos = Nodes[i - 1].position + Vector3.up * .5f;
            Vector3 end_pos = Nodes[i].position + Vector3.up * .5f;
            for (float t = 0f; t < 1f; t += 1f / n_iter) {
                cube.position = Vector3.Lerp(start_pos, end_pos, t);
                //yield return new WaitForSeconds(0.01f);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
	
    public void walkPath() {
        if (!walking) StartCoroutine(walk_path());
    }

    public void resetPath() {
        StopCoroutine(walk_path());
        foreach (Transform node in CurrentPath) {
            node.GetComponent<GraphNode>().deselect();
        }
        Destroy(cube.gameObject);
        CurrentPath = new List<Transform>();
        walkBtn.interactable = false;
    }
    
    IEnumerator walk_path() {
        walking = true;
        walkBtn.interactable = false;
        Vector3 cubeOffset = Vector3.up * .5f;
        cube.position = CurrentPath[0].position + cubeOffset;
        for (int i=1; i<CurrentPath.Count; i++) {
            Vector3 start_pos = CurrentPath[i-1].position + cubeOffset;
            Vector3 end_pos = CurrentPath[i].position + cubeOffset;
            int n_iter = 30;
            for (float t = 0f; t < 1f; t += 1f / n_iter) {
                cube.position = Vector3.Lerp(start_pos, end_pos, t);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.5f);
        }
        walking = false;
        walkBtn.interactable = true;
    }

    public void loadPathFromFile() {
        if (CurrentPath.Count > 0) resetPath();
        string filePath = Directory.GetCurrentDirectory() + @"\Path.txt";
        string pathStr = System.IO.File.ReadAllText(filePath);
        Debug.Log(pathStr);
        string[] lines = pathStr.Split('\n');
        InstantiateCube(Nodes[int.Parse(pathStr.Substring(0,1))]);
        foreach (string line in lines) {
            Transform nodeA = Nodes[int.Parse(line.Substring(0, 1))];
            Transform nodeB = Nodes[int.Parse(line.Substring(2, 1))];
            CurrentPath.Add(nodeA);
            CurrentPath.Add(nodeB);
            nodeA.GetComponent<GraphNode>().select();
            nodeB.GetComponent<GraphNode>().select();
        }
        walkBtn.interactable = true;
    }

    private void loadAdjacencyList() {
        /*  formatting of Adjacency_List.csv follows this pattern:
         *  
         *  1,,,
         *  0,7,2,
         *  1,8,3,
         */
        string filePath = Directory.GetCurrentDirectory() 
                                + @"\Assets\Scripts" // maybe should move to different folder
                                + @"\Adjacency_List.csv";
        string content = System.IO.File.ReadAllText(filePath);
        string[] lines = content.Split('\n');
        // parsing loop
        int node_A = 0; // node index
        foreach (string line in lines) {
            string[] words = line.Split(',');
            foreach (string word in words) {
                int node_B;
                if (int.TryParse(word, out node_B)) {
                    InstantiateEdge(node_A, node_B);
                }
            }
            ++node_A;
        }
    }

    

    public void addNodeToPath(Transform node) {
        if (CurrentPath.Count == 0) {
            // place playercube on this node if it is the first selected
            InstantiateCube(node.transform);
            walkBtn.interactable = true;
        }
        CurrentPath.Add(node.transform);
    }
	
}
