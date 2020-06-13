using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Light mySun;

    Vector3 startPoint1 = new Vector3(-15, -28, 72);
    Vector3 heightPoint1 = new Vector3(1, 37, 72);
    Vector3 finishPoint1 = new Vector3(15, -28, 72);

    float daytimeCounter = 0.0f;
    float sunIntensity = 0.0f;

    void FixedUpdate()
    {
        HandleDayNight();
    }

    void HandleDayNight()
    {
        if (daytimeCounter < 0.50f)
        {
            daytimeCounter += 0.01f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startPoint1, heightPoint1, daytimeCounter);
            Vector3 m2 = Vector3.Lerp(heightPoint1, finishPoint1, daytimeCounter);
            mySun.GetComponent<Transform>().localPosition = Vector3.Lerp(m1, m2, daytimeCounter);
            if (sunIntensity < 1.00f)
            {
                sunIntensity += 0.02f * Time.deltaTime;
                mySun.intensity = Mathf.Lerp(0f, 0.7f, sunIntensity);
            }
        }
        if (daytimeCounter < 1.00f && daytimeCounter >= 0.50f)
            StartCoroutine(GoNight(30.0f));
    }

    IEnumerator GoNight(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (daytimeCounter < 1.00f && daytimeCounter >= 0.50f)
        {
            daytimeCounter += 0.01f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(startPoint1, heightPoint1, daytimeCounter);
            Vector3 m2 = Vector3.Lerp(heightPoint1, finishPoint1, daytimeCounter);
            mySun.GetComponent<Transform>().localPosition = Vector3.Lerp(m1, m2, daytimeCounter);
            if (sunIntensity > 0.00f)
            {
                sunIntensity -= 0.02f * Time.deltaTime;
                mySun.intensity = Mathf.Lerp(0f, 0.7f, sunIntensity);
            }
        }
        if (daytimeCounter >= 1.00f)
            StartCoroutine(GoDay(30.0f));
    }
    IEnumerator GoDay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        daytimeCounter = 0.0f;
        sunIntensity = 0.0f;
    }
}
