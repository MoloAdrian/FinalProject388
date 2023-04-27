using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showandunshow : MonoBehaviour
{
    private float showTime = .15f;
    private float timeShown = 0.0f;
    bool activating = false;
    private Image mImage;
   
    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

        if (activating)
        {
            timeShown += Time.deltaTime;
            if (timeShown < showTime / 2)
            {
                Vector4 color = mImage.color;
                float percentage = timeShown / (showTime / 2);
                color.w = Mathf.Lerp(0, 1, percentage);
                mImage.color = color;
            }
            else
            {

                Vector4 color = mImage.color;
                float percentage = (timeShown - showTime / 2) / (showTime / 2);
                color.w = Mathf.Lerp(1, 0, percentage);
                mImage.color = color;
            }

            if (timeShown > showTime)
            {
                timeShown = 0.0f;
                activating = false;
            }
        }
    }

    public void Activate()
    {
        activating = true;

    }
}
