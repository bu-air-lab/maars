using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_tablet : MonoBehaviour
{
    public Vector3 point = new Vector3(0, 0, 0);
    public string part = "hello";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(Vector3.up, 70 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Z))
            transform.Rotate(-Vector3.up, 70 * Time.deltaTime);
    }

    public void rotateTabletLeft() 
    {
        transform.Rotate(-Vector3.up, 10 * Time.deltaTime);
    }

    public void rotateTabletRight()
    {
        Debug.Log("Rotating right");
        transform.Rotate(Vector3.up, 10 * Time.deltaTime);
    }
}
