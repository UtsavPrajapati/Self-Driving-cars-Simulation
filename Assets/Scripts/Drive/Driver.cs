using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public GameObject start;
    public GameObject end;

    public GameObject nodes_graph;
    private AStarManager astar;

    public GameObject r1;
    public GameObject l1;

    List<Connection> ConnectionArray = new List<Connection>();


    private bool ready = false;

    public float speed = 3f;
    public int count = 0;
    public int current = 0;
    public int inc = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float CastRays(GameObject r_point, Vector3 Direction, bool lane_correct=false)
    {
        RaycastHit hit;
        int raydistance = 2;
        
        if (Physics.Raycast(r_point.transform.position, r_point.transform.TransformDirection(Direction), out hit,raydistance))
        {
            if (hit.collider.tag == "road")
            {
                Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.green);

                if(lane_correct)
                {
                    
                    return -5;
                    
                }

            }else if(hit.collider.tag=="offroad")
            {
                Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.red);
            }

        }
      
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(nodes_graph.GetComponent<NodeGraph>().ready && !ready)
        {
            astar = nodes_graph.GetComponent<NodeGraph>().aStarManager;

            ConnectionArray = astar.PathfindAStar(start, end);
            foreach (Connection aConnection in ConnectionArray)
            {
                count++;
            }


            Vector3 source = ConnectionArray[current].GetFromNode().transform.position;

            gameObject.transform.position = new Vector3(source.x, gameObject.transform.position.y, source.z);
            ready = true;
        }


       


        if(ready)
        {
            float angle = 0;
            if (current == 0)
                //since we go the end from the start and back to the start which is 0 we end when we explored 0 already
                if (current == -1)
                {
                    
                    return;
                }


            if (current == count)// if reach end then iterate reverse
            {
                current--;
                inc = -1;
               

            }

           

            Vector3 dest = ConnectionArray[current].GetToNode().transform.position;
            Vector3 source = ConnectionArray[current].GetFromNode().transform.position;
            Vector3 player = new Vector3(gameObject.transform.position.x, dest.y, gameObject.transform.position.z);// make y axies independent so it dosent fly

            if (inc == -1)
            {
                dest = ConnectionArray[current].GetFromNode().transform.position;
                source = ConnectionArray[current].GetToNode().transform.position;

            }

            
            if (Mathf.Abs(Vector3.Distance(player, dest)) < 4)
            {
                float threshold = 0.1f;
                if(current+inc < count && current+inc<=0)
                {
                    Vector3 nsource, ndest, temp;

                    nsource = ConnectionArray[current+inc].GetFromNode().transform.position;
                    ndest = ConnectionArray[current + inc].GetToNode().transform.position;

                    Vector3 ntarget = ndest - nsource;
                    if (inc==-1)
                    {
                        ntarget =  nsource-ndest; // from and to nodes are opposite when travveling in reverse
                    }

                    

                    float nangle = Vector3.SignedAngle(ntarget, transform.forward, new Vector3(0, -1, 0));

                    if(nangle<0) // if turning left turn a little early
                    {
                        
                        threshold = 3f;
                    }

                }

                if (Mathf.Abs(Vector3.Distance(player, dest)) <= threshold)
                {
                    current += inc;
                    return;
                }
            }



            CastRays(r1, Vector3.right);
            float lane_angle = CastRays(l1, Vector3.left,true);


            Vector3 target = dest - player;

            angle = Vector3.SignedAngle(target, transform.forward, new Vector3(0, -1, 0));
            
            
            angle = Mathf.Abs(lane_angle)> Mathf.Abs(angle)? lane_angle:angle/50 ;
           
            
            
            Quaternion rot = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
            
            //transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
            transform.rotation = rot * transform.rotation;

            //Vector3 direction = Vector3.RotateTowards(transform.forward, target, 5 * Time.deltaTime, 0.0f);

            //gameObject.transform.rotation = Quaternion.LookRotation(direction);
            //gameObject.transform.position = Vector3.MoveTowards(source, dest, Time.deltaTime*speed);

            gameObject.transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        }
    }
}
