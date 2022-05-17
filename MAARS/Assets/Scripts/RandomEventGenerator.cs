using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventGenerator : MonoBehaviour
{
    public GameObject RobotPrefab;
    private GameObject RobotObject;

    public List<Vector3> robot_locations = new List<Vector3>();

    public List<string> location_names = new List<string>();
    private float time = 0.0f;
    public float interpolationPeriod = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        initializeRobotLocations();
        RobotObject = Instantiate(RobotPrefab, Vector3.zero, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;
            changeRobotLocation();
        }
            //StartCoroutine(TimedExecution());
        
    }

    private void changeRobotLocation()
    {
        int number = UnityEngine.Random.Range(0, 4);
        Debug.Log(number);
        Debug.Log(robot_locations[number]);
        Debug.Log(location_names[number]);
        RobotObject.transform.position = robot_locations[number];
    }

    IEnumerator TimedExecution()
    {
        yield return new WaitForSeconds(10.0f);
        int number = UnityEngine.Random.Range(0, 9);
        Debug.Log(number);
        Debug.Log(robot_locations[number]);
        Debug.Log(location_names[number]);
        RobotObject.transform.position = robot_locations[number];


    }

    private void initializeRobotLocations()
    {
        int total_robot_locations = 5;
        //0 ---> Inside room big door
        //1 ---> Outside room big door
        //2 ---> Inside room small door
        //3 ---> Outside room small door
        //4 ---> Outside room big door
        //3 ---> Inside room big door
        //3 ---> Inside room big door
        //3 ---> Inside room big door
        
        robot_locations.Add(new Vector3(5.7f, 0, -2.2f));
        location_names.Add("InsideBigDoor");
        robot_locations.Add(new Vector3(8.0f, 0, 0));
        location_names.Add("OutsideBigDoor");
        robot_locations.Add(new Vector3(-1.6f, 0, -8.35f));
        location_names.Add("InsideSmallDoor");
        robot_locations.Add(new Vector3(-3.74f, 0, -9.42f));
        location_names.Add("OutsideSmallDoor");
        robot_locations.Add(new Vector3(23.35f, 0, -3.790f));
        location_names.Add("Hallway");
        robot_locations.Add(new Vector3(14.09f, 0, 17.74f));
        location_names.Add("Location2");
        robot_locations.Add(new Vector3(13.75f, 0, 9.66f));
        location_names.Add("Location2_half");
        robot_locations.Add(new Vector3(15.62f, 0, -6.98f));
        location_names.Add("Location1_half");
        robot_locations.Add(new Vector3(-3.03f, 0, 0.96f));
        location_names.Add("Location0_half");
        //robot_locations.Add(new Vector3(13.75f, 0, 9.66f));
        //location_names.Add("Location2_half");
        //robot_locations.Add(new Vector3(13.75f, 0, 9.66f));
        //location_names.Add("Location2_half");
        //robot_locations.Add(new Vector3(13.75f, 0, 9.66f));
        //location_names.Add("Location2_half");
        //robot_locations.Add(new Vector3(13.75f, 0, 9.66f));
        //location_names.Add("Location2_half");

    }
}
