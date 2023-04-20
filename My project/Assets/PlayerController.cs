using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

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
    public ParticleSystem mLaunchParticles;
    public ParticleSystem mBounceParticlesRight;
    public ParticleSystem mBounceParticlesLeft;
    public Text mHowGoodText;
    public Text mJumpsText;
    public Text mHeightText;
    public float mJumps;
    bool puffed = false;

    public GameObject Arrow;
    float mInitialHeight;
    float mMaxHeight;

    public GameObject mAimSphere1;
    public GameObject mAimSphere2;
    public GameObject mAimSphere3;

    // Start is called before the first frame update
    void Start()
    {
        mLaunchParticles.Stop();
        mInitialHeight = transform.position.y;
        mMaxHeight = mInitialHeight;


    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y > mMaxHeight - mInitialHeight)
        {
            mMaxHeight = transform.position.y + mInitialHeight;
        }
        mHeightText.text = new string( mMaxHeight.ToString("F1") + " m");

        mJumpsText.text = mJumps.ToString();
        if (mJumps ==0)
        {
            mJumpsText.color = Color.red;
        }
        if (mJumps == 1)
        {
            mJumpsText.color = Color.black;
        }
        if (mJumps == 2)
        {
            mJumpsText.color = Color.green;
        }if (mJumps > 2)
        {
            mJumpsText.color = Color.yellow;
        }
       // Debug.Log(transform.position.y);
        if (Input.GetMouseButtonDown(0))
        {
            if ( !closeToLaunchPad  )
            {
                if (mJumps > 0)
                {
                    mouseClickPos = Input.mousePosition;
                    pressed = true;
                    Time.timeScale = 0.4f;
                }
                else
                {
                    //if (!puffed)
                    //{
                    //    gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5000.0f, 0));
                    //    puffed = true;
                    //    Debug.Log("Puf");
                    //}

                }
            }
            else
            {
                pressed = false;
                //compute distance to launchpad
                float dist = (gameObject.transform.position - thislaunchpad.transform.position).magnitude;
                float factor = 1;
                Debug.Log(dist);
                float distancetonext = 0.0f;
                if (dist <= 1.1f)
                {
                    factor = 100;
                    Debug.Log("Perfect " + factor);
                    distancetonext = 25;
                    mHowGoodText.text = "Perfect!";
                }
                else if (dist <= 1.4f)
                {
                    factor = 80;
                    Debug.Log("Very Good " + factor);
                    distancetonext = 17;
                    mHowGoodText.text = "Very Good!";

                }
                else if (dist <= 1.6F)
                {
                    factor = 65;
                    Debug.Log("Good " + factor);
                    distancetonext = 14;
                    mHowGoodText.text = "Good!";


                }
                else
                {
                    //factor = -70;
                    //Debug.Log("Bad " + factor);

                }
                float LY = thislaunchpad.transform.position.y;

                
                thislaunchpad.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(LY+distancetonext-3, LY+distancetonext), 1);
                Vector3 currVel = gameObject.GetComponent<Rigidbody>().velocity;

                mLaunchParticles.Play();
                if (factor >= 0)
                {
                    mCamController.PlayerLaunched();
                }
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(currVel.x, 0, 0);
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100.0f * factor, 0));
                puffed = false;
                mJumps++;
            }
        }

        if (Input.GetMouseButtonUp(0) )
        {
            if (!closeToLaunchPad && pressed)
            {


                mAimSphere1.transform.position = new Vector3(-100,-2000,3000);
                mAimSphere2.transform.position = new Vector3(-100,-2000,3000);
                mAimSphere3.transform.position = new Vector3(-100, -2000, 3000);
                if (mJumps > 0)
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
                    mJumps--;
                }
            }
        }

     if (!pressed)
        {
            Time.timeScale = 1.0f;
        }

     if (pressed)
        {
            //aim logic

            //direction to which the ball will go
            Vector2 Releasepos = Input.mousePosition;
            Vector2 Difference = -0.001f * (Releasepos - mouseClickPos);

            Vector3 Difference3d = new Vector3(Difference.x, Difference.y, 0);

            //positions for balls
            mAimSphere1.transform.position = transform.position + Difference3d;
            mAimSphere2.transform.position = transform.position + 2 * Difference3d - new Vector3(0,.1f,0);
            mAimSphere3.transform.position = transform.position + 3 * Difference3d - new Vector3(0, .3f, 0);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "LaunchPad")
        {
            closeToLaunchPad = true;
            thislaunchpad = other.gameObject;
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LaunchPad")
        {
            closeToLaunchPad = false;
            thislaunchpad = null;
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.transform.position.x > transform.position.x)
            {
                mBounceParticlesRight.Play();
            }
            else
            {
                mBounceParticlesLeft.Play();

            }
           
        }
    }


}
