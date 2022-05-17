using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{
    //public List<string> sam_task_list = new List<string>(new string[] { "load(Coca_Cola_Cup_Shelf_1,0)", "approach(madhu_corr,1)", "approach(d1_n09,2)", "open(d1_n09,3)", "gothrough(d1_n09,4)", "unload(Coca_Cola_Cup_Shelf_2,5)", "approach(d1_n09,6)", "open(d1_n09,7)", "gothrough(d1_n09,8)", "approach(ta_area1, 9)" });
    //public List<string> coke_vendor_plan = new List<string>(new string[] { "load(Coca_Cola_Cup_Shelf_1,0)", "approach(madhu_corr,1)", "approach(d1_n09,2)", "open(d1_n09,3)", "gothrough(d1_n09,4)", "unload(Coca_Cola_Cup_Shelf_2,5)", "load(Soda_Can_Shelf_3,6)", "approach(d1_n09,7)", "open(d1_n09,8)", "gothrough(d1_n09,9)", "approach(madhu_corr, 10)", "approach(library, 11)", "unload(Soda_Can_Shelf_1,12)" });
    public List<string> cokes_vendor_entire_plan = new List<string>(new string[] { "load(Coca_Cola_Cup_Shelf_1,0)", "approach(lab_small_door,0)", "unload(Coca_Cola_Cup_Shelf_2,5)" });
    //public List<string> cokes_vendor_randoms_plan = new List<string>(new string[] { "approach(coke_center,1)", "approach(coke_1,0)", "approach(coke_2,1)" });
    public List<string> cokes_vendor_drop_plan = new List<string>(new string[] { "approach(lab_small_door,0)", "unload(Coca_Cola_Cup_Shelf_2,5)" });

    public List<string> cokes_vendor_random_plan = new List<string>(new string[] { "approach(coke_center, 0)", "load(Coca_Cola_Cup_Shelf_1,0),0)", "approach(outside_coke_center,1)", "approach(lounge_door,1)", "unload(Coca_Cola_Cup_Group_1_1,2)", "approach(lounge_door,1)", "approach(lift_area,1)", "approach(outside_coke_center,1)", "approach(coke_center, 0)", "load(Coca_Cola_Cup_Shelf_1,0),0)", "approach(outside_coke_center,1)", "approach(lounge_door,1)", "unload(Coca_Cola_Cup_Group_2_1,5)", "approach(lounge_door,1)", "approach(lift_area,1)", "approach(outside_coke_center,1)", "approach(coke_center, 0)", "load(Coca_Cola_Cup_Shelf_1,0),0)", "approach(outside_coke_center,1)", "approach(lounge_door,1)", "unload(Coca_Cola_Cup_Group_3_1,5)", "approach(lounge_door,1)", "approach(lift_area,1)", "approach(outside_coke_center,1)", "approach(coke_center, 0)"
        
        //, "approach(coke_1, 0)", "load(Coca_Cola_Cup_Shelf_1,0),6)", "approach(lounge_door,7)", "unload(Coca_Cola_Cup_Group_2_1,8)", "approach(coke_1, 0)", "load(Coca_Cola_Cup_Shelf_1,0),9)", "approach(lounge_door,10)", "unload(Coca_Cola_Cup_Group_2_2,11)" 
    });



    public List<string> pizza_vendor_entire_plans = new List<string>(new string[] { "load(pizza_shelf_1,0)", "approach(lab_big_door,0)", "unload(pizza_shelf_2,5)", "approach(pizza_center,0)" });
    //public List<string> pizzas_vendor_randoms_plan = new List<string>(new string[] { "approach(pizza_center,0)", "approach(pizza_1,0)", "approach(pizza_2,1)" });
    public List<string> pizza_vendor_drops_plans = new List<string>(new string[] {"approach(lab_big_door,0)", "unload(pizza_shelf_2,5)", "approach(pizza_center,0)" });
    public List<string> pizzas_vendor_random_plan = new List<string>(new string[] 
    { "approach(pizza_center,0)", "load(pizza_shelf_1,0),0)", "approach(madhu_corr,1)","approach(lounge_door,1)", "unload(Pizza_Group_3_1,2)","approach(lounge_door,1)", "approach(madhu_corr,1)", "approach(pizza_center,0)", "load(pizza_shelf_1,0),0)", "approach(madhu_corr,1)","approach(lounge_door,1)", "unload(Pizza_Group_2_1,5)","approach(lounge_door,1)", "approach(madhu_corr,1)", "approach(pizza_center,1)", "load(pizza_shelf_1,0),0)", "approach(madhu_corr,1)", "unload(Pizza_Group_1_1,5)", "approach(madhu_corr,1)", "approach(pizza_center,1)"
        
        //, "approach(coke_1, 0)", "load(Coca_Cola_Cup_Shelf_1,0),6)", "approach(lounge_door,7)", "unload(Coca_Cola_Cup_Group_2_1,8)", "approach(coke_1, 0)", "load(Coca_Cola_Cup_Shelf_1,0),9)", "approach(lounge_door,10)", "unload(Coca_Cola_Cup_Group_2_2,11)" 
    });

    public List<string> pizzas_hut_vendor_entire_plans = new List<string>(new string[] { "load(pizza_shelf_1,0)", "approach(lab_big_door,0)", "unload(pizza_shelf_2,5)", "approach(pizza_center,0)" });
    //public List<string> pizzas_vendor_randoms_plan = new List<string>(new string[] { "approach(pizza_center,0)", "approach(pizza_1,0)", "approach(pizza_2,1)" });
    public List<string> pizza_hut_vendor_drops_plans = new List<string>(new string[] { "approach(lab_big_door,0)", "unload(pizza_shelf_2,5)", "approach(pizza_center,0)" });
    public List<string> pizza_hut_vendors_random_plan = new List<string>(new string[]
    { "approach(pizza_hut_center,0)", "load(pizza_shelf_1,0),0)", "unload(Pizza_Group_3_1,2)","approach(pizza_hut_center,0)", "load(pizza_shelf_1,0),0)", "unload(Pizza_Group_2_1,5)","approach(pizza_hut_center,0)", "load(pizza_shelf_1,0),0)", "unload(Pizza_Group_1_1,5)", "approach(pizza_hut_center,0)"
    });

    public Dictionary<string, string> locations = new Dictionary<string, string>
        {
            { "madhu_corr", "0: (13.49, 1, 5.92)"},
            { "library", "0: (17.89, 1, 14.93)"},
            { "d1_n09", "1: (7.16, 1, -0.48);(5.62,1,-1.14)" },
            { "ta_area1", "0: (11.73, 1, -1.67)" },
            { "coke_center", "0: (1.32, 1, -18.16)"},
            { "coke_1", "0: (2.982, 1, -25.106)"},
            { "coke_2", "0: (-2.329, 1, -17.926)"},
            { "pizza_center", "0: (22.85, 1, 7.13)"},
            { "pizza_1", "0: (24.3, 1, -0.172)"},
            { "pizza_2", "0: (25.337, 1, 12.219)"},
            { "lab_small_door", ("0:6.44,1, -11") },
            { "lab_big_door", ("0:6.44,1, -0.51") },
            { "lounge_door", ("0:15.04,1, -11.35") },
            { "outside_coke_center", ("0:-4.3,1, -21.6") },
            { "lift_area", ("0:4.58,1, -21.02") },
            { "pizza_hut_center", "0: (-14.74, 1, 7.21)"}



        };

    public GameObject coke_1;
    public GameObject coke_2;
    public GameObject pizza_1;
    public GameObject pizza_2;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(locations["coke_1"]);
        string current_location_coke_1 = "0: ";
        current_location_coke_1 = current_location_coke_1 + "(" + coke_1.transform.position.x + "," + coke_1.transform.position.y + "," + coke_1.transform.position.z + ")";
        locations["coke_1"] = current_location_coke_1;
        Debug.Log(locations["coke_1"]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
