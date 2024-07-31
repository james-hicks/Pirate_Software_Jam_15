using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            MenuManager.instance.FinishedGame();
            other.GetComponent<PlayerController>().HasControl = false;
        }
    }
}
