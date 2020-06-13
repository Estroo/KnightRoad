using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNewZone : MonoBehaviour
{
    public Text ZoneText;
    public string ZoneName;

    private Color Transparent = new Color(255, 255, 255, 0);
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            ZoneText.GetComponent<Text>().text = ZoneName;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        ZoneText.enabled = true;
        for (float f = 0; f <= 2f; f += Time.deltaTime)
        {
            ZoneText.GetComponent<Text>().color = Color.Lerp(Transparent, Color.white, f / 2f);
            yield return null;
        }
        ZoneText.GetComponent<Text>().color = Color.white;
        StartCoroutine(WaitStp(2f));
    }

    IEnumerator WaitStp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (float f = 0; f <= 2f; f += Time.deltaTime)
        {
            ZoneText.GetComponent<Text>().color = Color.Lerp(Color.white, Transparent, f / 2f);
            yield return null;
        }
        ZoneText.GetComponent<Text>().color = Transparent;
        ZoneText.enabled = false;
    }
}
