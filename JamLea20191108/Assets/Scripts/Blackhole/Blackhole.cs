using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole: MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Plant>() != null)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
        }

        if(collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().Die();
        }

        Destroy(collision.gameObject);
    }
}
