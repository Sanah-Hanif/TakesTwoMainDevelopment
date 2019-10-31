using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sfx_Player : MonoBehaviour
{
    public AudioClip footsteps;
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
}
