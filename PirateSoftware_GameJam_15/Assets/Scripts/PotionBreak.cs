using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBreak : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if(PotionSound.instance != null) PotionSound.instance.PlaySound();
            Destroy(gameObject, 0.1f);
        }
    }
}
