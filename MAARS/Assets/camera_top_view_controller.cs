using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_top_view_controller : MonoBehaviour
{
    GameObject emptyGO;
    // Start is called before the first frame update
    void Start()
    {
        emptyGO = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        Transform newTransform = emptyGO.transform;
        Vector3 new_position;
        new_position = transform.position;
        new_position.y = transform.position.y+0.05f;
        if (transform.position.y < 31) 
        {
            transform.position = new_position;
        }
    }
}
