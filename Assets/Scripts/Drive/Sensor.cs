using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{


    public GameObject lane_checker, r1,l1;

    public bool emergency = false;
    private float lane_limit = 0, limit_cut=0;
    
    public Sensor(GameObject lc,GameObject l_1, GameObject r_1)
    {
        lane_checker = lc;
        r1 = r_1;
        l1 = l_1;
    }
    private int CastRays(GameObject r_point, Vector3 Direction)
    {
        RaycastHit hit;
        int raydistance = 2;

        if (Physics.Raycast(r_point.transform.position, r_point.transform.TransformDirection(Direction), out hit, raydistance))
        {
            if (hit.collider.tag == "road")
            {
                Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.blue);
                return 0;
            }
            else if(hit.collider.tag=="offroad")
            {
                Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.yellow);
                return 1;
            }
            else
            {
                Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.red);
                return 2;
            }
        }
        else
        {
            Debug.DrawRay(r_point.transform.position, r_point.transform.TransformDirection(Direction) * hit.distance, Color.green);
            return 2;
        }
    }

    public float check()
    {
        emergency = false;
        float angle = 0;
        int l1value = CastRays(l1, Vector3.left);
        if(l1value==1)
        {
            emergency = true;
            angle = 3;
        }

        int r1value = CastRays(r1, Vector3.right);
        if(r1value==1)
        {
            emergency = true;
            angle = -3;
        }

        int lanecheck_value = CastRays(lane_checker, Vector3.left);
        if(lanecheck_value==0 && !emergency)//if hits road
        {
            
            lane_limit += 1;
            if (lane_limit<=5)
            {
                angle = -1;
            }else if(lane_limit>20)
            {
                lane_limit = 0;
            }
           
        }else if(lanecheck_value==1)
        {
            lane_limit = 0;
            
        }

        return angle;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
