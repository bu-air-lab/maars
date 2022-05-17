using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CokeVendorController : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        private UnityEngine.AI.NavMeshPath path;

        public GameObject coke_shelf_1;
        public GameObject coke_shelf_2;
        public GameObject soda_can_shelf_3;
        public GameObject soda_can_shelf_1;
        public GameObject coke_human_hand;
        public GameObject soda_can_human_head;
        public GameObject coke_group_1_1;
        public GameObject coke_group_1_2;
        public GameObject coke_group_2_1;
        public GameObject coke_group_2_2;
        public GameObject coke_group_3_1;



        public Planner plannerScript;


        List<string> task_list = new List<string>(new string[] { "Coca_Cola_Cup_Shelf_1", "Coca_Cola_Cup_Shelf_2" });

        public List<string> delivery_plan;
        public List<string> drop_plan;

        public List<string> current_random_plan;
        public List<string> current_plan;
        Dictionary<string, string> locations;
        public int current_task_index = 0;

        int path_complete_range = 1;
        int current_path_complete = 0;
        GameObject object_to_handle;
        public string previous_task = "";
        public string previous_object_name = "";
        string object_name = "";

        public bool randomWalks = true;
        bool deliveryComplete = false;
        bool deliveryStarted = false;

        // Global Variables

        string location_name;
        string location;
        string door_or_not;
        string location_0;
        string location_1;
        float x_0;
        float y_0;
        float z_0;

        Vector3 position_0;

        float x_1;
        float y_1;
        float z_1;

        Vector3 position_1;

        float dist_0;
        float dist_1;

        GameObject emptyGO;



        //void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.name == "Coca_Cola_Cup_Shelf_1")
        //    {
        //        coke_shelf_1.SetActive(false);
        //        //coke_shelf_2.SetActive(true);
        //    }
        //    else if (other.gameObject.name == "Coca_Cola_Cup_Shelf_2") 
        //    {
        //        coke_shelf_2.SetActive(true);
        //    }
        //}

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updateRotation = false;
            agent.updatePosition = true;
            path = new UnityEngine.AI.NavMeshPath();
            
            Debug.Log(delivery_plan.ToString());
            locations = plannerScript.locations;
            emptyGO = new GameObject();
            delivery_plan = plannerScript.cokes_vendor_entire_plan;
            drop_plan = plannerScript.cokes_vendor_drop_plan;
            current_random_plan = plannerScript.cokes_vendor_random_plan;
            current_plan = current_random_plan;
            
        }


        private void Update()
        {
            float dist = agent.remainingDistance;
            Debug.Log("random walks value is: "+randomWalks);
            if (randomWalks == false)
            {
                if (current_path_complete >= current_plan.Count && deliveryStarted)
                {
                    deliveryComplete = true;
                }
                if (deliveryStarted == false)
                {
                    agent.isStopped = true;
                    agent.SetDestination(transform.position);
                    agent.ResetPath();

                    if (coke_human_hand.activeSelf)
                    {
                        current_plan = drop_plan;
                    }
                    else
                    {
                        current_plan = drop_plan;
                    }
                    deliveryStarted = true;

                    current_path_complete = 0;
                }
                if (deliveryComplete) 
                {
                    current_plan = current_random_plan;
                    current_path_complete = 0;
                    randomWalks = true;
                }
                
                


            }
            else
            {
                if (current_path_complete >= current_plan.Count)
                {
                    current_path_complete = 0;
                }
            }
            foreach(string plan_element in current_plan)
                {
                Debug.Log("Current plan is:"+plan_element+ current_path_complete);
    
                }
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0.5)
                {
                    Debug.Log("Previous object name is:" + previous_object_name);
                    switch (previous_task)
                    {

                        case "load":
                            object_to_handle = findPlannedObject(previous_object_name);
                            //object_to_handle.SetActive(false);
                            previous_object_name = previous_object_name.Split('_')[0];
                            Debug.Log("Should go here loading"+object_to_handle.transform.position.ToString());

                        if (String.Equals(previous_object_name, "Coca"))
                            {
                                coke_human_hand.SetActive(true);
                            }
                            
                            break;

                        case "unload":
                            object_to_handle = findPlannedObject(previous_object_name);
                            object_to_handle.SetActive(true);
                            previous_object_name = previous_object_name.Split('_')[0];

                            if (String.Equals(previous_object_name, "Coca"))
                            {
                                coke_human_hand.SetActive(false);

                            }
                            else
                            {
                                soda_can_human_head.SetActive(false);
                            }
                            break;

                    }
                    if (current_path_complete < current_plan.Count)
                    {
                        string current_action_string = current_plan[current_path_complete];
                        string current_action = (current_action_string.Split('('))[0];
                        
                        // Dummy transform object to create an empty transform variable

                        Transform newTransform = emptyGO.transform;

                        switch (current_action)
                        {
                            case "load":
                                object_name = (current_action_string.Split('('))[1];
                                object_name = (object_name.Split(','))[0];
                                Debug.Log(object_name);

                                object_to_handle = findPlannedObject(object_name);
                                previous_task = current_action;
                                previous_object_name = object_name;
                                Debug.Log("Should go here"+object_to_handle.transform.position.ToString());
                                SetTarget(object_to_handle.transform);

                                break;

                            case "unload":
                                object_name = (current_action_string.Split('('))[1];
                                object_name = (object_name.Split(','))[0];
                                Debug.Log(object_name);

                                object_to_handle = findPlannedObject(object_name);
                                Debug.Log(current_action_string);
                                previous_task = current_action;
                                previous_object_name = object_name;
                                SetTarget(object_to_handle.transform);
                                break;

                            case "approach":
                                location_name = (current_action_string.Split('('))[1];
                                location_name = (location_name.Split(','))[0];
                                location = locations[location_name];
                                door_or_not = location.Split(':')[0];
                                if (String.Equals(door_or_not, "0"))
                                {
                                    location = location.Split(':')[1];
                                    location = location.Replace("(", "");
                                    location = location.Replace(")", "");
                                    float x = float.Parse(location.Split(',')[0]);
                                    float y = float.Parse(location.Split(',')[1]);
                                    float z = float.Parse(location.Split(',')[2]);

                                    Vector3 new_position = new Vector3(x, y, z);
                                    newTransform.position = new_position;
                                    SetTarget(newTransform);

                                }
                                else
                                {
                                    location = location.Split(':')[1];
                                    location_0 = location.Split(';')[0];
                                    location_1 = location.Split(';')[1];

                                    location_0 = location_0.Replace("(", "");
                                    location_0 = location_0.Replace(")", "");

                                    location_1 = location_1.Replace("(", "");
                                    location_1 = location_1.Replace(")", "");

                                    x_0 = float.Parse(location_0.Split(',')[0]);
                                    y_0 = float.Parse(location_0.Split(',')[1]);
                                    z_0 = float.Parse(location_0.Split(',')[2]);

                                    position_0 = new Vector3(x_0, y_0, z_0);

                                    x_1 = float.Parse(location_1.Split(',')[0]);
                                    y_1 = float.Parse(location_1.Split(',')[1]);
                                    z_1 = float.Parse(location_1.Split(',')[2]);

                                    position_1 = new Vector3(x_1, y_1, z_1);

                                    dist_0 = Vector3.Distance(position_0, transform.position);
                                    dist_1 = Vector3.Distance(position_1, transform.position);

                                    if (dist_0 < dist_1)
                                    {
                                        newTransform.position = position_0;
                                        SetTarget(newTransform);
                                    }

                                    else
                                    {
                                        newTransform.position = position_1;
                                        SetTarget(newTransform);
                                    }

                                    //float x = float.Parse(location.Split(',')[0]);
                                    //float y = float.Parse(location.Split(',')[1]);
                                    //float z = float.Parse(location.Split(',')[2]);

                                    //Vector3 new_position = new Vector3(x, y, z);
                                    //newTransform.position = new_position;
                                    //SetTarget(newTransform);
                                }

                                previous_task = "";
                                previous_object_name = "";
                                //SetTarget(object_to_handle.transform);
                                break;

                            case "open":
                                Debug.Log(current_action_string);
                                previous_task = "";
                                previous_object_name = "";
                                break;

                            case "gothrough":
                                location_name = (current_action_string.Split('('))[1];
                                location_name = (location_name.Split(','))[0];
                                location = locations[location_name];
                                door_or_not = location.Split(':')[0];
                                location = location.Split(':')[1];
                                location_0 = location.Split(';')[0];
                                location_1 = location.Split(';')[1];

                                location_0 = location_0.Replace("(", "");
                                location_0 = location_0.Replace(")", "");

                                location_1 = location_1.Replace("(", "");
                                location_1 = location_1.Replace(")", "");

                                x_0 = float.Parse(location_0.Split(',')[0]);
                                y_0 = float.Parse(location_0.Split(',')[1]);
                                z_0 = float.Parse(location_0.Split(',')[2]);

                                position_0 = new Vector3(x_0, y_0, z_0);

                                x_1 = float.Parse(location_1.Split(',')[0]);
                                y_1 = float.Parse(location_1.Split(',')[1]);
                                z_1 = float.Parse(location_1.Split(',')[2]);

                                position_1 = new Vector3(x_1, y_1, z_1);

                                dist_0 = Vector3.Distance(position_0, transform.position);
                                dist_1 = Vector3.Distance(position_1, transform.position);

                                if (dist_0 > dist_1)
                                {
                                    newTransform.position = position_0;
                                    SetTarget(newTransform);
                                }

                                else
                                {
                                    newTransform.position = position_1;
                                    SetTarget(newTransform);
                                }
                                previous_task = "";
                                previous_object_name = "";
                                break;

                            default:
                                Debug.Log("Something bad.");
                                previous_task = "";
                                previous_object_name = "";
                                break;


                        }






                        //    if (current_path_complete == 1)
                        //    {
                        //        Debug.Log("Path completed." + coke_shelf_2.transform.ToString());

                        //        coke_shelf_1.SetActive(false);
                        //        SetTarget(coke_shelf_2.transform);

                        //    }
                        //    else if (current_path_complete == 2)
                        //    {
                        //        coke_shelf_2.SetActive(true);
                        //        GameObject emptyGO = new GameObject();
                        //        Transform newTransform = emptyGO.transform;
                        //        Vector3 final_position = new Vector3(11.63f, 0.5f, -1.67f);
                        //        newTransform.position = final_position;
                        //        SetTarget(newTransform);

                        //    }
                        //    else if (current_path_complete == 3)
                        //    {
                        //        coke_shelf_2.SetActive(true);
                        //    }
                        current_path_complete += 1;
                        current_task_index += 1;
                    }
                }
            

            
            if (target != null)
                agent.SetDestination(target.position);
            agent.CalculatePath(target.position, path);

            
            
            Debug.Log("Agent stopped: "+ agent.isStopped);
            
                if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private GameObject findPlannedObject(string objectName) 
        {
            GameObject current_object = new GameObject();

            switch (objectName) 
            {
                case "Coca_Cola_Cup_Shelf_1":
                    current_object = coke_shelf_1;
                    break;

                case "Coca_Cola_Cup_Shelf_2":
                    current_object = coke_shelf_2;
                    break;

                case "Soda_Can_Shelf_3":
                    current_object = soda_can_shelf_3;
                    break;

                case "Soda_Can_Shelf_1":
                    current_object = soda_can_shelf_1;
                    break;

                case "Coca_Cola_Cup_Group_1_1":
                    current_object = coke_group_1_1;
                    break;

                case "Coca_Cola_Cup_Group_1_2":
                    current_object = coke_group_1_2;
                    break;

                case "Coca_Cola_Cup_Group_2_1":
                    current_object = coke_group_2_1;
                    break;

                case "Coca_Cola_Cup_Group_2_2":
                    current_object = coke_group_2_2;
                    break;

                case "Coca_Cola_Cup_Group_3_1":
                    current_object = coke_group_3_1;
                    break;

                default:
                    break;
            }

            return current_object;
        }
    }
}
