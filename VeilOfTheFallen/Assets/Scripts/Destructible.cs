using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage()
    {
        health = health - 1;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}