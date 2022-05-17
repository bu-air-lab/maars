using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class TaskAssignerRobot : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        private UnityEngine.AI.NavMeshPath path;

        WarehouseCreator warehouseScript;

        //List<GameObject> shelf_objects = new List<GameObject>(); // All the Shelf gameobjects to iterate
        //List<GameObject> dropStation_objects = new List<GameObject>(); // All the dropStation gameobjects to iterate
        //List<GameObject> turtlebot_objects = new List<GameObject>(); // All the Turtlebot gameobjects to iterate
        //List<GameObject> shelfBox_objects = new List<GameObject>(); // All the Boxes in Shelves gameobjects to iterate
        List<GameObject> targetGameObjectsRobots = new List<GameObject>(); // All the Boxes in Shelves gameobjects to iterate
        List<GameObject> robotPathTargets = new List<GameObject>(); // Used to differentiate between paths for robots

        // Start is called before the first frame update
        void Start()
        {
            warehouseScript = GetComponent<WarehouseCreator>();
            //Debug.Log("Value of integer is:" + warehouseScript.nRobots);

            for (int i = 0; i < warehouseScript.nRobots; i++) 
            {
                GameObject dummyGameObject = new GameObject(); // To prefill the list of targetGameObjectsRobots with dummy object in the beginning to compare the current and previous assigned frame objects are same or not
                dummyGameObject.name = "dummyObject";
                targetGameObjectsRobots.Add(dummyGameObject);

                robotPathTargets.Add(new GameObject());
            }
            
            

            
        }

        // Update is called once per frame
        private void Update()
        {

            for (int i = 0; i < warehouseScript.nRobots; i++)
            {
                GameObject currrent_robot, current_target_object;
                currrent_robot = warehouseScript.obtainRobot(i);
                current_target_object = warehouseScript.obtainTarget(i);
                //if (current_target_object.name == "dummyObject")
                //{
                //    current_target_object.transform.position = warehouseScript.getRobotInitialPosition(i);
                //}
                target = current_target_object.transform;
                //Debug.Log(target.position);
                //Debug.Log("Object to pickup is: "+current_target_object.name);
                agent = currrent_robot.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
                
                character = currrent_robot.GetComponent<ThirdPersonCharacter>();

                agent.updateRotation = false;
                agent.updatePosition = true;
                path = new UnityEngine.AI.NavMeshPath();
                //Debug.Log("path corners are :"+path.corners);
                
                if (targetGameObjectsRobots[i] != current_target_object) 
                {
                    agent.SetDestination(target.position);
                    targetGameObjectsRobots[i] = current_target_object;
                }
                agent.CalculatePath(target.position, path);



                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            //Debug.Log("Agent:" + i + " has completed the path.");
                            warehouseScript.updateNewLocation(i);
                        }
                    }

                    if(robotPathTargets[i] != targetGameObjectsRobots[i]) //Calculates the length of the current path at its longest
                    {
                        float distance = 0;
                        Vector3 [] pathCorners = agent.path.corners;

                        if (pathCorners.Length > 2)
                        {
                            for (int a = 1; a < pathCorners.Length; a++)
                            {   
                                distance += Vector2.Distance(new Vector2(pathCorners[a - 1].x, pathCorners[a - 1].z), new Vector2(pathCorners[a].x, pathCorners[a].z));
                            }
                        }
                        else
                        {
                            distance = agent.remainingDistance;
                        }

                        warehouseScript.addRobotDistance(i, distance);
                        robotPathTargets[i] = targetGameObjectsRobots[i];
                    }
                }

                //if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.5)
                //{
                //    Debug.Log("Agent:" + i + " has completed the path.");
                //    warehouseScript.updateNewLocation(i);
                //}
                //if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.5)
                //{
                //    Debug.Log("Agent:" + i + " has completed the path.");
                //    warehouseScript.updateNewLocation(i);
                //}
                if (agent.remainingDistance > agent.stoppingDistance)
                    character.Move(agent.desiredVelocity, false, false);
                else
                    character.Move(Vector3.zero, false, false);
            }
            
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}