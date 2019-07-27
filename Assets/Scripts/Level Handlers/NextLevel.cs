using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == Player.Instance.GetComponent<Collider2D>())
        {
            SceneLoader.LoadNextScene();
        }
    }
}
