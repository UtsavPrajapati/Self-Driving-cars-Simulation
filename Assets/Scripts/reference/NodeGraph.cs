using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeGraph : MonoBehaviour
{
   
    
    // The A* manager.
    public AStarManager aStarManager = new AStarManager();

    // Array of possible nodes.
    List<GameObject> nodes = new List<GameObject>();
    // Array of node map connections. Represents a path.
    List<Connection> ConnectionArray = new List<Connection>();
    // The start and end target point.
   
    
    

    public bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        
        
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
                aStarManager.AddConnection(aConnection);

            }
        }
        ready = true;
    }

   
}
