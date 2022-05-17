using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLearning : MonoBehaviour
{
    GameObject mainController;
    MainController mainScript;
    public string[] load_current_status;

    private Vector3[] robot_locations;
    IDictionary<string, int> robot_actions = new Dictionary<string, int>();

    enum TabletActions { None=0, Flash=0, LeftArrow=10, RightArrow=-10};
    enum RobotLocation { Far, AtDoor, Midway, Inroom};

    enum RobotTask { Picking, Dropping, Waiting};

    enum HumanState { Busy, Available};

    enum TabletOrientation { Left, Middle, Right};

    enum RobotInTabletFOV { Zero, One, Two, Three};
    
    
    // Start is called before the first frame update
    void Start()
    {
        mainController = GameObject.Find("MainController");
        mainScript = mainController.GetComponent<MainController>();
        Debug.Log("Q-learning started");
        initializeRobotLocations();
        initializeRobotActions();
    }

    private void initializeRobotActions()
    {
        robot_actions.Add("none",0);
        robot_actions.Add("flash",0);
        robot_actions.Add("leftArrow",0);
        robot_actions.Add("rightArrow",0);       
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

        robot_locations[0] = new Vector3(5.7f, 0, -2.2f);
        robot_locations[1] = new Vector3(8.0f, 0, 0);
        robot_locations[2] = new Vector3(-1.6f, 0, -8.35f);
        robot_locations[3] = new Vector3(-3.74f, 0, -9.42f);
        robot_locations[4] = new Vector3(8.0f, 0, 0);
        robot_locations[5] = new Vector3(8.0f, 0, 0);
        robot_locations[6] = new Vector3(8.0f, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {       
        load_current_status = mainScript.load_status_array;
        Debug.Log("QLearning values are :"+load_current_status[0]);
    }
}
