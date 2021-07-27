using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;
public class PathfindingTester : MonoBehaviour
{
    public GameObject right;
    public bool inverted = false;
    // The A* manager.
    private AStarManager AStarManager = new AStarManager();
    
    // Array of possible nodes.
    List<GameObject> nodes = new List<GameObject>();
    // Array of node map connections. Represents a path.
    List<Connection> ConnectionArray = new List<Connection>();
    // The start and end target point.
    public GameObject start;
    public GameObject end;
    // Debug line offset.
    Vector3 OffSet = new Vector3(0, 0.3f, 0);
    public float speed = 3f;
    public int count = 0;
    public int current = 0;
    public int inc = 1;
    private float timey;

    private bool done = false;

    // Start is called before the first frame update

    private float timer;
    void Start()
    {
        if (start == null || end == null)
        {
            Debug.Log("No start or end nodes.");
            return;
        }
        timer = Time.time;
        // Find all the nodes in the level.
        GameObject[] GameObjectsWithnodeTag;
        GameObjectsWithnodeTag = GameObject.FindGameObjectsWithTag("nodes");
       
        foreach (GameObject node in GameObjectsWithnodeTag)
        {
            node tmpnode = node.GetComponent<node>();
            
            if (tmpnode)
            {
                nodes.Add(node);
            }
        }
        // Go through the nodes and create connections.
        foreach (GameObject node in nodes)
        {
           
            node tmpnode = node.GetComponent<node>();
            
            // Loop through a nodes connections.
            foreach (GameObject nodeNode in tmpnode.Connections)
            {
                //Debug.Log("here?");
                Connection aConnection = new Connection();
                aConnection.SetFromNode(node);
                aConnection.SetToNode(nodeNode);
                AStarManager.AddConnection(aConnection);
                
            }
        }
        // Run A Star...
        ConnectionArray = AStarManager.PathfindAStar(start, end);
        foreach (Connection aConnection in ConnectionArray)
        {
            count++;
        }
        
       
        Vector3 source = ConnectionArray[current].GetFromNode().transform.position;
        
        gameObject.transform.position = new Vector3(source.x, gameObject.transform.position.y,source.z);

    }
   
    

    
    // Update is called once per frame

    void Update()
    {
        float angle=0;
        if(current==0)
        //since we go the end from the start and back to the start which is 0 we end when we explored 0 already
        if (current == -1) {
            if (!done) { 
                timer = Time.time - timer;
                Debug.Log(timer);
                done = true;
            }
            return;
        }
        

        if (current == count)// if reach end then iterate reverse
        {
            current--;
            inc = -1;
            timey = Time.time;

        }

        Vector3 dest = ConnectionArray[current].GetToNode().transform.position;
        Vector3 source = ConnectionArray[current].GetFromNode().transform.position;
        Vector3 player = new Vector3(gameObject.transform.position.x, dest.y, gameObject.transform.position.z);// make y axies independent so it dosent fly

        if (inc == -1)
        {
            dest = ConnectionArray[current].GetFromNode().transform.position;
            source = ConnectionArray[current].GetToNode().transform.position;

        }

        if (Mathf.Abs(Vector3.Distance(player,dest)) <0.6)
        {
            current += inc;           
            return;
        }

        Vector3 target = dest - player;

        angle = Vector3.SignedAngle(target, transform.forward, new Vector3(0,-1,0));
        
        Debug.Log(angle);
        Quaternion rot = Quaternion.AngleAxis(angle/30, new Vector3(0,1,0));
        Vector3 pivotPoint = right.transform.position;
        //transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;

        Vector3 direction = Vector3.RotateTowards(transform.forward, target, 5 * Time.deltaTime , 0.0f);
        
        //gameObject.transform.rotation = Quaternion.LookRotation(direction);
        //gameObject.transform.position = Vector3.MoveTowards(source, dest, Time.deltaTime*speed);
       
        gameObject.transform.Translate(transform.forward*Time.deltaTime*speed,Space.World);
        
    }

   
}

