using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 mouseClickPos;
    float minShootLen = 4000;
    float maxShootLen = 6000;
    bool pressed = false;

    bool closeToLaunchPad = false;
    GameObject thislaunchpad;
    public CameraController mCamController;

    Vector3 mVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // Debug.Log(transform.position.y);
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
                Debug.Log(dist);
                float distancetonext = 0.0f;
                if (dist <= 1.1)
                {
                    factor = 100;
                    Debug.Log("Perfect " + factor);
                    distancetonext = 25;
                }
                else if (dist <= 1.3)
                {
                    factor = 80;
                    Debug.Log("Very Good " + factor);
                    distancetonext = 17;
                }
                else if (dist <= 1.5F)
                {
                    factor = 65;
                    Debug.Log("Good " + factor);
                    distancetonext = 14;

                }
                else
                {
                    factor = -70;
                    Debug.Log("Bad " + factor);

                }
                float LY = thislaunchpad.transform.position.y;

                
                thislaunchpad.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(LY+distancetonext-3, LY+distancetonext), 1);
                Vector3 currVel = gameObject.GetComponent<Rigidbody>().velocity;

                if (factor >= 0)
                {
                    mCamController.PlayerLaunched();
                }
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVel.x, 0, 0);
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100.0f * factor, 0));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!closeToLaunchPad && pressed)
            {
                Time.timeScale = 1.0f;
                pressed = false;
                Vector2 Releasepos = Input.mousePosition;
                Vector2 Difference = Releasepos - mouseClickPos;

                float len = Difference.magnitude * 10;
                //Debug.Log(Difference.magnitude);
                len = Mathf.Clamp(len, minShootLen, maxShootLen);
                Difference = Difference.normalized;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce(-Difference * len);
            }
        }

     if (!pressed)
        {
            Time.timeScale = 1.0f;
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
