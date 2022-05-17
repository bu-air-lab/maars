using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        private UnityEngine.AI.NavMeshPath path;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
            path = new UnityEngine.AI.NavMeshPath();
        }


        private void Update()
        {
            
            if (target != null)
                agent.SetDestination(target.position);
                agent.CalculatePath(target.position, path);

            //for (int i = 0; i < path.corners.Length - 1; i++)
            //{
            //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 100);
            //    Debug.Log("The length of path is: "+ path.corners.Length +" and the path is:" +path.corners[i]);
            //}

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
