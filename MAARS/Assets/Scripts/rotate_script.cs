using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_script : MonoBehaviour
{
    public Vector3 point = new Vector3(4.55f,2.218f,-1.578f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(Vector3.up, 20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Z))
            transform.Rotate(-Vector3.up, 20 * Time.deltaTime);
    }
}
