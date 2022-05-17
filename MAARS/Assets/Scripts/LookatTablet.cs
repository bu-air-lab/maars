using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System.Linq;
public class LookatTablet : MonoBehaviour
{
    GameObject tablet;
    tab_area_rotate tabletRotateScript;
    GameObject tablet_script;

    GameObject MainController;
    public GameObject RedRobot;

    MainController mainControllerScript;

    Camera tablet_camera;
    public Vector3 point = new Vector3(0, 0, 0);
    Transform original_headpose;
    int speed = 2;
    Quaternion targetRotation;
    bool first = true;
    GameObject unity_logo;
    GameObject cube_table;
    // Start is called before the first frame update
    GameObject target_object;
    Dictionary<string, float> object_angles = new Dictionary<string, float>();

    float probability_straight = 0.6f;
    float probability_right = 0.2f;
    float probability_left = 0.2f;
    bool rotate = false;
    bool first_trial = true;
    bool rotate_left = false;
    bool rotate_right = true;
    float number;
    bool target_reached = false;
    bool tablet_rotate = false;
    string tablet_rotate_side = "none";
    private float nextActionTime = 5.0f;
    public float period = 200.0f;
    private string object_looking = "unity_logo";
    private float tabletRotateTime = 2.0f;
    private float starttabletRotateTime = 2.0f;
    private float tablet_left_rotate_probability = 0.5f;

    public float smooth = 1f;
    private Quaternion targetRotationNew;

    public float tablet_looking_angle_min, tablet_looking_angle_max, unity_logo_looking_angle_min, unity_logo_looking_angle_max;

    public float human_task_completion_time = 0.0f;
    public float human_task_completion_time_rand;
    public int experiment_number = 0;
    public int times_door_opened = 0;
    float human_task_allocation_time = 0;
    public Vector3 redRobotPos, blueRobotPos, greenRobotPos;
    private int task_status_robots;
    private bool wait_flag_robots = true;
    public float waitFlagCheckTime = 0.0f;
    void Start()
    {
        tablet_camera = findTabletCamera();
        unity_logo = GameObject.Find("Unity_logo");
        cube_table = GameObject.Find("Left_Object");
        original_headpose = transform.transform;
        //rotateLeft();
        //rotateRight();
        object_angles.Add("Left_Object",50.0f);
        object_angles.Add("GUICamera", 50.0f);
        object_angles.Add("Unity_logo", 50.0f);

        tablet = GameObject.Find("Tab_area");
        tabletRotateScript = tablet.GetComponent<tab_area_rotate>();
        targetRotationNew = transform.rotation;

        MainController = GameObject.Find("MainController"); 
        mainControllerScript = MainController.GetComponent<MainController>();  
        //RedRobot = mainControllerScript.RobotRedObject;
        //RedRobot = GameObject.FindGameObjectWithTag("MainController").GetComponent<MainController>().RobotRedObject;
        //Vector3 rpos = RedRobot.transform.position;
        tablet_looking_angle_min = 240.0f;
        tablet_looking_angle_max = 230.0f;
        unity_logo_looking_angle_min = 151.0f;
        unity_logo_looking_angle_max = 147.763f;
        rotate_right = true;

        experiment_number = 0;
        wait_flag_robots = true;
    }

