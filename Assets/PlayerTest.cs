using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Damage alýndý: " + amount);
        //if (health <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü!");
        // Destroy(gameObject); vs.
    }
}
