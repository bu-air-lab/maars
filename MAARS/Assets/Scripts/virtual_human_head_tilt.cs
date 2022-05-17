using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class virtual_human_head_tilt : MonoBehaviour
{
    public GameObject tablet;
    public GameObject computer_screen;
    public GameObject neck;
    public GameObject jigsawPlayer;
    private VideoPlayer jigsawVideoPlayer;
    public int rotateSpeed = 500;

    bool isCoroutineExecuting = false;
    bool turnLeft = true;
    float nextActionTime = 0.0f;
    string current_turn = "left";
    // Start is called before the first frame update
    void Start()
    {
        neck = GameObject.Find("Head_M");
        Debug.Log(neck.transform.position);
        jigsawVideoPlayer = jigsawPlayer.GetComponent<VideoPlayer>();
        jigsawVideoPlayer.Pause();

    }

    // Update is called once per frame
    void Update()
    {
        //return;
        //Vector3 relativePos = tablet.transform.position - neck.transform.position;
        //Quaternion rotation = neck.transform.rotation;
        //rotation.x = rotation.x + 1;
        //neck.transform.LookAt(tablet.transform);
        //StartCoroutine(ExecuteAfterTime(5));

        if (Time.time < nextActionTime)
        {
            if (string.Equals(current_turn, "left"))
            {
                neck.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
                Debug.Log("Executing loop left");

            }
            else if (string.Equals(current_turn, "right"))
            {
                neck.transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0);
                Debug.Log("Executing loop right");

            }
            else if (string.Equals(current_turn, "wait"))
            {

            }

        }
        else 
        {
            if (string.Equals(current_turn, "left"))
            {
                current_turn = "wait";
                nextActionTime = Time.time + 10;
                jigsawVideoPlayer.Play();

            }
            else if(string.Equals(current_turn, "right"))
            {
                current_turn = "wait_tablet";
                nextActionTime = Time.time + 3;
            }
            else if(string.Equals(current_turn, "wait"))
            {
                current_turn = "right";
                jigsawVideoPlayer.Pause();
                nextActionTime = Time.time + 3;
            }
            else if (string.Equals(current_turn, "wait_tablet"))
            {
                current_turn = "left";
                nextActionTime = Time.time + 3;
            }

        }
    
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        Debug.Log("Executing loop");
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;
        Debug.Log("Executing loop");
        yield return new WaitForSeconds(time);

        if (turnLeft)
        {
            neck.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
            turnLeft = false;
            Debug.Log("Executing loop left");

        }
        else 
        {
            neck.transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0);
            Debug.Log("Executing loop right");
            turnLeft = true;
        }

        // Code to execute after the delay
        isCoroutineExecuting = false;
    }
}
