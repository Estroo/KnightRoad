using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFootstep : MonoBehaviour
{
    private PlayerControl playercontrol;

    private void Start()
    {
        GameObject thePlayer = GameObject.Find("Player");
        playercontrol = thePlayer.GetComponent<PlayerControl>();
    }
    
    public void FootstepsRandom()
    {
        if (playercontrol.onGrass == true && playercontrol.inWater == false)
        {
            AudioClip SoundToPlay = playercontrol.FootstepsGrass[Random.Range(0, playercontrol.FootstepsGrass.Length)];
            if (playercontrol.isGrounded == false)
            {
                playercontrol.GrassFootstep.Stop();
                return;
            }
            playercontrol.GrassFootstep.PlayOneShot(SoundToPlay);
        }
        if (playercontrol.onSnow == true && playercontrol.inWater == false)
        {
            AudioClip SoundToPlay = playercontrol.FootstepsSnow[Random.Range(0, playercontrol.FootstepsSnow.Length)];
            if (playercontrol.isGrounded == false)
            {
                playercontrol.SnowFootstep.Stop();
                return;
            }
            playercontrol.SnowFootstep.PlayOneShot(SoundToPlay);
        }
        if (playercontrol.onChest == true && playercontrol.inWater == false)
        {
            AudioClip SoundToPlay = playercontrol.FootstepsChest[Random.Range(0, playercontrol.FootstepsChest.Length)];
            if (playercontrol.isGrounded == false)
            {
                playercontrol.ChestFootstep.Stop();
                return;
            }
            playercontrol.SnowFootstep.PlayOneShot(SoundToPlay);
        }
        if (playercontrol.inWater == true)
        {
            AudioClip SoundToPlay = playercontrol.FootstepsWater[Random.Range(0, playercontrol.FootstepsWater.Length)];
            if (playercontrol.isGrounded == false)
            {
                playercontrol.WaterFootstep.Stop();
                return;
            }
            playercontrol.WaterFootstep.PlayOneShot(SoundToPlay);
        }
    }
}
