using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPathRelic : MonoBehaviour
{
    private CompleteCameraController myCamera;
    private PlayerControl PlayerScript;

    public GameObject player;
    public GameObject sawDoorComplete1;
    public GameObject sawDoorComplete2;
    public GameObject sawDoorComplete3;
    public GameObject sawDoor1;
    public GameObject sawDoor2;
    public GameObject sawDoor3;

    void Start()
    {
        myCamera = FindObjectOfType<CompleteCameraController>();
        PlayerScript = FindObjectOfType<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            myCamera.Smoothvalue = 100;
            if (gameObject.name == "Relic1")
            {
                StartCoroutine(StartDestroyingDoorAnimation(sawDoorComplete1));
                myCamera.Target = sawDoor1;
            }
            else if (gameObject.name == "Relic2")
            {
                StartCoroutine(StartDestroyingDoorAnimation(sawDoorComplete2));
                myCamera.Target = sawDoor2;
            }
            else if (gameObject.name == "Relic3")
            {
                StartCoroutine(StartDestroyingDoorAnimation(sawDoorComplete3));
                myCamera.Target = sawDoor3;
            }
        }
    }

    IEnumerator StartDestroyingDoorAnimation(GameObject sawDoorComplete)
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(DestroyingDoor(sawDoorComplete));
    }

    IEnumerator DestroyingDoor(GameObject sawDoorComplete)
    {
        yield return new WaitForSeconds(1.0f);
        PlayerScript.inCinematic = true;
        Destroy(sawDoorComplete);
        StartCoroutine(ReturnToPlayer());
    }

    IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(3.0f);
        myCamera.Target = player;
        PlayerScript.inCinematic = false;
        StartCoroutine(SetCameraSmooth());
    }

    IEnumerator SetCameraSmooth()
    {
        yield return new WaitForSeconds(1.0f);
        myCamera.Smoothvalue = 5;
    }
}
