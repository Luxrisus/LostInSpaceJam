using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole: MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ManagersManager.Instance.Get<LevelManager>().getLevelState() != LevelState.EndOfGame)
        {
            if (collision.gameObject.GetComponent<Plant>() != null)
            {
                ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
            }
            else if (collision.gameObject.GetComponent<Player>() != null)
            {
                collision.gameObject.GetComponent<Player>().Die();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        } 
        else
        {
            //Destroy(collision.gameObject);
        }
    }
}
