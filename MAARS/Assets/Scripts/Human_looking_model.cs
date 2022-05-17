using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_looking_model : MonoBehaviour
{
    // Start is called before the first frame update
    Camera tablet_camera;
    public Vector3 point = new Vector3(0, 0, 0);
    Transform original_headpose;
    Transform original_tablet_pose;
    int speed = 2;
    Quaternion targetRotation;
    bool first = true;
    GameObject unity_logo;
    GameObject big_door;
    GameObject small_door;
    GameObject tablet;
    // Start is called before the first frame update
    GameObject target_object;
    GameObject old_target_object;
    Dictionary<string, float> object_angles = new Dictionary<string, float>();

    float probability_straight = 0.6f;
    float probability_right = 0.2f;
    float probability_left = 0.2f;
    bool rotate = false;
    float number,random_number;
    bool target_reached = false;

    bool tablet_reached_target = false;
    bool tablet_rotate = false;
    bool first_trial = true;
    RaycastHit hit_firstPerson;
    Camera mainCamera;
    Vector3 cameraCenter;
    GameObject currentCenterObject;
    void Start()
    {
        mainCamera = findMainCamera();
        Debug.Log("Camera name is :" + mainCamera.name);
        tablet_camera = findTabletCamera();
        unity_logo = GameObject.Find("Unity_logo");
        big_door = GameObject.Find("Big_door");
        small_door = GameObject.Find("Small_door_AR");
        tablet = GameObject.Find("Tablet");
        original_headpose = transform.transform;
        original_tablet_pose = tablet.transform;
        object_angles.Add("Left_Object", 50.0f);
        object_angles.Add("GUICamera", 27.85f);
        object_angles.Add("Unity_logo", 31.4f);
        cameraCenter = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, mainCamera.nearClipPlane));
    }

    void Update()
    {
        
        if (!rotate)
        {
            if (Input.GetKey(KeyCode.M))
            {
                number = 0.1f;
                rotate = true;
                target_reached = false;
                Debug.Log("number is " + number);
            }
            else if (Input.GetKey(KeyCode.N))
            {
                number = 0.8f;
                rotate = true;
                target_reached = false;
                Debug.Log("number is " + number);
            }
            //number = UnityEngine.Random.Range(0.0f, 1.0f);
            
        }
        else
        {
            Debug.Log("Current step");
            if (!target_reached)
            {
                Debug.Log("Current step 2");
                if (number < 0.5)
                {
                    target_object = tablet;
                    if (old_target_object == target_object)
                    {
                        Debug.Log("Same target");
                    }
                    else
                    {
                        rotateRight();
                    }

                    float angle = Quaternion.Angle(transform.rotation, tablet.transform.rotation);
                    Debug.Log("Angle of Tablet logo is :" + angle);
                    if (Mathf.Abs(170 - angle) < 1)
                    {

                        old_target_object = tablet;
                        Debug.Log("Target_reached");
                        //target_reached = true;
                        StartCoroutine(ExecuteAfterTime());
                        //rotate = false;
                    }
                    //if (Mathf.Abs(object_angles[target_object.name] - angle) < 1)
                    //{
                    //    //old_target_object = target_object;
                    //    Debug.Log("Target_reached");
                    //    //target_reached = true;
                    //    StartCoroutine(ExecuteAfterTime());
                    //    //rotate = false;
                    //}
                    //StartCoroutine(TimedExecution());
                }
                else
                {
                    target_object = unity_logo;
                    //target_object = tablet_camera.gameObject;
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -10, 0), 2 * Time.deltaTime);
                    //target_reached = true;
                    //rotate = false;
                    if (old_target_object == target_object)
                    {
                        Debug.Log("Same target");
                    }
                    else 
                    {
                        rotateLeft();
                    }

                    float angle = Quaternion.Angle(transform.rotation, unity_logo.transform.rotation);
                    Debug.Log("Angle of Unity logo is :" + angle);
                    if (Mathf.Abs(50 - angle) < 1)
                    {

                        old_target_object = unity_logo;
                        Debug.Log("Target_reached");
                        //target_reached = true;
                        StartCoroutine(ExecuteAfterTime());
                        //rotate = false;
                    }
                    //StartCoroutine(TimedExecution());

                }

                ////if ((old_target_object != target_object) )
                ////{
                //Debug.Log("Current step 3");
                //lookAtTargetObject(target_object);


                //float angle = Quaternion.Angle(transform.rotation, target_object.transform.rotation);
                //Debug.Log("Angle is :" + angle);
                //Debug.Log("Target name:" + target_object.name);
                ////StartCoroutine(ExecuteAfterTime());
                //if (Mathf.Abs(object_angles[target_object.name] - angle) < 1)
                //{
                //    //old_target_object = target_object;
                //    Debug.Log("Target_reached");
                //    //target_reached = true;
                //    StartCoroutine(ExecuteAfterTime());
                //    //rotate = false;
                //}

            }
        }
    }



    private void lookAtTargetObject(GameObject target_object)
    {
        targetRotation = Quaternion.LookRotation(target_object.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void moveTablet(string target_objectName)
    {
        if (target_objectName == "bigDoor")
        {
            tablet.transform.rotation = Quaternion.RotateTowards(tablet.transform.rotation, Quaternion.Euler(0, 200, 0), 20 * Time.deltaTime);
        }
        else if (target_objectName == "smallDoor")
        {
            tablet.transform.rotation = Quaternion.RotateTowards(tablet.transform.rotation, Quaternion.Euler(0, 40, 0), 20 * Time.deltaTime);
        }
        else 
        {
            tablet.transform.rotation = Quaternion.RotateTowards(tablet.transform.rotation, Quaternion.Euler(0, 100, 0), 20 * Time.deltaTime);
        }
            //tablet.transform.rotation = Quaternion.Slerp(tablet.transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void rotateRight()
    {

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(26, 220, 0), 30 * Time.deltaTime);
        
    }

    private void rotateLeft()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 20 * Time.deltaTime);
//        transform.Rotate(Vector3.right, 100.0f * Time.deltaTime);
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

    public Camera findMainCamera()
    {
        
        return Camera.main;
    }


    IEnumerator ExecuteDelay()
    {
        yield return new WaitForSeconds(15.0f);
        if (target_object.name == "GUICamera")
        {
            Debug.Log("Step 1");
            if (!tablet_rotate)
            {
                Debug.Log("Step 2");
                random_number = UnityEngine.Random.Range(0.0f, 1.0f);
                tablet_rotate = true;
                Debug.Log("New random number is " + number);

            }
            else
            {
                Debug.Log("Step 3");
                if (!tablet_reached_target)
                {

                    Debug.Log("Step 4");
                    if (random_number < 0.5)
                    {
                        Debug.Log("Step 5");
                        moveTablet("bigDoor");
                        float tablet_angle = Quaternion.Angle(original_tablet_pose.rotation, big_door.transform.rotation);
                        Debug.Log("Tablet angle is :" + tablet_angle);
                        if (Mathf.Abs(134 - tablet_angle) < 1)
                        {
                            Debug.Log("Step 6");
                            Debug.Log("Tablet Target_reached");
                            tablet_reached_target = true;
                            //tablet_rotate = false;
                            //rotate = false;
                            //target_reached = false;

                        }
                    }
                    else
                    {
                        Debug.Log("Step 7");
                        moveTablet("smallDoor");
                        float tablet_angle = Quaternion.Angle(original_tablet_pose.rotation, small_door.transform.rotation);
                        Debug.Log("Small door Tablet angle is :" + tablet_angle);
                        if (Mathf.Abs(82 - tablet_angle) < 1)
                        {
                            Debug.Log("Step 8");
                            Debug.Log("Target_reached");
                            tablet_reached_target = true;
                            //tablet_rotate = false;
                            //rotate = false;
                            //target_reached = false;
                        }
                    }

                }
                else
                {
                    Debug.Log("Step 9");
                    rotate = false;
                    target_reached = true;
                }
            }
        }
        else
        {
            Debug.Log("Step 10");
            rotate = false;
            target_reached = true;
        }

    }

    IEnumerator ExecuteAfterTime()
    {
        target_reached = true;
        
        yield return new WaitForSeconds(1.0f);
        rotate = false;
        
        //if (target_object.name == "GUICamera")
        //{
        //    Debug.Log("Step 1");
        //    if (!tablet_rotate)
        //    {
        //        Debug.Log("Step 2");
        //        random_number = UnityEngine.Random.Range(0.0f, 1.0f);
        //        tablet_rotate = true;
        //        Debug.Log("New random number is " + number);

        //    }
        //    else
        //    {
        //        Debug.Log("Step 3");
        //        if (!tablet_reached_target)
        //        {

        //            Debug.Log("Step 4");
        //            if (random_number < 0.5)
        //            {
        //                Debug.Log("Step 5");
        //                moveTablet("bigDoor");
        //                float tablet_angle = Quaternion.Angle(original_tablet_pose.rotation, big_door.transform.rotation);
        //                Debug.Log("Tablet angle is :" + tablet_angle);
        //                if (Mathf.Abs(134 - tablet_angle) < 1)
        //                {
        //                    Debug.Log("Step 6");
        //                    Debug.Log("Tablet Target_reached");
        //                    tablet_reached_target = true;
        //                    //tablet_rotate = false;
        //                    //rotate = false;
        //                    //target_reached = false;

        //                }
        //            }
        //            else
        //            {
        //                Debug.Log("Step 7");
        //                moveTablet("smallDoor");
        //                float tablet_angle = Quaternion.Angle(original_tablet_pose.rotation, small_door.transform.rotation);
        //                Debug.Log("Small door Tablet angle is :" + tablet_angle);
        //                if (Mathf.Abs(82 - tablet_angle) < 1)
        //                {
        //                    Debug.Log("Step 8");
        //                    Debug.Log("Target_reached");
        //                    tablet_reached_target = true;
        //                    //tablet_rotate = false;
        //                    //rotate = false;
        //                    //target_reached = false;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            Debug.Log("Step 9");
        //            rotate = false;
        //            target_reached = true;
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.Log("Step 10");
        //    rotate = false;
        //    target_reached = true;
        //}

    }

    IEnumerator TimedExecution()
    {
        target_reached = true;
        yield return new WaitForSeconds(10.0f);
        rotate = false;
        
        

    }
}