    void Update()
    {
        
        int robots_at_door = 0;
        redRobotPos = mainControllerScript.current_location_robot_red;
        blueRobotPos = mainControllerScript.current_location_robot_blue;
        greenRobotPos = mainControllerScript.current_location_robot_green;
        Debug.Log("My position is b:" + blueRobotPos);
        Debug.Log("My position is g:" + greenRobotPos);
        Debug.Log("My position is r:" + redRobotPos);
        if (Time.time > human_task_completion_time) 
        {
            if (experiment_number != 0 && wait_flag_robots)
            {
                if (Time.time > waitFlagCheckTime)
                {
                    Debug.Log("Waiting for robots to complete tasks");
                    StartCoroutine(getRobotTaskStatus());
                    waitFlagCheckTime = Time.time + 3;
                }

            }
            else 
            {
                reassign_task_human();
            }
            //while (experiment_number == 0 || wait_flag_robots)
            //{
            //    Debug.Log("Waiting for robots to complete tasks");
            //    //StartCoroutine(getRobotTaskStatus());
            //}
            //reassign_task_human();
            return;
        }
        //if (Time.time > nextActionTime)
        //{
        //    Debug.Log("Current in time cycle ");
        //    if (rotate_right == true)
        //    {
        //        rotate_right = false;
        //        rotate_left = true;
        //    }
        //    else
        //    {
        //        rotate_left = false;
        //        rotate_right = true;
        //    }
        //    nextActionTime = Time.time + 10;
        //}
        if (Input.GetKeyDown(KeyCode.B))
        {
            rotate_left = false;
            rotate_right = false;
        }
        if (rotate_left == false && rotate_right == false) 
        {
            return;
        }
        if (transform.localEulerAngles.y < tablet_looking_angle_min && transform.localEulerAngles.y < tablet_looking_angle_max && rotate_right)
        {
            Debug.Log("Current code should move right");
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 240, 0), Time.deltaTime * speed);
            object_looking = "unity_logo";
        }

        if (transform.localEulerAngles.y > unity_logo_looking_angle_min && transform.localEulerAngles.y > unity_logo_looking_angle_max && rotate_left)
        {
            Debug.Log("Current code should move left");
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 140, 0), Time.deltaTime * speed);
            object_looking = "tablet";
        }

        if (Time.time > nextActionTime)
        {
            Debug.Log("Old is " + nextActionTime);
            Debug.Log("Objecct looking is " + object_looking);
            if (string.Equals(object_looking, "unity_logo"))
            {
                nextActionTime = Time.time +   4;
            }
            else
            {
                Debug.Log("Code is coming here");
                robots_at_door = checkRobotsAtDoor();
                float probability_door_open = (float)(1 - Math.Pow(0.4, robots_at_door));

                float door_open_rand = UnityEngine.Random.Range(0.0f, 1.0f);

                if (door_open_rand < probability_door_open) 
                {
                    StartCoroutine(open_door());
                }

                Debug.Log("Door open probability is :"+probability_door_open);
                number = UnityEngine.Random.Range(0.0f, 1.0f);
                if (number > 0.2f)
                {
                    tablet_rotate = true;
                    starttabletRotateTime = Time.time + 2;
                    tabletRotateTime = Time.time + 4;
                    float tablet_rotate_rand_number = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (tablet_rotate_rand_number > tablet_left_rotate_probability)
                    {
                        tablet_rotate_side = "right";
                        tablet_left_rotate_probability = tablet_left_rotate_probability + 0.25f;
                        Debug.Log("Rotating tablet right");
                    }
                    else
                    {
                        tablet_left_rotate_probability = tablet_left_rotate_probability - 0.25f;
                        tablet_rotate_side = "left";
                        Debug.Log("Rotating tablet left");
                    }


                    nextActionTime = Time.time + 5;
                }
                else
                {
                    nextActionTime = Time.time + 2f;
                }

            }
            if (rotate_right)
            {
                rotate_right = false;
                rotate_left = true;
            }
            else
            {
                rotate_left = false;
                rotate_right = true;
            }
            Debug.Log(Time.time);
            Debug.Log("Next is " + nextActionTime);

            // execute block of code here
        }

        Debug.Log("Number of robots at door is: "+robots_at_door);
        

        if (tablet_rotate == true)
        {
            if (Time.time < tabletRotateTime && Time.time > starttabletRotateTime)
            {
                if (tablet_rotate_side == "right")
                {
                    tabletRotateScript.rotateTabletRight();
                }
                else
                {
                    tabletRotateScript.rotateTabletLeft();
                    //StartCoroutine(open_door());
                }
            }
        }

        


        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(open_door());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(close_door());
        }

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    rotate_right = true;
        //    rotate_left = false;
        //    //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 3, 0), Time.deltaTime * speed);
        //    Debug.Log("Current angle is :" + transform.localEulerAngles.y);
        //}

        //if (transform.localEulerAngles.y < 58 && rotate_right && transform.localEulerAngles.y < 60)
        //{
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 3, 0), Time.deltaTime * speed);

        //    object_looking = "unity_logo";
        //}
        ////else 
        ////{
        ////    rotate_right = false;
        ////    rotate_left = true;
        ////}
        //if (transform.localEulerAngles.y > 5 && transform.localEulerAngles.y < 60 && rotate_left)
        //{
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, -3, 0), Time.deltaTime * speed);
        //    object_looking = "tablet";
        //}
        ////else
        ////{
        ////    rotate_right = true;
        ////    rotate_left = false;
        ////}



























        //rotate_left = true;
        //rotate_right = false;
        //while (transform.localEulerAngles.y > 55 && rotate_left)
        //{
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, -60, 0), Time.deltaTime * speed);

        //}
        //rotate_left = false;
        //rotate_right = true;
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, -60, 0), Time.deltaTime * speed);

        //if (Time.time > nextActionTime)
        //{
        //    nextActionTime += period;
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 60, 0), Time.deltaTime * speed);
        //    // execute block of code here
        //}
        //while (transform.localEulerAngles.y < 55 && transform.localEulerAngles.y < 60)
        //{

        //    //            transform.Rotate(Vector3.up, 1.0f * Time.deltaTime);
        //    //rotateRight();
        //}
        //if (first_trial) 
        //{

        //    rotateRight();
        //    StartCoroutine(StopRotate());
        //    //first_trial = true;
        //}
        //if (rotate_left) 
        //{
        //    rotateLeft();
        //    StartCoroutine(StopRotate());
        //}

        //        if (Input.GetKey(KeyCode.J))
        //        {
        //            target_object = cube_table;

        //        }
        //        else if (Input.GetKey(KeyCode.K))
        //        {

        //            target_object = unity_logo;

        //        }
        //        else if (Input.GetKey(KeyCode.L))
        //        {
        //            target_object = tablet_camera.gameObject;
        //        }
        //        if (!rotate)
        //        {
        //            number = UnityEngine.Random.Range(0.0f, 1.0f);
        //            rotate = true;
        //            Debug.Log("number is " + number);
        //        }
        //        else
        //        {
        //            if (!target_reached)
        //            {
        //                if (number < 0.1)
        //                {
        //                    target_object = cube_table;
        //                }
        //                else if (number >= 0.1 && number < 0.3)
        //                {
        //                    target_object = tablet_camera.gameObject;
        //                    rotateRight();
        //                    //target_reached = true;
        //                    StartCoroutine(ExecuteAfterTime());
        //                }
        //                else
        //                {
        //                    target_object = tablet_camera.gameObject;
        //                    rotateRight();
        //                    //target_reached = true;
        //                    StartCoroutine(ExecuteAfterTime());
        ////                    target_object = unity_logo;
        //                }
        //                Debug.Log(target_object);
        //                //lookAtTargetObject(target_object);
        //                //
        //                float angle = Quaternion.Angle(original_headpose.rotation, target_object.transform.rotation);
        //                Debug.Log(angle);
        //                if (Mathf.Abs(object_angles[target_object.name] - angle) < 0.5)
        //                {
        //                    Debug.Log("Target_reached");
        //                    target_reached = true;
        //                    StartCoroutine(ExecuteAfterTime());
        //                    //rotate = false;
        //                }
        //            }

        //        }
    }

   

    private int checkRobotsAtDoor()
    {
        int number_robots_at_door = 0;
        Debug.Log("I am checking for the robot.");
        Vector3 object_red_postion = tablet_camera.WorldToViewportPoint(redRobotPos);
        Vector3 screen_red_position = tablet_camera.WorldToScreenPoint(redRobotPos);
        Debug.Log("Object position is "+object_red_postion.ToString());
        Debug.Log("Object position  screen is " + screen_red_position.ToString());
        if (object_red_postion.x >= 0 && object_red_postion.x <= 1 && object_red_postion.y >= 0 && object_red_postion.y <= 1 && object_red_postion.z > 0)
        {
            float redDistance = Vector3.Distance(tablet_camera.transform.position, redRobotPos);

            Debug.Log("I can see the Red robot at "+redDistance);
            if (redDistance < 7 && mainControllerScript.load_status_red_robot == 0)
            {
                Debug.Log("Object position  screen is door");
                number_robots_at_door = number_robots_at_door + 1;
            }
            else
            {
                Debug.Log("Object position  screen is corridor");
            }

        }

        Vector3 object_blue_postion = tablet_camera.WorldToViewportPoint(blueRobotPos);
        Vector3 screen_blue_position = tablet_camera.WorldToScreenPoint(blueRobotPos);
        Debug.Log("Object position is " + object_blue_postion.ToString());
        Debug.Log("Object position  screen is " + screen_blue_position.ToString());
        if (object_blue_postion.x >= 0 && object_blue_postion.x <= 1 && object_blue_postion.y >= 0 && object_blue_postion.y <= 1 && object_blue_postion.z > 0)
        {
            float blueDistance = Vector3.Distance(tablet_camera.transform.position, blueRobotPos);

            Debug.Log("I can see the blue robot at " + blueDistance);
            if (blueDistance < 7 && mainControllerScript.load_status_blue_robot == 0)
            {
                Debug.Log("Object position  screen is door blue");
                number_robots_at_door = number_robots_at_door + 1;
            }
            else
            {
                Debug.Log("Object position  screen is corridor blue");
            }

        }

        Vector3 object_green_postion = tablet_camera.WorldToViewportPoint(greenRobotPos);
        Vector3 screen_green_position = tablet_camera.WorldToScreenPoint(greenRobotPos);
        Debug.Log("Object position is " + object_green_postion.ToString());
        Debug.Log("Object position  screen is " + screen_green_position.ToString());
        if (object_green_postion.x >= 0 && object_green_postion.x <= 1 && object_green_postion.y >= 0 && object_green_postion.y <= 1 && object_green_postion.z > 0)
        {
            float greenDistance = Vector3.Distance(tablet_camera.transform.position, greenRobotPos);

            Debug.Log("I can see the green robot at " + greenDistance);
            if (greenDistance < 7 && mainControllerScript.load_status_green_robot == 0)
            {
                Debug.Log("Object position  screen is door green");
                number_robots_at_door = number_robots_at_door + 1;
            }
            else
            {
                Debug.Log("Object position  screen is corridor green");
            }

        }

        return number_robots_at_door;
    }

    private void reassign_task_human()
    {
        Debug.Log("Reassigning tasks"+experiment_number);
        if (experiment_number > 0) 
        {
            Debug.Log("Reassigning tasks writing to file");
            float total_time_human_task = human_task_completion_time + (times_door_opened*15) - human_task_allocation_time;
            string path = "Assets/Experiment_data/experiment"+experiment_number+".txt";
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("Human task completion time :"+total_time_human_task);
            writer.WriteLine("Number of times door opened :"+times_door_opened);
            writer.Close();
        }
        
        human_task_completion_time_rand = UnityEngine.Random.Range(0.0f, 1.0f);
        human_task_completion_time = (human_task_completion_time_rand * 35) + 300 + Time.time;
        human_task_allocation_time  = Time.time;
        experiment_number = experiment_number + 1;
        times_door_opened = 0;
        Debug.Log("Reassigning tasks done");
        wait_flag_robots = true;
    }

    private void lookAtTargetObject(GameObject target_object)
    {
        targetRotation = Quaternion.LookRotation(target_object.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void rotateRight()
    {
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, tablet_camera.transform.rotation, 2f);
        transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);
    }

    private void rotateLeft()
    {
        transform.Rotate(Vector3.down, 80.0f * Time.deltaTime);
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

    IEnumerator ExecuteAfterTime()
    {
        yield return new WaitForSeconds(4.0f);
        target_reached = true;
        rotate = false;
        target_reached = false;
    }

    IEnumerator StopRotate()
    {
        yield return new WaitForSeconds(0.8f);
        first_trial = false;
     //   yield return new WaitForSeconds(0.7f);
    }

    IEnumerator open_door()
    {

        UnityWebRequest www = UnityWebRequest.Get("http://localhost/arn_extension/ar_demo/door_state_arn.php?state=open");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("This is the error :" + www.error);
        }
        times_door_opened = times_door_opened + 1;
    }
    IEnumerator close_door()
    {

        UnityWebRequest www = UnityWebRequest.Get("http://localhost/arn_extension/ar_demo/door_state_arn.php?state=close");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("This is the error :" + www.error);
        }
    }

    IEnumerator getRobotTaskStatus()
    {
        Debug.Log("Getting this 1");
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:80/arn_extension/makeandroidapp/ar_demo/queue_manager/task_status.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Getting this 2"+ www.downloadHandler.text);
            task_status_robots = int.Parse(www.downloadHandler.text);         
        }
        
        if (task_status_robots == 1)
        {
            wait_flag_robots = false;
        }
        else
        { 
            wait_flag_robots = true;
        }
        Debug.Log("Getting this 4");
    }
}
