using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossAfterDeath : MonoBehaviour
{
    public GameObject invisibleWall;
    public GameObject bossFightView;
    public AudioSource BossFightMusic;
    private CompleteCameraController myCamera;

    public GameObject BossPrefab;
    public GameObject positionToSpawn;

    private BossScript Boss;
    private PlayerControl PlayerScript;

    private void Start()
    {
        myCamera = FindObjectOfType<CompleteCameraController>();
        PlayerScript = FindObjectOfType<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && PlayerScript.dead == false && PlayerScript.bossKilled == false)
        {
            Instantiate(BossPrefab, positionToSpawn.transform.position, Quaternion.identity);
            Boss = GameObject.Find("Boss-Child").GetComponent<BossScript>();
            Boss.StartAgain();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            invisibleWall.SetActive(true);
            myCamera.Target = bossFightView;
            // stop ambient music
            BossFightMusic.Play();
        }
    }
}
