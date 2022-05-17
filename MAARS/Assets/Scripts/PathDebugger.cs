using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class PathDebugger : MonoBehaviour
    {
        private LineRenderer linerenderer;
        WarehouseCreator warehouseScript;

        private List<GameObject> navLines = new List<GameObject>();

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        private UnityEngine.AI.NavMeshPath path;
        // Start is called before the first frame update
        void Start()
        {
            warehouseScript = GetComponent<WarehouseCreator>();

            AddNavLines();
            
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < warehouseScript.nRobots; i++)
            {
                GameObject current_robot, current_target_object;
                current_robot = warehouseScript.obtainRobot(i);
                current_target_object = warehouseScript.obtainTarget(i);
                //if (current_target_object.name == "dummyObject")
                //{
                //    current_target_object.transform.position = warehouseScript.getRobotInitialPosition(i);
                //}
                target = current_target_object.transform;
                //Debug.Log("Object to pickup is: " + current_target_object.name);
                agent = current_robot.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

                character = current_robot.GetComponent<ThirdPersonCharacter>();

                agent.updateRotation = false;
                agent.updatePosition = true;
                GameObject navLineGameObject = navLines[i];
                LineRenderer lr = navLineGameObject.GetComponent<LineRenderer>();
                if (agent.hasPath)
                {
                    
                    lr.positionCount = agent.path.corners.Length;
                    lr.SetPositions(agent.path.corners);
                    lr.enabled = true;

                    float distanceToTarget = Vector3.Distance(current_robot.transform.position, current_target_object.transform.position);
                    lr.endWidth = (float) (((0.85f * distanceToTarget) / 27.5f) + 0.1f);
                }

                else
                {
                    lr.enabled = false;
                }
            }

        }
        void AddNavLines()
        {
            for (int i = 0; i < warehouseScript.nRobots; i++)
            {
                GameObject newLine = new GameObject();
                //myLine.transform.position = start;
                newLine.AddComponent<LineRenderer>();
                newLine.name = "navLine_Robot_" + i;
                LineRenderer lr = newLine.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Sprites/Default"));
                Color lineColor;
                //lineColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
                lineColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                lr.startColor = lineColor;
                lr.endColor = lineColor;
                lr.startWidth = 0.1f;
                lr.endWidth = 0.95f;
                navLines.Add(newLine);
            }
                
        }
    }
}