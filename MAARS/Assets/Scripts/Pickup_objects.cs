using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_objects : MonoBehaviour
{
    public GameObject coke_shelf_2;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Coca_Cola_Cup_Shelf_1")
        {
            Debug.Log("Collision");
            Debug.Log(other.gameObject.name);
            other.gameObject.SetActive(false);
            coke_shelf_2.SetActive(true);
            //Vector3 new_position = other.gameObject.transform.position;
            //new_position.y = new_position.y + 0.5f;
            //transform.position = new_position;
            //transform.parent = other.gameObject.transform;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
