using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vis_Human_Look_Handler : MonoBehaviour
{
    public GameObject SamHuman;
    public Dropdown myDropdown;

    public GameObject cokeVendor;
    public GameObject joespizzaVendor;
    public GameObject pizzaHutVendor;
    public GameObject CokeTable;
    public GameObject Desk;

    public GameObject coke_logo;
    public GameObject joes_pizza_logo;
    public GameObject lounge_logo;
    public GameObject pizza_hut_logo;

    GameObject objectToVisualize;

    public string current_visualizing_object = "Desk";
    // Start is called before the first frame update

    public float Speed = 20f;

    void Start()
    {
      
        // Calculate the journey length.
        
    }

    // Update is called once per frame
    void Update()
    {
        if (string.Equals(current_visualizing_object, "Desk"))
        {
            objectToVisualize = Desk;
        }
        else if (string.Equals(current_visualizing_object, "JoesPizzaLogo"))
        {
            //objectToVisualize = pizzaVendor;
            objectToVisualize = joes_pizza_logo;

        }
        else if (string.Equals(current_visualizing_object, "CokeLogo"))
        {
            //objectToVisualize = cokeVendor;
            objectToVisualize = coke_logo;

        }
        else if (string.Equals(current_visualizing_object, "Desk"))
        {
            objectToVisualize = CokeTable;
        }
        else if (string.Equals(current_visualizing_object, "PizzaHutLogo"))
        {
            objectToVisualize = pizza_hut_logo;
        }
        else if (string.Equals(current_visualizing_object, "LoungeLogo"))
        {
            objectToVisualize = lounge_logo;
        }
        else if (string.Equals(current_visualizing_object, "JoesPizzaPerson"))
        {
            objectToVisualize = joespizzaVendor;
        }
        else if (string.Equals(current_visualizing_object, "CokePerson"))
        {
            objectToVisualize = cokeVendor;
        }
        else if (string.Equals(current_visualizing_object, "PizzaHutPerson"))
        {
            objectToVisualize = pizzaHutVendor;
        }
        //while (Vector3.Distance(SamHuman.transform.position, transform.position) > 0.1)
        //{
        //    Vector3 direction;
        //    direction = transform.position - SamHuman.transform.position;
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f * Time.deltaTime);
        //    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        //    //yield;
        //}
    }

    void LateUpdate()
    {
        if (objectToVisualize == joes_pizza_logo)
        {
            Quaternion rotTarget  = Quaternion.LookRotation(objectToVisualize.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Speed * Time.deltaTime);
        }
        else 
        {
            Vector3 dir = transform.position - objectToVisualize.transform.position;

            //Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f * Time.deltaTime);
            //transform.position = SamHuman.transform.position + rotation * dir;  //posizione camera
            Quaternion rotTarget = Quaternion.LookRotation(objectToVisualize.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Speed * Time.deltaTime);
            //transform.LookAt(objectToVisualize.transform.position);
        }


    }
}
