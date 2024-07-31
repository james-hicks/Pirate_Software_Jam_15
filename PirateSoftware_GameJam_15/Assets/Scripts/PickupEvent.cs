using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupEvent : MonoBehaviour
{

    public GameObject NewPotionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>()._potionPrefab = NewPotionPrefab;
            other.gameObject.GetComponent<PlayerController>().PickupPotion();
            Destroy(gameObject);
        }
    }
}
