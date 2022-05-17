using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationDebugger : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agentHuman2;

    private LineRenderer linerenderer;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent < LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agentHuman2.hasPath)
        {
            linerenderer.positionCount = agentHuman2.path.corners.Length;
            linerenderer.SetPositions(agentHuman2.path.corners);
            linerenderer.enabled = true;
        }

        else 
        {
            linerenderer.enabled = false;
        }
    }
}
