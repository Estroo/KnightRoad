using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    private bool isOn = false;
    PlayerControl player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        if (player.onPause == false) 
        {
            if (Input.GetButtonDown("Lantern"))
            {
                isOn = !isOn;
                if (isOn == true)
                    StartCoroutine(LightUp());
                else
                    StartCoroutine(LightDown());
            }
        }
    }

    IEnumerator LightUp()
    {
        for (float f = 0; f <= 0.20f; f += Time.deltaTime)
        {
            gameObject.GetComponent<Light>().intensity = Mathf.Lerp(0f, 1.25f, f / 0.20f);
            yield return null;
        }
        gameObject.GetComponent<Light>().intensity = 1.25f;
    }

    IEnumerator LightDown()
    {
        for (float f = 0; f <= 0.20f; f += Time.deltaTime)
        {
            gameObject.GetComponent<Light>().intensity = Mathf.Lerp(1.25f, 0f, f / 0.20f);
            yield return null;
        }
        gameObject.GetComponent<Light>().intensity = 0;
    }
}
