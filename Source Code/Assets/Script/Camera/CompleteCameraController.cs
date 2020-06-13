using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteCameraController : MonoBehaviour
{
    public GameObject Target;
    public float Smoothvalue = 2;
    public float PosY = 1;

    public Coroutine my_co;
    PlayerControl player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {

        if (Target && Input.GetAxisRaw("Horizontal") == 0 && player.onPause == false && player.inCinematic == false && PlayerControl.inDialog == false && Smoothvalue == 5) { 
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                Vector3 lol = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY + 2, -1);
                transform.position = Vector3.Lerp(transform.position, lol, Time.deltaTime * Smoothvalue);
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                Vector3 lol = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY - 2, -1);
                transform.position = Vector3.Lerp(transform.position, lol, Time.deltaTime * Smoothvalue);
            }
            else
            {
                Vector3 Targetpos = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY, -1);
                transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);
            }
        }
        else if (Target)
        {
            Vector3 Targetpos = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY, -1);
            transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);
        }

    }
}
