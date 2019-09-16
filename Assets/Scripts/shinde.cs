using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shinde : MonoBehaviour
{

    // Use this for initialization
    void OnCollision(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("levelTest");
            Debug.Log("working");
        }
    }

}

