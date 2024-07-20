using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBreak : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
