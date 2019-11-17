using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sfx_Player : MonoBehaviour
{
    public AudioClip footsteps;
    public AudioClip jumpSfx;
    public AudioSource audioS;
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void Footsteps()
    {
        audioS.PlayOneShot(footsteps);
    }
    void Jump()
    {
        audioS.PlayOneShot(jumpSfx);
    }
}
