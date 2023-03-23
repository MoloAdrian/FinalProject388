using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 mouseClickPos;
    float minShootLen = 4000;
    float maxShootLen = 6000;
    bool pressed = false;

    bool closeToLaunchPad = false;
    GameObject thislaunchpad;

    Vector3 mVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!closeToLaunchPad)
            {
                mouseClickPos = Input.mousePosition;
                pressed = true;
                Time.timeScale = 0.4f;
            }
            else
            {
                pressed = false;
                //compute distance to launchpad
                float dist = (gameObject.transform.position - thislaunchpad.transform.position).magnitude;
                float factor = 1;
                if (dist <= 20)
                {
                    factor = 35;
                    Debug.Log("Perfect " + factor);
                }
                else if (dist <= 30)
                {
                    factor = 25;
                    Debug.Log("Very Good " + factor);
                }
                else if (dist <= 40)
                {
                    factor = 20;
                    Debug.Log("Good " + factor);

                }
                else
                {
                    Debug.Log("Bad " + factor);

                }

                thislaunchpad.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(-3, 4.75f), 0);
                Vector3 currVel = gameObject.GetComponent<Rigidbody>().velocity;


                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVel.x, 0, 0);
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100.0f * factor, 0));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!closeToLaunchPad)
            {
                Time.timeScale = 1.0f;
                pressed = false;
                Vector2 Releasepos = Input.mousePosition;
                Vector2 Difference = Releasepos - mouseClickPos;

                float len = Difference.magnitude * 10;
                Debug.Log(Difference.magnitude);
                len = Mathf.Clamp(len, minShootLen, maxShootLen);
                Difference = Difference.normalized;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce(-Difference * len);
            }
        }

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "LaunchPad")
        {
            closeToLaunchPad = true;
            thislaunchpad = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LaunchPad")
        {
            closeToLaunchPad = false;
            thislaunchpad = null;
        }
    }
}
