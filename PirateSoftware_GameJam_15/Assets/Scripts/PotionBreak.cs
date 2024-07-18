using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBreak : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
