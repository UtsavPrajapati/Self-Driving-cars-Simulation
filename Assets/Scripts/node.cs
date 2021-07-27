using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class node : MonoBehaviour
{
    // List of all connections for the node.
   
    public List<GameObject> Connections = new List<GameObject>();
    
    private bool ObjectSelected = false;
    private Vector3 NoOffSet = new Vector3(0,0,0);
    private Vector3 UpOffSet = new Vector3(0, 0.1f, 0);
 
 
    void OnDrawGizmos()
    {       
        if (ObjectSelected)
        {
            DrawWaypoint(Color.yellow, Color.magenta, UpOffSet);
        }
        else
        {
            DrawWaypoint(Color.cyan, Color.blue, NoOffSet);
        }
        ObjectSelected = false;

    }


    // Draws debug objects when an object is selected.
    void OnDrawGizmosSelected()
    {
       ObjectSelected = true;
    }


    // Function to draw debug objects.
    private void DrawWaypoint(Color WaypointColor, Color ConnectionsColor, Vector3 OffSet)
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = WaypointColor;
        Gizmos.DrawCube(transform.position, new Vector3(2,2,2));
      
        for (int i = 0; i < Connections.Count; i++)
        {
            if (Connections[i] != null)
            {
                Gizmos.color = ConnectionsColor;
                Gizmos.DrawLine((transform.position + OffSet), (Connections[i].transform.position +OffSet));
            }
        }
    }
}