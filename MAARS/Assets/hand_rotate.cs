using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
//            transform.Rotate(Vector3.down, 70 * Time.deltaTime);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 10, 0), (float)(Time.deltaTime * 1));
        }
        else if (Input.GetKey(KeyCode.Z))
//            transform.Rotate(-Vector3.down, 70 * Time.deltaTime);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 500, 0), (float)(Time.deltaTime * 1));
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
