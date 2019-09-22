using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shinde : MonoBehaviour
{

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Test2_Pads_Levers_Platforms");
            Debug.Log("working");
        }
    }

}

