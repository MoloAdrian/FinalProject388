using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    Vector2 mouseClickPos;
    float minShootLen = 4000;
    float maxShootLen = 6000;
    bool pressed = false;

    float mPressTime = 0.0f;

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

    public GameObject mBar;
    public ParticleSystem mGainJumpParts;
    private float mBarZeroPos;
    private float mBarMaxPos;
    float mBarCharge = .0f;
    float mBarDischargeSpeed = 10.0f;
    bool mEmptying = false;

    public AudioClip mHoldClip;
    public AudioClip mJumpClip;
    public AudioClip mLaunchClip;
    public AudioClip mHurtClip;

    public Text DebugText;

    public GameObject mBouncesounder;
    private AudioSource mBounceSource;

    public showandunshow mHurtScreen;

    bool mHasStarted = false;
    public bool lost = false;

    float timeBeingStill = 0.0f;
    Rigidbody mRB;

    float mForceMultiplyer;


    private AudioSource mAudioSource;

    public bool isTutorial;
    int tutorialPhase = 0;
    public GameObject TutorialObj;
    // Start is called before the first frame update
    void Start()
    {
        mLaunchParticles.Stop();
        mGainJumpParts.Stop();

        mBounceParticlesRight.Stop();
        mBounceParticlesLeft.Stop();
        mInitialHeight = transform.position.y;
        mMaxHeight = mInitialHeight;

        mBarZeroPos = -2.694f;
        mBarMaxPos = 0;
        mAudioSource = GetComponent<AudioSource>();

        mBounceSource = mBouncesounder.GetComponent<AudioSource>();
        mRB = gameObject.GetComponent<Rigidbody>();
        mHasStarted = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        mForceMultiplyer =  1920.0f / Screen.height;

        if (PlayerPrefs.GetInt("DoneTutorial") == 0)
        {
            isTutorial = true;
            TutorialObj.transform.Find("1").gameObject.SetActive(true);
            TutorialObj.transform.Find("TutoImg").gameObject.SetActive(true);
            TutorialObj.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        if (FlowManager.isPaused)
        {
            pressed = false;
            mAudioSource.Stop();
            return;
        }
        if (lost)
        {
            return;
        }

        if (mHasStarted&& Vector3.Magnitude(mRB.velocity)< 0.1f && mJumps <=0)
        {
            timeBeingStill += Time.deltaTime;
            if (timeBeingStill >= 1.0f)
            {
                lost = true;
                mCamController.Lose();
            }
        }
        
        if (mEmptying)
        {
            mBarCharge -= 10 * mBarDischargeSpeed * Time.deltaTime;
            if (mBarCharge <= 0.0f)
            {
                mEmptying = false;
            }
        }
        else if (mBarCharge >= 0.0f)
        {
            mBarCharge -= mBarDischargeSpeed * Time.deltaTime;
        }

        if (transform.position.y > mMaxHeight - mInitialHeight)
        {
            mMaxHeight = transform.position.y + mInitialHeight;
        }
        mHeightText.text = new string( mMaxHeight.ToString("F1") + " m");

        mJumpsText.text = mJumps.ToString();
        if (!mEmptying)
        {
            if (mJumps == 0)
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
            }
            if (mJumps > 2)
            {
                mJumpsText.color = Color.yellow;
            }
        }
        // Debug.Log(transform.position.y);

#if UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
           
            DebugText.text = new string("Touch began");
#else
            if (Input.GetMouseButtonDown(0))
        {
#endif

           

            if ( !closeToLaunchPad  )
            {
                if (mJumps > 0)
                {
#if UNITY_ANDROID
                        mouseClickPos = Input.GetTouch(0).position;
#else

                        mouseClickPos = Input.mousePosition;
#endif

                        pressed = true;
                    Time.timeScale = 0.4f;

                    mAudioSource.Stop();
                    mAudioSource.clip = mHoldClip;
                    mAudioSource.Play();
                    if (isTutorial && tutorialPhase == 0)
                    {
                        Transform t = TutorialObj.transform.Find("TutoImg");
                        t.position -= new Vector3(0, 400, 0);
                        tutorialPhase++;
                        TutorialObj.transform.Find("1").gameObject.GetComponent<Text>().text = ("Drag and\n Release!");



                    }


                }
                
            }
            else
            {
                //Get components and if launchpad is not on cooldown, trigger it
                LaunchPad lpProperties = thislaunchpad.GetComponent<LaunchPad>();
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                if (!lpProperties.IsInCooldown())
                {
                    //compute distance to launchpad
                    pressed = false;
                    float dist = (gameObject.transform.position - thislaunchpad.transform.position).magnitude;
                    float factor = 1;
                    Debug.Log(dist);
                    float distancetonext = 0.0f;

                    //Depending on distance give boost to launch
                    if (dist <= 1.1f)
                    {
                        factor = 100;
                        Debug.Log("Perfect " + factor);
                        distancetonext = 25;
                        mHowGoodText.text = "Perfect!";
                        if (!mEmptying)
                            mBarCharge += 30.0f * lpProperties.mJumpBarFactor;
                    }
                    else if (dist <= 1.4f)
                    {
                        factor = 80;
                        Debug.Log("Very Good " + factor);
                        distancetonext = 17;
                        mHowGoodText.text = "Very Good!";
                        if (!mEmptying)
                            mBarCharge += 25.0f * lpProperties.mJumpBarFactor;


                    }
                    else if (dist <= 1.6F)
                    {
                        factor = 65;
                        Debug.Log("Good " + factor);
                        distancetonext = 14;
                        mHowGoodText.text = "Good!";
                        if (!mEmptying)
                            mBarCharge += 22.0f * lpProperties.mJumpBarFactor;
                    }
                    float LY = thislaunchpad.transform.position.y;

                    //Play camera transition if last launchpad of block
                    mLaunchParticles.Play();
                    if (factor >= 0 && lpProperties.mLastLP)
                        mCamController.PlayerLaunched();

                    //Set velocity and update launchpad for level generation
                    Vector3 currVel = rb.velocity;
                    if(lpProperties.mCancelPlayerVel)
                        rb.velocity = new Vector3(0, 0, 0);
                   else
                        rb.velocity = new Vector3(currVel.x, 0, 0);

                    //Add force of launchpad and set launchpad on cooldown
                    rb.AddForce(new Vector3(lpProperties.mDirection.x * lpProperties.mForce * factor, lpProperties.mDirection.y * lpProperties.mForce * factor, 0));
                    lpProperties.mAccuracyValue = distancetonext;
                    lpProperties.StartCooldown();
                    lpProperties.mTriggered = true;

                    //Remove launchpad
                    closeToLaunchPad = false;
                    thislaunchpad = null;
                    //GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    puffed = false;

                    //Give extra jump if launchpad gives it
                    if (lpProperties.mExtraJump)
                        mJumps++;
                    mAudioSource.Stop();
                    mAudioSource.clip = mLaunchClip;
                    mAudioSource.Play();

                    if (isTutorial && tutorialPhase == 3)
                    {
                        Time.timeScale = 1.0f;
                        tutorialPhase++;
                        TutorialObj.transform.Find("1").gameObject.SetActive(false);
                        TutorialObj.SetActive(false);
                        tutorialPhase = -1;

                    }
                }
            }
        }

#if UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            DebugText.text = new string("Touch Ended");
#else

            if (Input.GetMouseButtonUp(0))
            {
#endif

            if (!closeToLaunchPad && pressed)
            {

                
                mAimSphere1.transform.position = new Vector3(-100,-2000,3000);
                mAimSphere2.transform.position = new Vector3(-100,-2000,3000);
                mAimSphere3.transform.position = new Vector3(-100, -2000, 3000);

                if (mJumps > 0)
                {
                    if (!isTutorial || tutorialPhase == -1)
                    {
                        Time.timeScale = 1.0f;
                    }
                    pressed = false;

                    Vector2 Releasepos;

#if UNITY_ANDROID

                    Releasepos = Input.GetTouch(0).position;

#else
                    Releasepos = Input.mousePosition;

#endif

                    Vector2 Difference = Releasepos - mouseClickPos;

                    float len = mForceMultiplyer * Difference.magnitude * 10;
                    //Debug.Log(Difference.magnitude);
                    len = Mathf.Clamp(len, minShootLen, maxShootLen);
                    mAudioSource.Stop();

                    //Debug.Log(len);
                    if (len >= 4000f && mPressTime>= 0.07f)
                    {

                        if (isTutorial && tutorialPhase == 1)
                        {
                            TutorialObj.transform.Find("1").gameObject.SetActive(false);
                            TutorialObj.transform.Find("TutoImg").gameObject.SetActive(false);
                            tutorialPhase++;
                        }
                        
                        mHasStarted = true;
                        gameObject.GetComponent<Rigidbody>().useGravity = true;
                        
                        mAudioSource.clip = mJumpClip;
                        mAudioSource.Play();
                        Difference = Difference.normalized;
                        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                        rb.velocity = Vector3.zero;
                        rb.AddForce(-Difference * len);
                        mJumps--;
                    }

                    mPressTime = 0.0f;

                }
            }
        }

        if (!pressed)
        {
            if (!isTutorial || tutorialPhase != -1)
            {

                Time.timeScale = 1.0f;
            }
        }

        if (pressed)
        {
            mPressTime += Time.deltaTime;
            //aim logic

            //direction to which the ball will go
#if UNITY_ANDROID
            Vector2 Releasepos = Input.GetTouch(0).position;
            DebugText.text = new string(Releasepos.ToString("F1"));
            

#else
            Vector2 Releasepos = Input.mousePosition;
            
#endif
            Vector2 Difference = -0.001f * mForceMultiplyer* (Releasepos - mouseClickPos);

            Vector3 Difference3d = new Vector3(Difference.x, Difference.y, 0);

            //positions for balls
            mAimSphere1.transform.position = transform.position + Difference3d;
            mAimSphere2.transform.position = transform.position + 2 * Difference3d - new Vector3(0,.1f,0);
            mAimSphere3.transform.position = transform.position + 3 * Difference3d - new Vector3(0, .3f, 0);

        }


        SetBarPos();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("lp") || other.name == "LaunchPad")
        {
            closeToLaunchPad = true;
            thislaunchpad = other.gameObject;
            //GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            if (isTutorial && tutorialPhase== 2)
            {
                Time.timeScale = .05f;
                tutorialPhase++;
                TutorialObj.transform.Find("1").gameObject.SetActive(true);

                TutorialObj.transform.Find("1").gameObject.GetComponent<Text>().text = ("Tap to\n launch!");


            }
        }
        if (other.CompareTag("spikes"))
        {
            mJumps--;
            if (mJumps <=0)
            {
                lost = true;
                mCamController.Lose();
            }
            mHurtScreen.Activate();

            mAudioSource.Stop();
            mAudioSource.clip = mHurtClip;
            mAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("lp") || other.name == "LaunchPad")
        {
            closeToLaunchPad = false;
            thislaunchpad = null;
           // GetComponent<Renderer>().material.SetColor("_Color", Color.white);

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
            mBounceSource.Play();
           
        }
    }

    private void SetBarPos()
    {

        if (mBarCharge >= 100.0f)
        {
            mJumps++;
            mEmptying = true;
            mGainJumpParts.gameObject.SetActive(true);

            mGainJumpParts.Play();
            mBar.GetComponent<AudioSource>().Play();
            mJumpsText.color = new Vector4(0.41f,0.85f,1,1);
            
        }
        mBarCharge = Mathf.Clamp(mBarCharge, 0, 100);
        float normalizedCharge = mBarCharge / 100.0f;
        float pos = Mathf.Lerp(mBarZeroPos, mBarMaxPos, normalizedCharge);
        Vector3 pos3d = mBar.transform.position;
        pos3d.x = pos;
        mBar.transform.position = pos3d;
    }


}
