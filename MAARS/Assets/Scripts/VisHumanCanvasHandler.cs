using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class VisHumanCanvasHandler : MonoBehaviour
{
    public CokeVendorController coke_vendor_controller;
    public PizzaVendorController pizza_vendor_controller;
    public Vis_Human_Look_Handler visHandler;

    public TextMeshProUGUI planText;
    string planString = "";
    public Dropdown myDropdown;

    List<string> coke_vendor_current_plan;
    int coke_vendor_current_task_index;
    int previous_coke_vendor_task_index = -1;

    List<string> pizza_vendor_current_plan;
    int pizza_vendor_current_task_index;
    int previous_pizza_vendor_task_index = -1;

    bool showPlan = false;
    int current_food_option = 0;
    // Start is called before the first frame update
    void Start()
    {
        coke_vendor_controller = GameObject.Find("CokeVendor").GetComponent <CokeVendorController>();
        coke_vendor_current_plan = coke_vendor_controller.current_plan;
        coke_vendor_current_task_index = coke_vendor_controller.current_task_index;
        
        pizza_vendor_controller = GameObject.Find("JoesPizzaVendor").GetComponent<PizzaVendorController>();
        pizza_vendor_current_plan = pizza_vendor_controller.current_plan;
        pizza_vendor_current_task_index = pizza_vendor_controller.current_task_index;
        

        visHandler = GameObject.Find("VisualizingHuman").GetComponent<Vis_Human_Look_Handler>();
        planText = GetComponentInChildren<TextMeshProUGUI>();

        myDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(myDropdown);
        });
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buyButton();
        }

        //Dropdown_values: 0 - Select food, 1 - Joes Pizza, 2- Coke, 3- Pizza Hut, 4- Lounge
        if (Input.GetKeyDown(KeyCode.V))
        {
            current_food_option = 0;
            myDropdown.value = 0;
            visHandler.current_visualizing_object = "Desk";
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            //visHandler.current_visualizing_object = "PizzaVendor";
            current_food_option = 1;
            myDropdown.value = 1;
            visHandler.current_visualizing_object = "JoesPizzaLogo";
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //visHandler.current_visualizing_object = "CokeVendor";
            
            current_food_option = 2;
            myDropdown.value = 2;
            visHandler.current_visualizing_object = "CokeLogo";
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            current_food_option = 3;
            myDropdown.value = 3;
            visHandler.current_visualizing_object = "PizzaHutLogo";
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            current_food_option = 4;
            myDropdown.value = 4;
            visHandler.current_visualizing_object = "LoungeLogo";
        }
        
        //if (showPlan) 
        //{
        //    coke_vendor_current_task_index = coke_vendor_controller.current_task_index;
        //    Debug.Log("Sam task index is: " + coke_vendor_current_task_index + "old: " + previous_coke_vendor_task_index);
        //    if (previous_coke_vendor_task_index != coke_vendor_current_task_index)
        //    {
        //        planString = "";
        //        coke_vendor_current_plan = coke_vendor_controller.current_plan;

        //        for (int i = 0; i < coke_vendor_current_plan.Count; i++)
        //        {
        //            if (coke_vendor_current_task_index - 1 == i)
        //            {
        //                Debug.Log("Sam colors index: " + coke_vendor_current_task_index + ":" + i);
        //                planString = planString + " " + "<color=#FF0000>" + coke_vendor_current_plan[i] + "</color>";
        //            }
        //            else
        //            {
        //                Debug.Log(i);
        //                planString = planString + " " + coke_vendor_current_plan[i];
        //            }

        //        }

        //        planText.SetText(planString);
        //        previous_coke_vendor_task_index = coke_vendor_current_task_index;

        //    }
        //}


    }

    void Destroy()
    {
        myDropdown.onValueChanged.RemoveAllListeners();
    }

    public void buyButton() 
    {
        switch (current_food_option)
        {
            case 0:
                break;

            case 1:
                pizza_vendor_controller.randomWalks = false;
                pizza_vendor_controller.current_path_complete = 0;
                visHandler.current_visualizing_object = "JoesPizzaPerson";
                Debug.Log("Pizza vendor randomwalks turned off");
                break;

            case 2:
                coke_vendor_controller.randomWalks = false;
                Debug.Log("Coke vendor randomwalks turned off");
                visHandler.current_visualizing_object = "CokePerson";
                break;

            case 3:
                coke_vendor_controller.randomWalks = false;
                Debug.Log("Pizza Hut vendor randomwalks turned off");
                visHandler.current_visualizing_object = "PizzaHutPerson";
                break;

            case 4:
                coke_vendor_controller.randomWalks = false;
                Debug.Log("Coke vendor randomwalks turned off");
                visHandler.current_visualizing_object = "LoungeLogo";
                break;

            default:
                break;
        }
    }

    public void myDropdownValueChangedHandler(Dropdown target)
    {
        Debug.Log("selected: " + target.value);
        current_food_option = target.value;
        switch (target.value)
        {
            case 0:
                visHandler.current_visualizing_object = "Coke";
                break;

            case 1:
                //pizza_vendor_controller.randomWalks = false;
                //pizza_vendor_controller.current_path_complete = 0;
                visHandler.current_visualizing_object = "JoesPizzaLogo";
                break;

            case 2:
                //coke_vendor_controller.randomWalks = false;
                visHandler.current_visualizing_object = "CokeLogo";
                break;

            case 3:
                visHandler.current_visualizing_object = "PizzaHutLogo";
                break;

            case 4:
                visHandler.current_visualizing_object = "LoungeLogo";
                break;
            default:

                break;
        }
    }

    public void SetDropdownIndex(int index)
    {
        myDropdown.value = index;
    }
}
