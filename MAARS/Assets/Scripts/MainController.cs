using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class MainController : MonoBehaviour
{
    Camera tablet_camera;
    private string downloaded_text = "";
    private string[] position_array;
    public Vector3 current_location_robot_red, current_location_robot_blue, current_location_robot_green;
    private string plan_text_green;
    private string plan_text_blue;
    private string plan_text_red;
    private string door_token_status;
    private string[] door_token_status_array;
    private bool show_robot_live_location = true;

    private string[] plan_poses_red;
    private string[] plan_poses_green;
    private string[] plan_poses_blue;

    private List<Vector3> lineList2Red = new List<Vector3>();
    private List<Vector3> lineList2Green = new List<Vector3>();
    private List<Vector3> lineList2Blue = new List<Vector3>();
    private List<Vector3> previouslineList2Red = new List<Vector3>();
    private List<Vector3> previouslineList2Green = new List<Vector3>();
    private List<Vector3> previouslineList2Blue = new List<Vector3>();


    private string load_status;
    public int load_status_blue_robot = 0;
    public int load_status_green_robot = 0;
    public int load_status_red_robot = 0;
    public string[] load_status_array;

    private bool andyFirstInstance = false;
    private Vector3 SpawnFloorPosition;
    private Vector3 PlaneCenterPoseDetected = new Vector3(0.0f, 0.08f, 0.0f);

    private Vector3 SpawnPositionBlue;
    private Vector3 SpawnPositionRed;
    private Vector3 SpawnPositionGreen;

    public GameObject RobotRedPrefab;
    public GameObject RobotGreenPrefab;
    public GameObject RobotBluePrefab;
    public GameObject TransparentRobotRedPrefab;
    public GameObject TransparentRobotGreenPrefab;
    public GameObject TransparentRobotBluePrefab;

    public GameObject RobotGreenObject;
    public GameObject RobotRedObject;
    public GameObject RobotBlueObject;
    private GameObject TransparentRobotGreenObject;
    private GameObject TransparentRobotRedObject;
    private GameObject TransparentRobotBlueObject;

    private Vector3 TestSpawnPosition;

    public int blue_robot_wait_token = 0;
    private int green_robot_wait_token = 1;
    private int red_robot_wait_token = 2;

    public GameObject blue_marker_prefab, red_marker_prefab, green_marker_prefab;
    private GameObject blue_marker_object, red_marker_object, green_marker_object;

    public Vector3[] marker_locations = new[] { new Vector3(7.0f, 1.0f, 1.0f), new Vector3(3.6173f, 0, 3.36497f), new Vector3(3.0643f, 0, 3.94748f) };
    private Vector3[] robot_locations_door = new[] { new Vector3(7.0f, 0, -2.0f), new Vector3(3.6173f, 0, 3.36497f), new Vector3(3.0643f, 0, 3.94748f) };

    private List<GameObject> greenArrowObjects = new List<GameObject>();
    private List<GameObject> redArrowObjects = new List<GameObject>();
    private List<GameObject> blueArrowObjects = new List<GameObject>();
    private int redraw_arrows_load = 0;

    private Vector3 LastPoint;
    private Vector3 NextPoint;

    public string tester = "testing";
    public GameObject greenArrowPrefab;
    public GameObject redArrowPrefab;
    public GameObject blueArrowPrefab;

    public GameObject targetArrowIndicator;
    private GameObject targetArrowObject;

    private GameObject arrowObject;
    private float red_robot_fraction_dist;

    private float red_robot_startTime;
    private Vector3 RedRobotStartPosn;
   

    private Vector3 red_robot_new_location;

    private int red_robot_current_movemement = 1;
    private int green_robot_current_movemement = 1;
    private int blue_robot_current_movemement = 1;
    private float green_robot_fraction_dist;
    private float green_robot_startTime;
    private Vector3 green_robot_new_location;
    private Vector3 GreenRobotStartPosn;

    
    private float blue_robot_fraction_dist;

    private float blue_robot_startTime;
    private Vector3 blue_robot_new_location;

    private Vector3 BlueRobotStartPosn;
    

    public GameObject tablet_canvas;
    public GameObject sprite_of_arrow;
    // Start is called before the first frame update

    public int previous_load_status_blue_robot = 1;
    public int previous_load_status_green_robot = 1;
    public int previous_load_status_red_robot = 1;

    private bool first_animation_red = true;
    private bool first_animation_blue = true;
    private bool first_animation_green = true;
    private GameObject big_door;
    private GameObject third_floor_walls;
    private GameObject big_door_transparent;
    private GameObject third_floor_walls_transparent;

    //private int red_robot_stop = 0;
    //private int blue_robot_stop = 0;
    //private int green_robot_stop = 0;

    private int red_robot_stop = 4;
    private int blue_robot_stop = 5;
    private int green_robot_stop = 4;

    //private int red_robot_stop = 1;
    //private int blue_robot_stop = 1;
    //private int green_robot_stop = 2;

    private float duration_of_motion_red = 1.1f;
    private float duration_of_motion_green = 1.0f;
    private float duration_of_motion_blue = 1.2f;

    //private float duration_of_motion_red = 0.1f;
    //private float duration_of_motion_green = 0.1f;
    //private float duration_of_motion_blue = 0.1f;

    private int red_robot_speed = 1;
    private int blue_robot_speed = 1;
    private int green_robot_speed = 1;

    private string ip_address = "https://buairlab.tech/";
    private bool isCoroutineExecuting = false;
    private bool isFirstFetchCycle = true;
    
    public GameObject VirtualMan;
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject VirtualHumanHead;

    public string[] red_robot_locations, blue_robot_locations, green_robot_locations;
    public int currentLocationIndex = 1;
    private string currentTurnGoingInside = "";

    void Start()
    {
        //tablet_camera = findTabletCamera();
        //tablet_canvas = GameObject.Find("Tablet_canvas");
        third_floor_walls = GameObject.Find("3rd_floor");

        big_door = GameObject.Find("Big_door");

        third_floor_walls_transparent = GameObject.Find("Type_2_wall - Transparent");

        big_door_transparent = GameObject.Find("Big_door_AR");
        Debug.Log("Wall position is :" + big_door.transform.position);

        //if (tablet_canvas != null)
        //{
        //    sprite_of_arrow = tablet_canvas.transform.GetChild(0).gameObject;
        //}
        //findChildElementsVirtualMan();




        readRobotLocations();

    }

    private void readRobotLocations()
    {
        string red_path = "Assets/Locations/robot_3_loc.txt";
        string blue_path = "Assets/Locations/robot_1_loc.txt";
        string green_path = "Assets/Locations/robot_2_loc.txt";
        string red_plan = "Assets/Locations/red_robot_plan.txt";
        string green_plan = "Assets/Locations/green_robot_plan.txt";
        string blue_plan = "Assets/Locations/blue_robot_plan.txt";

        string red_robot_location_text;
        string blue_robot_location_text;
        string green_robot_location_text;
        
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(red_path);
        red_robot_location_text = reader.ReadToEnd();
        red_robot_locations = red_robot_location_text.Split('\n');
        
        Debug.Log(reader.ReadToEnd());
        Debug.Log(red_robot_locations.Length);
        reader.Close();

        reader = new StreamReader(blue_path);
        blue_robot_location_text = reader.ReadToEnd();
        blue_robot_locations = blue_robot_location_text.Split('\n');

        Debug.Log(reader.ReadToEnd());
        Debug.Log(blue_robot_locations.Length);
        reader.Close();

        reader = new StreamReader(green_path);
        green_robot_location_text = reader.ReadToEnd();
        green_robot_locations = green_robot_location_text.Split('\n');

        Debug.Log(reader.ReadToEnd());
        Debug.Log(green_robot_locations.Length);
        reader.Close();



        reader = new StreamReader(red_plan);
        plan_text_red = reader.ReadToEnd();
        reader.Close();

        reader = new StreamReader(green_plan);
        plan_text_green = reader.ReadToEnd();
        reader.Close();

        reader = new StreamReader(blue_plan);
        plan_text_blue = reader.ReadToEnd();
        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.B))
        {
            big_door.gameObject.SetActive(false);
            big_door_transparent.gameObject.SetActive(false);
            currentTurnGoingInside = "red";
            //red_robot_stop = 0;
            //blue_robot_stop = 0;
            //green_robot_stop = 0;
        }


        if (String.Equals(currentTurnGoingInside, "red"))
        {
            red_robot_stop = 0;
        }
        else if (String.Equals(currentTurnGoingInside, "blue"))
        {
            blue_robot_stop = 0;
        }
        else if (String.Equals(currentTurnGoingInside, "green"))
        {
            green_robot_stop = 0;
        }


        StartCoroutine(ExecuteAfterTime(1));
        if (isFirstFetchCycle)
        {
            return;
        }



        if (lineList2Red.Count != 0 && lineList2Blue.Count != 0 && lineList2Green.Count != 0)
        {
            if (andyFirstInstance == false)
            {
                SpawnFloorPosition.x = 0;
                SpawnFloorPosition.y = PlaneCenterPoseDetected.y + 0.2F;
                SpawnFloorPosition.z = 0;
                SpawnPositionRed.x = lineList2Red[0].x;
                SpawnPositionRed.y = PlaneCenterPoseDetected.y;
                SpawnPositionRed.z = lineList2Red[0].z;
                SpawnPositionBlue.x = lineList2Blue[0].x;
                SpawnPositionBlue.y = PlaneCenterPoseDetected.y;
                SpawnPositionBlue.z = lineList2Blue[0].z;
                SpawnPositionGreen.x = lineList2Green[0].x;
                SpawnPositionGreen.y = PlaneCenterPoseDetected.y;
                SpawnPositionGreen.z = lineList2Green[0].z;
                RobotRedObject = Instantiate(RobotRedPrefab, SpawnPositionRed, Quaternion.identity);
                RobotGreenObject = Instantiate(RobotGreenPrefab, SpawnPositionGreen, Quaternion.identity);
                RobotBlueObject = Instantiate(RobotBluePrefab, SpawnPositionBlue, Quaternion.identity);
                TransparentRobotRedObject = Instantiate(TransparentRobotRedPrefab, SpawnPositionRed, Quaternion.identity);
                TransparentRobotGreenObject = Instantiate(TransparentRobotGreenPrefab, SpawnPositionGreen, Quaternion.identity);
                TransparentRobotBlueObject = Instantiate(TransparentRobotBluePrefab, SpawnPositionBlue, Quaternion.identity);
                SetLayerRecursively(RobotRedObject, 10);
                SetLayerRecursively(RobotGreenObject, 10);
                SetLayerRecursively(RobotBlueObject, 10);
                //SetLayerRecursively(TransparentRobotRedObject, 8);
                //SetLayerRecursively(TransparentRobotGreenObject, 8);
                //SetLayerRecursively(TransparentRobotBlueObject, 8);
                TestSpawnPosition.x = 4.47F;
                TestSpawnPosition.y = PlaneCenterPoseDetected.y + 1.5F;
                TestSpawnPosition.z = 1.96F;

                enableMarkers();
                andyFirstInstance = true;

            }
        }
        else
        {
            return;
        }
        Debug.Log("Current value is " + show_robot_live_location);
        if (show_robot_live_location == true)
        {
            TransparentRobotBlueObject.SetActive(true);
            TransparentRobotGreenObject.SetActive(true);
            TransparentRobotRedObject.SetActive(true);

            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = current_location_robot_blue.x;
            TestSpawnPosition.z = current_location_robot_blue.z;
            RobotBlueObject.transform.position = TestSpawnPosition;
            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = current_location_robot_red.x;
            TestSpawnPosition.z = current_location_robot_red.z;
            RobotRedObject.transform.position = TestSpawnPosition;
            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = current_location_robot_green.x;
            TestSpawnPosition.z = current_location_robot_green.z;
            RobotGreenObject.transform.position = TestSpawnPosition;

        }
        else
        {
            TransparentRobotBlueObject.SetActive(false);
            TransparentRobotGreenObject.SetActive(false);
            TransparentRobotRedObject.SetActive(false);
            int blue_robot_door_location = int.Parse(door_token_status_array[1]);
            int green_robot_door_location = int.Parse(door_token_status_array[2]);
            int red_robot_door_location = int.Parse(door_token_status_array[3]);

            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = robot_locations_door[blue_robot_door_location - 1].x;
            TestSpawnPosition.z = robot_locations_door[blue_robot_door_location - 1].z;
            RobotBlueObject.transform.position = TestSpawnPosition;
            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = robot_locations_door[green_robot_door_location - 1].x;
            TestSpawnPosition.z = robot_locations_door[green_robot_door_location - 1].z;
            RobotGreenObject.transform.position = TestSpawnPosition;
            TestSpawnPosition.y = PlaneCenterPoseDetected.y;
            TestSpawnPosition.x = robot_locations_door[red_robot_door_location - 1].x;
            TestSpawnPosition.z = robot_locations_door[red_robot_door_location - 1].z;
            RobotRedObject.transform.position = TestSpawnPosition;
        }

        if (previouslineList2Blue.Count != lineList2Blue.Count || previouslineList2Green.Count != lineList2Green.Count || previouslineList2Red.Count != lineList2Red.Count || redraw_arrows_load == 1)
        {
            foreach (GameObject current_arrow in blueArrowObjects)
            {
                Destroy(current_arrow);
            }
            foreach (GameObject current_arrow in greenArrowObjects)
            {
                Destroy(current_arrow);
            }
            foreach (GameObject current_arrow in redArrowObjects)
            {
                Destroy(current_arrow);
            }
            previouslineList2Red = lineList2Red.ToList();
            previouslineList2Green = lineList2Green.ToList();
            previouslineList2Blue = lineList2Blue.ToList();
            if (load_status_red_robot == 1)
            {
                for (int j = lineList2Red.Count - 2; j > 2; j = j - 2)
                {
                    LastPoint = lineList2Red[j - 1];
                    LastPoint.x = LastPoint.x;
                    LastPoint.y = PlaneCenterPoseDetected.y;
                    LastPoint.z = LastPoint.z;
                    NextPoint = lineList2Red[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    redArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(redArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    redArrowObjects.Add(arrowObject);
                    Vector3 lookHere;
                    lookHere = lineList2Red[j + 1];
                    lookHere.x = lookHere.x;
                    lookHere.y = PlaneCenterPoseDetected.y;
                    lookHere.z = lookHere.z;
                    arrowObject.transform.LookAt(lookHere);
                    arrowObject.transform.Rotate(90, 0, 0);

                }
            }
            else
            {
                for (int j = 1; j < lineList2Red.Count - 2; j = j + 2)
                {

                    LastPoint = lineList2Red[j - 1];
                    LastPoint.x = LastPoint.x;
                    LastPoint.y = PlaneCenterPoseDetected.y;
                    LastPoint.z = LastPoint.z;
                    NextPoint = lineList2Red[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    redArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(redArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    redArrowObjects.Add(arrowObject);
                    if (j + 1 < lineList2Red.Count)
                    {
                        Vector3 lookHere;
                        lookHere = lineList2Red[j - 1];
                        lookHere.x = lookHere.x;
                        lookHere.y = PlaneCenterPoseDetected.y;
                        lookHere.z = lookHere.z;
                        arrowObject.transform.LookAt(lookHere);
                        arrowObject.transform.Rotate(90, 0, 0);
                    }
                }
            }


            if (load_status_green_robot == 1)
            {
                for (int j = lineList2Green.Count - 3; j > 2; j = j - 2)
                {
                    NextPoint = lineList2Green[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    greenArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(greenArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    greenArrowObjects.Add(arrowObject);
                    Vector3 lookHere;
                    lookHere = lineList2Green[j + 1];
                    lookHere.x = lookHere.x;
                    lookHere.y = PlaneCenterPoseDetected.y;
                    lookHere.z = lookHere.z;
                    arrowObject.transform.LookAt(lookHere);
                    arrowObject.transform.Rotate(90, 0, 0);

                }
            }
            else
            {
                for (int j = 2; j < lineList2Green.Count - 2; j = j + 2)
                {
                    NextPoint = lineList2Green[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    greenArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(greenArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    greenArrowObjects.Add(arrowObject);
                    if (j + 1 < lineList2Green.Count)
                    {
                        Vector3 lookHere;
                        lookHere = lineList2Green[j - 1];
                        lookHere.x = lookHere.x;
                        lookHere.y = PlaneCenterPoseDetected.y;
                        lookHere.z = lookHere.z;
                        arrowObject.transform.LookAt(lookHere);
                        arrowObject.transform.Rotate(90, 0, 0);
                    }
                }
            }




            if (load_status_blue_robot == 1)
            {
                for (int j = lineList2Blue.Count - 2; j > 2; j = j - 2)
                {
                    NextPoint = lineList2Blue[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    blueArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(blueArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    blueArrowObjects.Add(arrowObject);
                    Vector3 lookHere;
                    lookHere = lineList2Blue[j + 1];
                    lookHere.x = lookHere.x;
                    lookHere.y = PlaneCenterPoseDetected.y;
                    lookHere.z = lookHere.z;
                    arrowObject.transform.LookAt(lookHere);
                    arrowObject.transform.Rotate(90, 0, 0);

                }

            }
            else
            {
                for (int j = 1; j < lineList2Blue.Count - 2; j = j + 2)
                {
                    NextPoint = lineList2Blue[j];
                    NextPoint.x = NextPoint.x;
                    NextPoint.y = PlaneCenterPoseDetected.y;
                    NextPoint.z = NextPoint.z;
                    blueArrowPrefab.SetActive(true);
                    arrowObject = Instantiate(blueArrowPrefab, NextPoint, Quaternion.identity);
                    arrowObject.layer = 8;
                    blueArrowObjects.Add(arrowObject);
                    if (j + 1 < lineList2Blue.Count)
                    {
                        Vector3 lookHere;
                        lookHere = lineList2Blue[j - 1];
                        lookHere.x = lookHere.x;
                        lookHere.y = PlaneCenterPoseDetected.y;
                        lookHere.z = lookHere.z;
                        arrowObject.transform.LookAt(lookHere);
                        arrowObject.transform.Rotate(90, 0, 0);
                    }
                }
            }
        }


        if (load_status_red_robot == 1)
        {
            red_robot_fraction_dist = (Time.time - red_robot_startTime) / duration_of_motion_red;
            TransparentRobotRedObject.transform.LookAt(red_robot_new_location);
            RedRobotStartPosn = TransparentRobotRedObject.transform.position;
            TransparentRobotRedObject.transform.position = Vector3.Slerp(RedRobotStartPosn, red_robot_new_location, red_robot_fraction_dist);
            if (red_robot_fraction_dist >= 1)
            {
                red_robot_startTime = Time.time;
                RedRobotStartPosn = red_robot_new_location;
                red_robot_current_movemement = red_robot_current_movemement + red_robot_speed;
                if (red_robot_current_movemement < lineList2Red.Count - 1)
                {
                    red_robot_new_location = lineList2Red[red_robot_current_movemement];
                    red_robot_new_location.x = red_robot_new_location.x;
                    red_robot_new_location.y = PlaneCenterPoseDetected.y;
                    red_robot_new_location.z = red_robot_new_location.z;
                }
                else
                {
                    load_status_red_robot = 0;
                    lineList2Red.Reverse();
                    redraw_arrows_load = 1;
                    //red_robot_current_movemement = 1;
                    //red_robot_new_location = lineList2Red[red_robot_current_movemement];
                    //red_robot_new_location.x = red_robot_new_location.x;
                    //red_robot_new_location.y = PlaneCenterPoseDetected.y;
                    //red_robot_new_location.z = red_robot_new_location.z;
                }

            }

        }
        else
        {
            red_robot_fraction_dist = (Time.time - red_robot_startTime) / duration_of_motion_red;
            
            TransparentRobotRedObject.transform.LookAt(red_robot_new_location);
            RedRobotStartPosn = TransparentRobotRedObject.transform.position;
            TransparentRobotRedObject.transform.position = Vector3.Slerp(RedRobotStartPosn, red_robot_new_location, red_robot_fraction_dist);
            RobotRedObject.transform.LookAt(red_robot_new_location);
            RobotRedObject.transform.position = Vector3.Slerp(RedRobotStartPosn, red_robot_new_location, red_robot_fraction_dist);
            if (red_robot_fraction_dist >= 1)
            {
                red_robot_startTime = Time.time;
                RedRobotStartPosn = red_robot_new_location;
                
                if (red_robot_current_movemement > red_robot_stop)
                {
                    red_robot_current_movemement = red_robot_current_movemement - red_robot_speed;
                    red_robot_new_location = lineList2Red[red_robot_current_movemement];
                    red_robot_new_location.x = red_robot_new_location.x;
                    red_robot_new_location.y = PlaneCenterPoseDetected.y;
                    red_robot_new_location.z = red_robot_new_location.z;
                }
                else
                {

                    if ((String.Equals(currentTurnGoingInside, "red")) && (red_robot_stop == 0))
                    {
                        currentTurnGoingInside = "green";
                    }
                    
                    first_animation_red = false;
                    if (first_animation_red == true)
                    {
                        red_robot_current_movemement = lineList2Red.Count - 1;
                        red_robot_new_location = lineList2Red[red_robot_current_movemement];
                        red_robot_new_location.x = red_robot_new_location.x;
                        red_robot_new_location.y = PlaneCenterPoseDetected.y;
                        red_robot_new_location.z = red_robot_new_location.z;
                        first_animation_red = false;
                    }

                }

            }
        }
        if (load_status_green_robot == 1)
        {
            green_robot_fraction_dist = (Time.time - green_robot_startTime) / duration_of_motion_green;
            TransparentRobotGreenObject.transform.LookAt(green_robot_new_location);
            GreenRobotStartPosn = TransparentRobotGreenObject.transform.position;
            TransparentRobotGreenObject.transform.position = Vector3.Slerp(GreenRobotStartPosn, green_robot_new_location, green_robot_fraction_dist);
            if (green_robot_fraction_dist >= 1)
            {
                green_robot_startTime = Time.time;
                GreenRobotStartPosn = green_robot_new_location;
                green_robot_current_movemement = green_robot_current_movemement + green_robot_speed;
                if (green_robot_current_movemement < lineList2Green.Count - 1)
                {
                    green_robot_new_location = lineList2Green[green_robot_current_movemement];
                    green_robot_new_location.x = green_robot_new_location.x;
                    green_robot_new_location.y = PlaneCenterPoseDetected.y;
                    green_robot_new_location.z = green_robot_new_location.z;
                }
                else
                {
                    lineList2Blue.Reverse();
                    load_status_green_robot = 0;                    
                    redraw_arrows_load = 1;
                    //green_robot_current_movemement = 1;
                    //green_robot_new_location = lineList2Green[green_robot_current_movemement];
                    //green_robot_new_location.x = green_robot_new_location.x;
                    //green_robot_new_location.y = PlaneCenterPoseDetected.y;
                    //green_robot_new_location.z = green_robot_new_location.z;
                }

            }
        }
        else
        {
            green_robot_fraction_dist = (Time.time - green_robot_startTime) / duration_of_motion_green;
            TransparentRobotGreenObject.transform.LookAt(green_robot_new_location);
            GreenRobotStartPosn = TransparentRobotGreenObject.transform.position;
            TransparentRobotGreenObject.transform.position = Vector3.Slerp(GreenRobotStartPosn, green_robot_new_location, green_robot_fraction_dist);

            RobotGreenObject.transform.LookAt(green_robot_new_location);
            RobotGreenObject.transform.position = Vector3.Slerp(GreenRobotStartPosn, green_robot_new_location, green_robot_fraction_dist);
            if (green_robot_fraction_dist >= 1)
            {
                green_robot_startTime = Time.time;
                GreenRobotStartPosn = green_robot_new_location;
                if (green_robot_current_movemement > green_robot_stop)
                {
                    green_robot_current_movemement = green_robot_current_movemement - green_robot_speed;
                    green_robot_new_location = lineList2Green[green_robot_current_movemement];
                    green_robot_new_location.x = green_robot_new_location.x;
                    green_robot_new_location.y = PlaneCenterPoseDetected.y;
                    green_robot_new_location.z = green_robot_new_location.z;
                }
                else
                {
                    if ((String.Equals(currentTurnGoingInside, "green")) && (green_robot_stop == 0))
                    {
                        currentTurnGoingInside = "blue";
                    }
                    first_animation_green = false;
                    if (first_animation_green == true)
                    {
                        green_robot_current_movemement = lineList2Green.Count - 1;
                        green_robot_new_location = lineList2Green[green_robot_current_movemement];
                        green_robot_new_location.x = green_robot_new_location.x;
                        green_robot_new_location.y = PlaneCenterPoseDetected.y;
                        green_robot_new_location.z = green_robot_new_location.z;
                    }
                    first_animation_green = false;

                }

            }
        }
        if (load_status_blue_robot == 1)
        {
            blue_robot_fraction_dist = (Time.time - blue_robot_startTime) / duration_of_motion_blue;
            TransparentRobotBlueObject.transform.LookAt(blue_robot_new_location);
            BlueRobotStartPosn = TransparentRobotBlueObject.transform.position;
            TransparentRobotBlueObject.transform.position = Vector3.Slerp(BlueRobotStartPosn, blue_robot_new_location, blue_robot_fraction_dist);
            if (blue_robot_fraction_dist >= 1)
            {
                blue_robot_startTime = Time.time;
                BlueRobotStartPosn = blue_robot_new_location;
                blue_robot_current_movemement = blue_robot_current_movemement + blue_robot_speed;
                if (blue_robot_current_movemement < lineList2Blue.Count - 1)
                {
                    blue_robot_new_location = lineList2Blue[blue_robot_current_movemement];
                    blue_robot_new_location.x = blue_robot_new_location.x;
                    blue_robot_new_location.y = PlaneCenterPoseDetected.y;
                    blue_robot_new_location.z = blue_robot_new_location.z;
                }
                else
                {
                    load_status_blue_robot = 0;
                    lineList2Green.Reverse();
                    redraw_arrows_load = 1;
                    //blue_robot_current_movemement = 1;
                    //blue_robot_new_location = lineList2Blue[blue_robot_current_movemement];
                    //blue_robot_new_location.x = blue_robot_new_location.x;
                    //blue_robot_new_location.y = PlaneCenterPoseDetected.y;
                    //blue_robot_new_location.z = blue_robot_new_location.z;
                }

            }
        }
        else
        {
            blue_robot_fraction_dist = (Time.time - blue_robot_startTime) / duration_of_motion_blue;
            TransparentRobotBlueObject.transform.LookAt(blue_robot_new_location);
            BlueRobotStartPosn = TransparentRobotBlueObject.transform.position;
            TransparentRobotBlueObject.transform.position = Vector3.Slerp(BlueRobotStartPosn, blue_robot_new_location, blue_robot_fraction_dist);
            RobotBlueObject.transform.LookAt(blue_robot_new_location);
            RobotBlueObject.transform.position = Vector3.Slerp(BlueRobotStartPosn, blue_robot_new_location, blue_robot_fraction_dist);
            if (blue_robot_fraction_dist >= 1)
            {
                blue_robot_startTime = Time.time;
                BlueRobotStartPosn = blue_robot_new_location;
                if (blue_robot_current_movemement > blue_robot_stop)
                {
                    blue_robot_current_movemement = blue_robot_current_movemement - blue_robot_speed;
                    blue_robot_new_location = lineList2Blue[blue_robot_current_movemement];
                    blue_robot_new_location.x = blue_robot_new_location.x;
                    blue_robot_new_location.y = PlaneCenterPoseDetected.y;
                    blue_robot_new_location.z = blue_robot_new_location.z;
                }
                else
                {
                    first_animation_blue = false;
                    if (first_animation_blue == true)
                    {
                        blue_robot_current_movemement = lineList2Blue.Count - 1;
                        blue_robot_new_location = lineList2Blue[blue_robot_current_movemement];
                        blue_robot_new_location.x = blue_robot_new_location.x;
                        blue_robot_new_location.y = PlaneCenterPoseDetected.y;
                        blue_robot_new_location.z = blue_robot_new_location.z;
                    }
                    first_animation_blue = false;

                }
            }
        }

        //checkObjectVisibility();
        //RobotRedObject.layer = 8;
        //RobotGreenObject.layer = 8;
        //RobotBlueObject.layer = 8;
        //TransparentRobotRedObject.layer = 8;
        //TransparentRobotGreenObject.layer = 8;
        //TransparentRobotBlueObject.layer = 8;
        //RobotRedObject.SetActive(false);
        //RobotGreenObject.SetActive(false);
        //TransparentRobotRedObject.SetActive(false);
        //TransparentRobotBlueObject.SetActive(false);
        //TransparentRobotGreenObject.SetActive(false);
        //RobotBlueObject.SetActive(false);
    }

    private void checkObjectVisibility()
    {

        Vector3 object_postion = tablet_camera.WorldToViewportPoint(RobotRedObject.transform.position);
        Vector3 screen_position = tablet_camera.WorldToScreenPoint(RobotRedObject.transform.position);
        //if (object_postion.x >= 0 && object_postion.x <= 1 && object_postion.y >= 0 && object_postion.y <= 1 && object_postion.z > 0)
        //{
        //    sprite_of_arrow.SetActive(false);         
        //}
        //else 
        //{

        //    if (screen_position.x < Screen.width / 2) {

        //        sprite_of_arrow.SetActive(true);
        //        sprite_of_arrow.transform.localPosition = new Vector3(0.47f, 0f, 1f);
        //        sprite_of_arrow.transform.localRotation = Quaternion.Euler(0, 0, -90);
        //    }
        //    else
        //    {             
        //        sprite_of_arrow.SetActive(true);
        //        sprite_of_arrow.transform.localPosition = new Vector3(-0.47f, 0f, 1f);
        //        sprite_of_arrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
        //    }
        //}

    }

    IEnumerator getLoadStatus()
    {
        UnityWebRequest www = UnityWebRequest.Get(ip_address + "load_token.txt");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            load_status = www.downloadHandler.text;
            load_status_array = load_status.Split('\n');
            load_status_blue_robot = int.Parse(load_status_array[0]);
            load_status_green_robot = int.Parse(load_status_array[1]);
            load_status_red_robot = int.Parse(load_status_array[2]);
        }
    }

    private void check_load_status()
    {
        if (previous_load_status_blue_robot != load_status_blue_robot)
        {
            //previous_load_status_blue_robot = load_status_blue_robot;
            //Debug.Log("New value for load blue");
            //List<Vector3> tempBlueLineArray = new List<Vector3>();
            //Vector3 temp_vect3;
            //for (int i = 0; i < lineList2Blue.Count / 2; i++)
            //{
            //    temp_vect3 = lineList2Blue[i];
            //    lineList2Blue[i] = lineList2Blue[lineList2Blue.Count - i - 1];
            //    lineList2Blue[lineList2Blue.Count - i - 1] = temp_vect3;
            //}
            redraw_arrows_load = 1;
            lineList2Blue.Reverse();
            Debug.Log("New value for load blue");
            previous_load_status_blue_robot = load_status_blue_robot;
        }
        if (load_status_green_robot != previous_load_status_green_robot)
        {
            redraw_arrows_load = 1;
            lineList2Green.Reverse();
            Debug.Log("New value for load green");
            previous_load_status_green_robot = load_status_green_robot;
        }
        if (load_status_red_robot != previous_load_status_red_robot)
        {
            redraw_arrows_load = 1;
            lineList2Red.Reverse();
            Debug.Log("New value for load red");
            previous_load_status_red_robot = load_status_red_robot;
        }
    }

    //IEnumerator Get_Current_Location()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(ip_address+"current_position_red_robot.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        downloaded_text = www.downloadHandler.text;

    //        position_array = downloaded_text.Split(',');
    //        if (position_array.Length > 1)
    //        {
    //            current_location_robot_red.x = float.Parse(position_array[0]);
    //            current_location_robot_red.z = float.Parse(position_array[1]);
    //            current_location_robot_red.x = (float)(Math.Truncate((double)current_location_robot_red.x * 10.0) / 10.0);
    //            current_location_robot_red.z = (float)(Math.Truncate((double)current_location_robot_red.z * 10.0) / 10.0);
    //            byte[] results = www.downloadHandler.data;
    //        }
    //    }
    //    www = UnityWebRequest.Get(ip_address+"current_position_blue_robot.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        downloaded_text = www.downloadHandler.text;
    //        position_array = downloaded_text.Split(',');
    //        if (position_array.Length > 1)
    //        {
    //            if (!float.TryParse(position_array[1], out current_location_robot_blue.z) )
    //            {
    //                Debug.Log("The number input string was " + downloaded_text);
    //                Debug.Log("The number input was "+ position_array.ToString());
    //                Debug.Log("The number input was 0 :" + position_array[0]);
    //                Debug.Log("The number input was 1 :" + position_array[1]);
    //            }
    //            current_location_robot_blue.x = float.Parse(position_array[0]);
    //            //current_location_robot_blue.z = float.Parse(position_array[1]);
    //            current_location_robot_blue.x = (float)(Math.Truncate((double)current_location_robot_blue.x * 10.0) / 10.0);
    //            current_location_robot_blue.z = (float)(Math.Truncate((double)current_location_robot_blue.z * 10.0) / 10.0);
    //            byte[] results = www.downloadHandler.data;
    //        }
    //    }
    //    www = UnityWebRequest.Get(ip_address+"current_position_green_robot.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        downloaded_text = www.downloadHandler.text;

    //        position_array = downloaded_text.Split(',');
    //        if (position_array.Length > 1)
    //        {
    //            current_location_robot_green.x = float.Parse(position_array[0]);
    //            current_location_robot_green.z = float.Parse(position_array[1]);
    //            current_location_robot_green.x = (float)(Math.Truncate((double)current_location_robot_green.x * 10.0) / 10.0);
    //            current_location_robot_green.z = (float)(Math.Truncate((double)current_location_robot_green.z * 10.0) / 10.0);
    //            byte[] results = www.downloadHandler.data;
    //        }
    //    }

    //}
    //IEnumerator getPlanRed()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(ip_address+"entire_plan_robot_red.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        plan_text_red = www.downloadHandler.text;

    //        // Or retrieve results as binary data
    //        byte[] results = www.downloadHandler.data;
    //    }
    //}

    //IEnumerator getPlanBlue()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(ip_address+"entire_plan_robot_blue.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        plan_text_blue = www.downloadHandler.text;

    //    }
    //}

    //IEnumerator getPlanGreen()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(ip_address+"entire_plan_robot_green.txt");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        plan_text_green = www.downloadHandler.text;

    //    }
    //}


    IEnumerator Get_Current_Location()
    {
        UnityWebRequest www = UnityWebRequest.Get(ip_address + "current_position_red_robot.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            downloaded_text = www.downloadHandler.text;

            position_array = downloaded_text.Split(',');
            if (position_array.Length > 1)
            {
                current_location_robot_red.x = float.Parse(position_array[0]);
                current_location_robot_red.z = float.Parse(position_array[1]);
                current_location_robot_red.x = (float)(Math.Truncate((double)current_location_robot_red.x * 10.0) / 10.0);
                current_location_robot_red.z = (float)(Math.Truncate((double)current_location_robot_red.z * 10.0) / 10.0);
                byte[] results = www.downloadHandler.data;
            }
        }
        www = UnityWebRequest.Get(ip_address + "current_position_blue_robot.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            downloaded_text = www.downloadHandler.text;
            position_array = downloaded_text.Split(',');
            if (position_array.Length > 1)
            {
                if (!float.TryParse(position_array[1], out current_location_robot_blue.z))
                {
                    Debug.Log("The number input string was " + downloaded_text);
                    Debug.Log("The number input was " + position_array.ToString());
                    Debug.Log("The number input was 0 :" + position_array[0]);
                    Debug.Log("The number input was 1 :" + position_array[1]);
                }
                current_location_robot_blue.x = float.Parse(position_array[0]);
                //current_location_robot_blue.z = float.Parse(position_array[1]);
                current_location_robot_blue.x = (float)(Math.Truncate((double)current_location_robot_blue.x * 10.0) / 10.0);
                current_location_robot_blue.z = (float)(Math.Truncate((double)current_location_robot_blue.z * 10.0) / 10.0);
                byte[] results = www.downloadHandler.data;
            }
        }
        www = UnityWebRequest.Get(ip_address + "current_position_green_robot.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            downloaded_text = www.downloadHandler.text;

            position_array = downloaded_text.Split(',');
            if (position_array.Length > 1)
            {
                current_location_robot_green.x = float.Parse(position_array[0]);
                current_location_robot_green.z = float.Parse(position_array[1]);
                current_location_robot_green.x = (float)(Math.Truncate((double)current_location_robot_green.x * 10.0) / 10.0);
                current_location_robot_green.z = (float)(Math.Truncate((double)current_location_robot_green.z * 10.0) / 10.0);
                byte[] results = www.downloadHandler.data;
            }
        }

    }
    IEnumerator getPlanRed()
    {
        //string plan_url = ip_address + "/ar_demo/entire_plan_robot_red.txt";
        string plan_url = "https://buairlab.tech/entire_plan_robot_red.txt";

        Debug.Log("ARN is trying to get plan red: " + plan_url);
        WWW w = new WWW("https://buairlab.tech/entire_plan_robot_red.txt");

        yield return w;
        if (w.error != null)
        {
            Debug.Log("ARN Error .. " + w.error);
            // for example, often 'Error .. 404 Not Found'
        }
        else
        {
            Debug.Log("ARN Found ... ==>" + w.text + "<==");
            if (w.text != "")
            {
                plan_text_red = w.text;
            }


        }
    }

    IEnumerator getPlanBlue()
    {
        string plan_url = "https://buairlab.tech/entire_plan_robot_blue.txt";

        Debug.Log("ARN is trying to get plan red: " + plan_url);
        WWW w = new WWW("https://buairlab.tech/entire_plan_robot_blue.txt");

        yield return w;
        if (w.error != null)
        {
            Debug.Log("ARN Error .. " + w.error);
            // for example, often 'Error .. 404 Not Found'
        }
        else
        {
            Debug.Log("ARN Found ... ==>" + w.text + "<==");
            plan_text_blue = w.text;

        }
    }

    IEnumerator getPlanGreen()
    {
        string plan_url = "https://buairlab.tech/entire_plan_robot_green.txt";

        Debug.Log("ARN is trying to get plan red: " + plan_url);
        WWW w = new WWW("https://buairlab.tech/entire_plan_robot_green.txt");

        yield return w;
        if (w.error != null)
        {
            Debug.Log("ARN Error .. " + w.error);
            // for example, often 'Error .. 404 Not Found'
        }
        else
        {
            Debug.Log("ARN Found ... ==>" + w.text + "<==");
            plan_text_green = w.text;

        }
    }
    IEnumerator get_door_token()
    {
        UnityWebRequest www = UnityWebRequest.Get(ip_address + "door_tokens.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            door_token_status = www.downloadHandler.text;
            door_token_status_array = door_token_status.Split('\n');
            if (int.Parse(door_token_status_array[0]) == 1)
            {
                show_robot_live_location = false;
            }
            else
            {
                show_robot_live_location = true;
            }
        }
    }


    void readPlanFileRed()
    {
        //plan_poses = PlanFile.text.Split('\n');
        if (String.IsNullOrEmpty(plan_text_red))
        {
            return;
        }
        Debug.Log("File is not empty: " + plan_text_red);
        plan_poses_red = plan_text_red.Split('\n');
        lineList2Red.Clear();
        for (int j = 0; j < plan_poses_red.Length; j++)
        {
            string[] posePoints = System.Text.RegularExpressions.Regex.Split(plan_poses_red[j], @"\s{2,}");
            if (plan_poses_red[j] == "")
            {
                continue;
            }
            float x_coordinate = float.Parse(posePoints[0]);
            float z_coordinate = float.Parse(posePoints[1]);
            lineList2Red.Add(new Vector3(x_coordinate, 0.0F, z_coordinate));
        }


    }

    void readPlanFileBlue()
    {
        if (String.IsNullOrEmpty(plan_text_blue))
        {
            return;
        }
        plan_poses_blue = plan_text_blue.Split('\n');
        lineList2Blue.Clear();
        for (int j = 0; j < plan_poses_blue.Length; j++)
        {
            string[] posePoints = System.Text.RegularExpressions.Regex.Split(plan_poses_blue[j], @"\s{2,}");
            if (plan_poses_blue[j] == "")
            {
                continue;
            }
            float x_coordinate = float.Parse(posePoints[0]);
            float z_coordinate = float.Parse(posePoints[1]);

            lineList2Blue.Add(new Vector3(x_coordinate, 0.0F, z_coordinate));
        }
    }

    void readPlanFileGreen()
    {
        //plan_poses = PlanFile.text.Split('\n');
        if (String.IsNullOrEmpty(plan_text_green))
        {
            return;
        }
        plan_poses_green = plan_text_green.Split('\n');
        lineList2Green.Clear();
        for (int j = 0; j < plan_poses_green.Length; j++)
        {
            string[] posePoints = System.Text.RegularExpressions.Regex.Split(plan_poses_green[j], @"\s{2,}");
            if (plan_poses_green[j] == "")
            {
                continue;
            }
            float x_coordinate = float.Parse(posePoints[0]);
            float z_coordinate = float.Parse(posePoints[1]);

            lineList2Green.Add(new Vector3(x_coordinate, 0.0F, z_coordinate));
        }
    }

    private void enableMarkers()
    {
        if (blue_robot_wait_token >= 0)
        {
            blue_marker_prefab.SetActive(false);
            blue_marker_object = Instantiate(blue_marker_prefab, marker_locations[blue_robot_wait_token], Quaternion.identity);
            blue_marker_object.transform.localScale = new Vector3(0.3F, 0.3F, 0.3F);
            blue_marker_object.layer = 8;

        }
        else
        {
            blue_marker_prefab.SetActive(false);
        }

    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public Camera findTabletCamera()
    {
        foreach (Camera c in Camera.allCameras)
        {
            if (c.gameObject.name == "GUICamera")
            {
                return c;
            }
        }
        return Camera.main;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        //StartCoroutine(getPlanRed());
        //StartCoroutine(getPlanBlue());
        //StartCoroutine(getPlanGreen());
        //StartCoroutine(get_door_token());
        //StartCoroutine(Get_Current_Location());
        findRobotsCurrentLocation();
        readPlanFileRed();
        readPlanFileBlue();
        readPlanFileGreen();
        //StartCoroutine(getLoadStatus());
        //check_load_status();
        isCoroutineExecuting = false;
        isFirstFetchCycle = false;
    }

    

    private void findRobotsCurrentLocation()
    {
        string red_robot_loc = red_robot_locations[currentLocationIndex];
        string blue_robot_loc = blue_robot_locations[currentLocationIndex];
        string green_robot_loc = green_robot_locations[currentLocationIndex];

        position_array = red_robot_loc.Split(',');
        if (position_array.Length > 1)
        {
            current_location_robot_red.x = float.Parse(position_array[0]);
            current_location_robot_red.z = float.Parse(position_array[1]);
            current_location_robot_red.x = (float)(Math.Truncate((double)current_location_robot_red.x * 10.0) / 10.0);
            current_location_robot_red.z = (float)(Math.Truncate((double)current_location_robot_red.z * 10.0) / 10.0);

        }

        position_array = blue_robot_loc.Split(',');
        if (position_array.Length > 1)
        {
            current_location_robot_blue.x = float.Parse(position_array[0]);
            current_location_robot_blue.z = float.Parse(position_array[1]);
            current_location_robot_blue.x = (float)(Math.Truncate((double)current_location_robot_blue.x * 10.0) / 10.0);
            current_location_robot_blue.z = (float)(Math.Truncate((double)current_location_robot_blue.z * 10.0) / 10.0);

        }

        position_array = green_robot_loc.Split(',');
        if (position_array.Length > 1)
        {
            current_location_robot_green.x = float.Parse(position_array[0]);
            current_location_robot_green.z = float.Parse(position_array[1]);
            current_location_robot_green.x = (float)(Math.Truncate((double)current_location_robot_green.x * 10.0) / 10.0);
            current_location_robot_green.z = (float)(Math.Truncate((double)current_location_robot_green.z * 10.0) / 10.0);

        }

        Debug.Log("Green robot plan has :" + lineList2Green.Count);
        Debug.Log("Red robot plan has :" + lineList2Red.Count);
        Debug.Log("Blue robot plan has :" + lineList2Blue.Count);


    }

    private Transform FindDeepChild(Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    private void findChildElementsVirtualMan() 
    {
        GameObject current_arm;
        VirtualMan = GameObject.Find("SittingMan");
        current_arm = VirtualMan.transform.Find("Shoulder_L").gameObject;
        foreach (Transform child in VirtualMan.transform)
        {
            Debug.Log("Child name is: " + child.name);
            if (child.name == "Group")
            {
                foreach (Transform group_child in child.transform)
                {
                    Debug.Log("Group Child name is: " + group_child.name);
                }
            }
        }
    }

    



}
