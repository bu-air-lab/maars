using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarehouseCreator : MonoBehaviour
{
    public GameObject ShelfPrefab;
    public GameObject SingleWallPrefab;
    public GameObject DropStationPrefab;
    public GameObject TurtlebotPrefab;
    public GameObject HumanPrefab;
    public GameObject Camera;
    public GameObject HumanCube;
    public NavMeshSurface surface;
    public bool doCornerDropStations;
    public bool omnipotentRobots;
    public bool doVisionAllocation;
    public int nDropStations;
    public int nRobots;
    public int ShelfRows;
    public int ShelfColumns;
    public int taskSeed = 42;
    public int totalBoxesPerRobot;
    float rowSpacing = 3;
    float colSpacing = 3.0f;
    int numShelfRowsGroup = 3;
    int boxes_in_one_shelf = 12;
    float initial_ZSpacing_Shelf = 1.0f;
    float initial_XSpacing_Shelf = 1.0f;
    float wallWidth = 4; // Width of the wall prefab (z value)
    float wallHeight = 1.5f; // Height of the wall prefab to be on be floor (y value)

    float shelfHeightAboveGround = 1.13f; // Distance to instantiate shelf to be on the floor
    float dropStationWidth = 1.0f; // Drop station prefab width
    List<GameObject> shelf_objects = new List<GameObject>(); // All the Shelf gameobjects to iterate
    List<GameObject> dropStation_objects = new List<GameObject>(); // All the dropStation gameobjects to iterate
    List<GameObject> turtlebot_objects = new List<GameObject>(); // All the Turtlebot gameobjects to iterate
    List<GameObject> shelfBox_objects = new List<GameObject>(); // All the Boxes in Shelves gameobjects to iterate
    List<GameObject> current_assigned_robots_locations = new List<GameObject>(); // All the gameobjects that are assigned to the robots to go to, where the index is the robot number same as the turtlebot objects. The list will be updated based on the next location that the robot has to go (Box location or the drop location or anything else).
    List<int> completed_boxes = new List<int>(); // Keeps track of how many packages each robot has dropped off
    List<double>[] robotTimes; // Tracks the timings for each robot
    List<double>[] robotDistances; // Stores the total distance a robot had to move in one trial
    List<Vector3> initial_robot_spawn_locations = new List<Vector3>();
    bool[] atDropStation; // Stores whether a certain robot is waiting at a drop station
    int[,] positions = new int[,] { { -1, 24 }, { -1, -1 }, { 12, 24 }, { 12, -1 } };
    int[] viewOccurences = new int[4];

    int current_shelf_row = 0;
    int current_shelf_col = 0;
    int current_shelf_box = 0;
    int human_goal = 0;

    enum HumanState
    {
        movingToDropstation,
        LookingAtDropstation1,
        LookingAtDropstation2,
        LookingAtDropstation3,
        LookingAtDropstation4
    }

    HumanState currState = HumanState.movingToDropstation;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes Camera
        Camera = GameObject.Find("Magic Leap Cam");
        if (Camera == null)
        {
            Camera = GameObject.Find("Environment View");
            if (Camera == null)
            {
                Debug.Log("Could not find camera for getAvailableDropStation. Check that either \"Magic Leap Cam\" or \"Environment View\" cameras are enabled.");
            }
        }

        //Sets up human-related variables
        HumanCube = GameObject.Find("Cube");

        //Sets human goal
        HumanCube.transform.position = new Vector3(positions[human_goal, 0], 0.1f, positions[human_goal, 1]);

        //Sets seed for robot boxes
        UnityEngine.Random.seed = taskSeed;

        for (int i = 0; i < ShelfRows; i++) 
        {
            for (int j = 0; j < ShelfColumns; j++)
            {
                GameObject current_created_shelf;
                current_created_shelf = Instantiate(ShelfPrefab, new Vector3(initial_XSpacing_Shelf+(j* colSpacing), shelfHeightAboveGround, initial_ZSpacing_Shelf + (i * 2.5f + ((i/ numShelfRowsGroup) * rowSpacing))), Quaternion.identity);
                //Debug.Log(current_created_shelf.name);
                current_created_shelf.name = "Shelf_" + i + "_" + j;
                shelf_objects.Add(current_created_shelf);
                //Debug.Log(current_created_shelf.name);
            }
        }

        GameObject wallsWarehouse = new GameObject(); // To add all walls as the child to the wallsWarehouse gameobject
        wallsWarehouse.name = "wallsWarehouse"; 
        for (int b = 0; b <= ShelfRows; b++)
        {
            GameObject current_created_wall;
            // Back side wall: Robot side
            current_created_wall = Instantiate(SingleWallPrefab, new Vector3(-2, wallHeight, (b * wallWidth)), Quaternion.identity);
            current_created_wall.transform.parent = wallsWarehouse.transform;

            current_created_wall = Instantiate(SingleWallPrefab, new Vector3(((ShelfColumns) * wallWidth) + wallWidth/2, wallHeight, (b * wallWidth)), Quaternion.identity);
            current_created_wall.transform.parent = wallsWarehouse.transform;

        }


        // Walls width
        for (int c = 0; c <= ShelfColumns; c++)
        {
            GameObject current_created_wall;

            current_created_wall = Instantiate(SingleWallPrefab, new Vector3((c * wallWidth), wallHeight, -(wallWidth / 2)), Quaternion.identity);
            current_created_wall.transform.Rotate(0, 90.0f, 0.0f, Space.Self);
            current_created_wall.transform.parent = wallsWarehouse.transform;

            current_created_wall = Instantiate(SingleWallPrefab, new Vector3((c * wallWidth), wallHeight, (ShelfRows * wallWidth)+2), Quaternion.identity);
            current_created_wall.transform.Rotate(0, 90.0f, 0.0f, Space.Self);
            current_created_wall.transform.parent = wallsWarehouse.transform;

        }

        // Add drop stations
        GameObject dropStationsWarehouse = new GameObject(); // To add all dropStations as the child to the dropStationsWarehouse gameobject
        dropStationsWarehouse.name = "dropStationsWarehouse";

        if (doCornerDropStations)
        {
            nDropStations = 4;
            
            for(int c = 0; c < nDropStations; c++)
            {

                GameObject current_drop_station;
                current_drop_station = Instantiate(DropStationPrefab, new Vector3(positions[c, 0], 0.1f, positions[c, 1] ), Quaternion.identity);
                current_drop_station.name = "Drop_Station_" + c;
                current_drop_station.transform.parent = dropStationsWarehouse.transform;
                //current_drop_station.transform.Rotate(0, 90.0f, 0.0f, Space.Self);
                dropStation_objects.Add(current_drop_station);

            }
        }
        else
        {
            for (int c = 0; c < nDropStations; c++)
            {

                GameObject current_drop_station;

                current_drop_station = Instantiate(DropStationPrefab, new Vector3((c * dropStationWidth * 2) + 2, 0.1f, (ShelfRows * wallWidth)), Quaternion.identity);
                current_drop_station.name = "Drop_Station_" + c;
                current_drop_station.transform.parent = dropStationsWarehouse.transform;
                //current_drop_station.transform.Rotate(0, 90.0f, 0.0f, Space.Self);
                dropStation_objects.Add(current_drop_station);

            }
        }
        

        

       
        
        
        for (int k = 0; k < shelf_objects.Count; k++)
        {
            for (int l = 0; l < boxes_in_one_shelf; l++)
            {
                GameObject current_box;
                current_box = shelf_objects[k].transform.Find("Box_" + l).gameObject;
                current_box.name = "Box_" + ((k * boxes_in_one_shelf) + l);
                shelfBox_objects.Add(current_box);
                //Debug.Log("Current box name is is: " + current_box.name);
            }
        }
        surface.BuildNavMesh();
        // Add Robots
        for (int b = 0; b < nRobots; b++)
        {

            GameObject current_turtlebot;
            current_turtlebot = Instantiate(TurtlebotPrefab, new Vector3(((ShelfColumns) * wallWidth), wallHeight, (b * wallWidth/2)), Quaternion.identity);
            current_turtlebot.name = "Turtlebot_" + b;
            initial_robot_spawn_locations.Add(current_turtlebot.transform.position);
            turtlebot_objects.Add(current_turtlebot);
        }

        GameObject dummyGameObject = new GameObject(); // To prefill the list of current_assigned_robots_locations with dummy object in the beginning
        dummyGameObject.name = "dummyObject";
        for (int a = 0; a < nRobots; a++) 
        {
            //Debug.Log("Adding the robot location: "+a+":"+nRobots);
            current_assigned_robots_locations.Add(dummyGameObject);
            completed_boxes.Add(0);
        }
        //Debug.Log("Robot location count: "+current_assigned_robots_locations.Count);


        //Creates lists for keeping track of timing of tasks
        robotTimes = new List<double>[nRobots];
        for(int i = 0; i < nRobots; i++)
        {
            robotTimes[i] = new List<double>();
        }

        //Tracks if robots are able to be helped at a drop station
        atDropStation = new bool[nRobots];

        //Sets up distance tracking
        robotDistances = new List<double>[nRobots];
        for (int i = 0; i < nRobots; i++)
        {
            robotDistances[i] = new List<double>();
        }
    }

    //private void OnEnable()
    //{
    //     surface.BuildNavMesh(); // To avoid the errors caused due to static batching
    //}
    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Length of objects" + shelf_objects.Count);


        for (int i = 0; i < current_assigned_robots_locations.Count; i++) 
        {
           // Debug.Log(current_assigned_robots_locations[i].name+" : "+i);
        }
        checkForKeyEvents();

        doHumanRoutine();

        robotNeedsHelp();

        string result = "List contents: ";
        List<GameObject> stuff = getObjectsOfInterest();
        foreach (var item in stuff)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
        for (int i = 0; i < dropStation_objects.Count; i++)
        {
            if (stuff.Contains(dropStation_objects[i]))
            {
                viewOccurences[i]++;
            }
        }
    }


    private void checkForKeyEvents()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (current_shelf_row < ShelfRows-1) 
            {
                current_shelf_row += 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (current_shelf_row > 1)
            {
                current_shelf_row -= 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (current_shelf_col < ShelfColumns - 1)
            {
                current_shelf_col += 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (current_shelf_col > 1)
            {
                current_shelf_col -= 1;
            }
        }

        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            string objectNameToDestroy = "Shelf_" + current_shelf_row + "_" + current_shelf_col; ;
            for (int k = 0; k < shelf_objects.Count; k++)
            {
                if (shelf_objects[k].name == objectNameToDestroy) 
                {
                    Destroy(shelf_objects[k]);
                    shelf_objects.RemoveAt(k);
                }
            }
        }
    }

    public GameObject obtainRobot(int r) 
    {
        return turtlebot_objects[r];
    }

    public GameObject obtainTarget(int r) 
    {
        string current_assigned_object_name = current_assigned_robots_locations[r].name;

        if ((current_assigned_object_name == "dummyObject") && shelfBox_objects.Count > 0 && completed_boxes[r] < totalBoxesPerRobot) // dummyObject assigned at initialization if the shelfs are not empty which happens at the initialization step
        {
            int new_object_index = UnityEngine.Random.Range(0, shelfBox_objects.Count);
            current_assigned_robots_locations[r] = shelfBox_objects[new_object_index];
            shelfBox_objects.RemoveAt(new_object_index); // or shelfBox_objects.Remove(shelfBox_objects[new_object_index]);
        }
        //Debug.Log("Robot " + r + " target: Box " + current_assigned_robots_locations[r]);
        return current_assigned_robots_locations[r];
    }


    public void updateNewLocation(int r)
    {
        //Debug.Log("Current object to go to: "+current_assigned_robots_locations[r].name);
        if ((current_assigned_robots_locations[r].name).Contains("Box_")) // If the current object was a box object
        {
            //int new_drop_station_index = UnityEngine.Random.Range(0, dropStation_objects.Count);
            current_assigned_robots_locations[r].SetActive(false);
            
            GameObject turtlebot_box;
            turtlebot_box = turtlebot_objects[r].transform.Find("robot_box").gameObject;

            //Gets box number from name
            string boxNumStr = current_assigned_robots_locations[r].name.Remove(0, 4);
            int boxNum;
            int.TryParse(boxNumStr, out boxNum);

            //Changes robot box texture to match the one on the shelf
            Texture2D boxTexture;
            switch(boxNum % 4)
            {
                case 0:
                    boxTexture = (Texture2D)Resources.Load("UDS_BoxesLLogoAL_RED");
                    turtlebot_box.GetComponent<Renderer>().material.mainTexture = boxTexture;
                    break;
                case 1:
                    boxTexture = (Texture2D)Resources.Load("UDS_BoxesLLogoAL_GREEN");
                    turtlebot_box.GetComponent<Renderer>().material.mainTexture = boxTexture;
                    break;
                case 2:
                    boxTexture = (Texture2D)Resources.Load("UDS_BoxesLLogoAL_BLUE");
                    turtlebot_box.GetComponent<Renderer>().material.mainTexture = boxTexture;
                    break;
                case 3:
                    boxTexture = (Texture2D)Resources.Load("UDS_BoxesLLogoAL_YELLOW");
                    turtlebot_box.GetComponent<Renderer>().material.mainTexture = boxTexture;
                    break;
                default:
                    boxTexture = (Texture2D)Resources.Load("UDS_BoxesLLogoAL");
                    turtlebot_box.GetComponent<Renderer>().material.mainTexture = boxTexture;
                    break;
            }
            turtlebot_box.SetActive(true);

            markRobotTimeSegment(r);
            current_assigned_robots_locations[r] = getAvailableDropStation((boxNum % 4)); //dropStation_objects[new_drop_station_index];
        }
        //else if(current_assigned_robots_locations[r].name 
        //    != "dummyObject") 
        //{
            
        //}
        else 
        {
            //Debug.Log("Else Current object to go to: " + current_assigned_robots_locations[r].name);
            if (shelfBox_objects.Count > 0) // If there are no boxes left then robots are given the initial locations as dummyObjects
            {
                if (isHelpAtDropStation(r))
                {
                    markRobotTimeSegment(r);
                    atDropStation[r] = false;
                    completed_boxes[r] += 1;
                    //Debug.Log(completed_boxes[r] + " " + totalBoxesPerRobot);
                    //Debug.Log(completed_boxes[r] < totalBoxesPerRobot);

                    if(completed_boxes[r] < totalBoxesPerRobot)
                    {
                        //Debug.Log("In first");
                        int new_object_index = UnityEngine.Random.Range(0, shelfBox_objects.Count);
                        //Debug.Log("New object index: " + new_object_index + ":" + shelfBox_objects.Count);
                        current_assigned_robots_locations[r] = shelfBox_objects[new_object_index];
                        shelfBox_objects.RemoveAt(new_object_index); // or shelfBox_objects.Remove(shelfBox_objects[new_object_index]);
                        GameObject turtlebot_box;
                        turtlebot_box = turtlebot_objects[r].transform.Find("robot_box").gameObject;
                        turtlebot_box.SetActive(false);
                    }
                    else
                    {
                        //Debug.Log("In second");
                        if (current_assigned_robots_locations[r].name != "dummyObject") // Avoid creating unnecessary dummy objects 
                        {
                            //Debug.Log("In second too");
                            GameObject turtlebot_box;
                            turtlebot_box = turtlebot_objects[r].transform.Find("robot_box").gameObject;
                            turtlebot_box.SetActive(false);
                            GameObject dummyGameObject = new GameObject(); // Added dummy object which will then store the position of the intial robot spawn locations returned by getRobotInitialPosition
                            dummyGameObject.name = "dummyObject";
                            dummyGameObject.transform.position = initial_robot_spawn_locations[r];
                            current_assigned_robots_locations[r] = dummyGameObject;
                            //Debug.Log(dummyGameObject.transform.position);
                        }
                    }
                }
            }
            else
            {
                if (current_assigned_robots_locations[r].name != "dummyObject") // Avoid creating unnecessary dummy objects 
                {
                    GameObject dummyGameObject = new GameObject(); // Added dummy object which will then store the position of the intial robot spawn locations returned by getRobotInitialPosition
                    dummyGameObject.name = "dummyObject";
                    dummyGameObject.transform.position = initial_robot_spawn_locations[r];
                    current_assigned_robots_locations[r] = dummyGameObject;
                }
            }
        }
        

    }

    public Vector3 getRobotInitialPosition(int r) 
    {
        return initial_robot_spawn_locations[r];
    }

    //Checks that the human is close enough to help for a given robot
    public bool isHelpAtDropStation(int r)
    {
        if ((current_assigned_robots_locations[r].name).Contains("Drop"))
        {
            GameObject drop_station = current_assigned_robots_locations[r];
            if((Camera.transform.position.x > drop_station.transform.position.x - 2.5 && Camera.transform.position.x < drop_station.transform.position.x + 2.5) 
                && (Camera.transform.position.z > drop_station.transform.position.z - 2.5 && Camera.transform.position.z < drop_station.transform.position.z + 2.5))
            {
                return true;
            }
        }
        return false;
    }

    //Returns the closest drop station, if that station is occupied by another robot, it will choose a drop station randomly
    public GameObject getAvailableDropStation()
    {
        List<GameObject> availableStations = new List<GameObject>();
        foreach (GameObject ds in dropStation_objects)
        {
            if (!current_assigned_robots_locations.Contains(ds))
            {
                availableStations.Add(ds);
            }
        }

        if(availableStations.Count > 0)
        {
            GameObject closest = availableStations[0];
            float minDistance = Mathf.Infinity;
            foreach (GameObject ds in availableStations)
            {
                float currDistance = Vector3.Distance(ds.transform.position, Camera.transform.position);
                if (currDistance < minDistance)
                {
                    closest = ds;
                    minDistance = currDistance;
                }
            }

            return closest;
        }
        
        return dropStation_objects[UnityEngine.Random.Range(0, dropStation_objects.Count)];
    }

    //Assigns a robot a drop station given its current box's color
    public GameObject getAvailableDropStation(int num)
    {
        List<GameObject> availableStations = new List<GameObject>();

        if (doVisionAllocation)
        {
            switch (num)
            {
                case 0:
                    if (viewOccurences[0] > viewOccurences[1])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            viewOccurences[i] = 0;
                        }
                        return dropStation_objects[0];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        viewOccurences[i] = 0;
                    }
                    return dropStation_objects[1];
                    break;
                case 1:
                    if (viewOccurences[1] > viewOccurences[2])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            viewOccurences[i] = 0;
                        }
                        return dropStation_objects[1];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        viewOccurences[i] = 0;
                    }
                    return dropStation_objects[2];
                    break;
                case 2:
                    if (viewOccurences[2] > viewOccurences[3])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            viewOccurences[i] = 0;
                        }
                        return dropStation_objects[2];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        viewOccurences[i] = 0;
                    }
                    return dropStation_objects[3];
                    break;
                case 3:
                    if (viewOccurences[3] > viewOccurences[0])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            viewOccurences[i] = 0;
                        }
                        return dropStation_objects[3];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        viewOccurences[i] = 0;
                    }
                    return dropStation_objects[0];
                    break;
                default:
                    Debug.Log("Unable to assign a dropstation for vision allocation!");
                    return null;
                    break;
            }
        }
        if (!omnipotentRobots)
        {
            switch (num)
            {
                case 0:
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[0]))
                    {
                        availableStations.Add(dropStation_objects[0]);
                    }
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[1]))
                    {
                        availableStations.Add(dropStation_objects[1]);
                    }
                    break;
                case 1:
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[1]))
                    {
                        availableStations.Add(dropStation_objects[1]);
                    }
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[2]))
                    {
                        availableStations.Add(dropStation_objects[2]);
                    }
                    break;
                case 2:
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[2]))
                    {
                        availableStations.Add(dropStation_objects[2]);
                    }
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[3]))
                    {
                        availableStations.Add(dropStation_objects[3]);
                    }
                    break;
                case 3:
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[3]))
                    {
                        availableStations.Add(dropStation_objects[3]);
                    }
                    if (!current_assigned_robots_locations.Contains(dropStation_objects[0]))
                    {
                        availableStations.Add(dropStation_objects[0]);
                    }
                    break;
                default:
                    Debug.Log("Unable to assign a dropstation! Box num: " + num);
                    break;
            }

            if (availableStations.Count > 0)
            {
                GameObject closest = availableStations[0];
                float minDistance = Mathf.Infinity;
                foreach (GameObject ds in availableStations)
                {
                    float currDistance = Vector3.Distance(ds.transform.position, Camera.transform.position);
                    if (currDistance < minDistance)
                    {
                        closest = ds;
                        minDistance = currDistance;
                    }
                }

                return closest;
            }

            switch (num)
            {
                case 0:
                    return dropStation_objects[UnityEngine.Random.Range(0, 1)];
                    break;
                case 1:
                    return dropStation_objects[UnityEngine.Random.Range(1, 2)];
                    break;
                case 2:
                    return dropStation_objects[UnityEngine.Random.Range(2, 3)];
                    break;
                case 3:
                    if (UnityEngine.Random.value < .5)
                    {
                        return dropStation_objects[0];
                    }
                    return dropStation_objects[3];
                    break;
                default:
                    Debug.Log("This should never print");
                    return dropStation_objects[0];
                    break;
            }
        }
        else
        {
            switch (num)
            {
                case 0:
                    if(human_goal == 0)
                    {
                        return dropStation_objects[0];
                    }
                    else if(human_goal == 1)
                    {
                        return dropStation_objects[1];
                    }
                    else
                    {
                        return dropStation_objects[UnityEngine.Random.Range(0, 1)];
                    }
                    break;
                case 1:
                    if (human_goal == 1)
                    {
                        return dropStation_objects[1];
                    }
                    else if (human_goal == 2)
                    {
                        return dropStation_objects[2];
                    }
                    else
                    {
                        return dropStation_objects[UnityEngine.Random.Range(1, 2)];
                    }
                    break;
                case 2:
                    if (human_goal == 2)
                    {
                        return dropStation_objects[2];
                    }
                    else if (human_goal == 3)
                    {
                        return dropStation_objects[3];
                    }
                    else
                    {
                        return dropStation_objects[UnityEngine.Random.Range(2, 3)];
                    }
                    break;
                case 3:
                    if (human_goal == 3)
                    {
                        return dropStation_objects[3];
                    }
                    else if (human_goal == 0)
                    {
                        return dropStation_objects[0];
                    }
                    else
                    {
                        if (UnityEngine.Random.value < .5)
                        {
                            return dropStation_objects[0];
                        }
                        return dropStation_objects[3];
                    }
                    break;
                default:
                    Debug.Log("Unable to assign a dropstation! Box num: " + num);
                    return dropStation_objects[0];
                    break;
            }
        }
        
    }

    public void doHumanRoutine()
    {

        if(Vector3.Distance(Camera.transform.position,HumanCube.transform.position) < 2.5f)
        {
            
            human_goal = (human_goal + 1) % 4;
            HumanCube.transform.position = new Vector3(positions[human_goal, 0], 0.1f, positions[human_goal, 1]);
        }
    }

    //Adds the time since the last time was added to the timekeeping variable 
    public void markRobotTimeSegment(int robotNum)
    {
        if(robotTimes[robotNum].Count > 0)
        {
            double timeSum = 0;
            for(int i = 0; i < robotTimes[robotNum].Count; i++) 
            {
                timeSum += robotTimes[robotNum][i];
            }
            robotTimes[robotNum].Add(Time.realtimeSinceStartupAsDouble - timeSum);
        }
        else
        {
            robotTimes[robotNum].Add(Time.realtimeSinceStartupAsDouble);
        }
        //Debug.Log("Added time for Robot " + robotNum + ": " + robotTimes[robotNum][robotTimes[robotNum].Count - 1]);
    }

    //Adds robot distances to the distance keeping variable
    public void addRobotDistance(int r, double d)
    {
        robotDistances[r].Add(d);
        //Debug.Log("Robot " + r + " total distance: " + robotDistances[r]);
    }

    //Checks to see if a robot is close enough to unload a package with the human's help
    public void robotNeedsHelp()
    {
        for (int r = 0; r < nRobots; r++)
        {
            if (current_assigned_robots_locations[r].name.Contains("Drop") && obtainRobot(r).GetComponentInChildren<UnityEngine.AI.NavMeshAgent>().velocity.sqrMagnitude == 0f && nearDropStation(r)) // Check that robot has started waiting at drop station
            {
                if (!atDropStation[r])
                {
                    //Debug.Log("Robot " + r + " is ready to be helped!");
                    markRobotTimeSegment(r);
                    atDropStation[r] = true;
                }
            }

            if (current_assigned_robots_locations[r].name.Contains("dummy") && obtainRobot(r).GetComponentInChildren<UnityEngine.AI.NavMeshAgent>().velocity.sqrMagnitude == 0f && completed_boxes[r] >= totalBoxesPerRobot && nearSpawnPosition(r)) // Extra case to log when the robot is at its starting position
            {
                if (!atDropStation[r])
                {
                    //Debug.Log("Robot " + r + " is ready to be helped!");
                    markRobotTimeSegment(r);
                    atDropStation[r] = true;
                }
            }
        }
    }

    //Returns if a robot is near any drop station
    public bool nearDropStation(int r)
    {
        for(int i = 0; i < nDropStations; i++)
        {
            if(Vector3.Distance(obtainRobot(r).transform.position, dropStation_objects[i].transform.position) < 3.5)
            {
                return true;
            }
        }
        return false;
    }

    //Returns an array of the number of robots near each drop station
    public int[] getNearDropStations()
    {
        int [] robotsNear = new int[nDropStations];

        for(int i = 0; i < nDropStations; i++)
        {
            robotsNear[i] = 0;
        }

        for (int i = 0; i < nDropStations; i++)
        {
            for(int r = 0; r < nRobots; r++)
            {
                if (Vector3.Distance(obtainRobot(r).transform.position, dropStation_objects[i].transform.position) < 3.5)
                {
                    robotsNear[i]++;
                }
            }
        }

        return robotsNear;
    }

    //Checks that a given robot is near its spawn position
    public bool nearSpawnPosition(int r)
    {
        if (Vector3.Distance(obtainRobot(r).transform.position, initial_robot_spawn_locations[r]) < 1.75)
        {
            return true;
        }
        return false; 
    }

    //Obtains list of AR gameObjects that are within 5 degrees of the center of the screen given its distance from the virtual human
    public List<GameObject> getObjectsOfInterest()
    {
        List<GameObject> retList = new List<GameObject>();
        GameObject[] arObjects = GameObject.FindGameObjectsWithTag("AR"); //Gets all AR Objects in the scene
        
        foreach (GameObject arObj in arObjects)
        {
            //Calculates the angle between the camera rotation vector and the vector between the camera and the object of interest
            if(Vector3.Angle((arObj.transform.position - Camera.transform.position), Camera.transform.forward) <= 15.0f){
                retList.Add(arObj);
            }
        }

        return retList;
    }
}
