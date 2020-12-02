using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerIndicatorAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<Enemy>().isInDangerZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<Enemy>().isInDangerZone = false;
        }
    }
}
