using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public GameObject start;
    public GameObject end;

    public GameObject nodes_graph;
    private AStarManager astar;

    public GameObject lanecheck;
    public GameObject r1;
    public GameObject l1;

    List<Connection> ConnectionArray = new List<Connection>();


    private bool ready = false;

    public float speed = 2f;
    public int count = 0;
    public int current = 0;
    public int inc = 1;

    bool imp_turn = true, about_turn = false;
    float turn_angle = 40;

    private Sensor sensor;
    // Start is called before the first frame update
    void Start()
    {
        sensor = new Sensor(lanecheck,l1, r1);
    }

    

    // Update is called once per frame
    void Update()
    {
        //wait till the graph has finished loading
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
            {

            }
                //since we go the end from the start and back to the start which is 0 we end when we explored 0 already
            if (current == -1)
            {
                current++;
                inc = 1;
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

            
            if (Mathf.Abs(Vector3.Distance(player, dest)) < 3.5f)
            {
                float threshold = 2;
                float nangle;
                current+=inc;
                imp_turn = true;
                if(current==count || current==-1)
                {
                    about_turn = true;
                }
                
                /*if (current + inc < count && current + inc >= 0)
                {
                   
                    Vector3 nsource, ndest, temp;

                    nsource = ConnectionArray[current + inc].GetFromNode().transform.position;
                    ndest = ConnectionArray[current + inc].GetToNode().transform.position;

                    Vector3 ntarget = ndest - nsource;
                    if (inc == -1)
                    {
                        ntarget = nsource - ndest; // from and to nodes are opposite when travveling in reverse
                    }



                    nangle = Vector3.SignedAngle(ntarget, transform.forward, new Vector3(0, -1, 0));

                    if (nangle < 0 && Mathf.Abs(nangle) < 100)// if turning left turn a little early
                    {

                        threshold = 3.5f;
                    }
                    
                }
                if (Mathf.Abs(Vector3.Distance(player, dest)) <= threshold)
                {
                    current += inc;
                    imp_turn = true;
                    Debug.Log("turning");
                }*/
            }


            Vector3 target = dest - player;

            angle = Vector3.SignedAngle(target, transform.forward, new Vector3(0, -1, 0));


            if (about_turn)
            { 
                
                angle = Mathf.Abs(angle);
            }
            

            float sensor_angle = sensor.check();

            if(Mathf.Abs(angle)<40)
            {
                about_turn = false;
            }
            if (sensor.emergency)
            {
                angle = sensor_angle;
            }
            else if (imp_turn)
            {
                if (Mathf.Abs(angle) < 10)
                {
                    imp_turn = false;
                    about_turn = false;
                }
                else
                {
                    angle = angle / 40;
                }
            }
            else
            {
                if(Mathf.Abs(angle)>60)
                {
                    angle = angle / 50;
                }
                else
                {
                    angle = sensor_angle;
                }
                
            }

            

            
            
            

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
