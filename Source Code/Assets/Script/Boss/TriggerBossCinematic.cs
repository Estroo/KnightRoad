using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBossCinematic : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject BossName;
    public GameObject MainCamera;
    public GameObject bossFightView;
    public GameObject SecondBox;
    public GameObject invisibleWall;

    public AudioSource BossFightMusic;

    private CompleteCameraController myCamera;
    private PlayerControl PlayerScript;

    private BossScript Boss;

    public GameObject BossPrefab;
    public GameObject positionToSpawn;

    private Color Transparent = new Color(255, 255, 255, 0);

    private bool started = false;

    private void Start()
    {
        myCamera = FindObjectOfType<CompleteCameraController>();
        PlayerScript = FindObjectOfType<PlayerControl>();
    }

    private void Update()
    {
        if (PlayerScript.dead == true && started == true)
        {
            myCamera.Target = player;
            invisibleWall.SetActive(false);
            SecondBox.GetComponent<BoxCollider2D>().enabled = true;
            BossFightMusic.Stop();
            // Start ambient music
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Instantiate(BossPrefab, positionToSpawn.transform.position, Quaternion.identity);
            Boss = GameObject.Find("Boss-Child").GetComponent<BossScript>();
            started = true;
            invisibleWall.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            PlayerScript.inCinematic = true;
            myCamera.Smoothvalue = 2f;
            myCamera.PosY = 0.5f;
            myCamera.Target = boss;
            // Play an animation for the boss
            // Stop ambient music
            BossFightMusic.Play();
            StartCoroutine(FadeIn());
            StartCoroutine(returnToPlayer(5f));
        }
                
    }

    IEnumerator returnToPlayer(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(ZoomOutCamera());
        myCamera.Smoothvalue = 5f;
        myCamera.PosY = 2.5f;
        myCamera.Target = bossFightView;
        PlayerScript.inCinematic = false;
        Boss.StartAgain();
    }

    IEnumerator FadeIn()
    {
        StartCoroutine(ZoomCamera());
        BossName.SetActive(true);
        for (float f = 0; f <= 1.5f; f += Time.deltaTime)
        {
            BossName.GetComponent<Text>().color = Color.Lerp(Transparent, Color.white, f / 1.5f);
            yield return null;
        }
        BossName.GetComponent<Text>().color = Color.white;
        StartCoroutine(WaitStp(4f));
    }

    IEnumerator WaitStp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        for (float f = 0; f <= 1.5f; f += Time.deltaTime)
        {
            BossName.GetComponent<Text>().color = Color.Lerp(Color.white, Transparent, f / 1.5f);
            yield return null;
        }
        BossName.GetComponent<Text>().color = Transparent;
        BossName.SetActive(false);
    }

    IEnumerator ZoomCamera()
    {
        for (float f = 0; f <= 0.75f; f += Time.deltaTime)
        {
            MainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(7.5f, 5f, f / 0.75f);
            yield return null;
        }
    }

    IEnumerator ZoomOutCamera()
    {
        for (float f = 0; f <= 0.75f; f += Time.deltaTime)
        {
            MainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(5f, 7.5f, f / 0.75f);
            yield return null;
        }
    }
}
