using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Text CheckpointText;
    private bool alreadyDone = false;
    private Color Transparent = new Color(255, 255, 255, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyDone == false && collision.gameObject.tag == "Player")
        {
            alreadyDone = true;
            DestroyGameObjectsWithTag("Spawn");
            transform.tag = "Spawn";
            StartCoroutine(FadeIn());
        }
    }

    public static void DestroyGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject target in gameObjects)
            Destroy(target);
    }

    IEnumerator FadeIn()
    {
        CheckpointText.enabled = true;
        for (float f = 0; f <= 2f; f += Time.deltaTime)
        {
            CheckpointText.GetComponent<Text>().color = Color.Lerp(Transparent, Color.white, f / 2f);
            yield return null;
        }
        CheckpointText.GetComponent<Text>().color = Color.white;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (float f = 0; f <= 2f; f += Time.deltaTime)
        {
            CheckpointText.GetComponent<Text>().color = Color.Lerp(Color.white, Transparent, f / 2f);
            yield return null;
        }
        CheckpointText.GetComponent<Text>().color = Transparent;
        CheckpointText.enabled = false;
    }
}