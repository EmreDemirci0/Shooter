using Akila.FPSFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Daha okunaklı olsun diye kısalttım

public class NpcTest : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform targetPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //FindNearestPlayer(); // Başlangıçta en yakın oyuncuyu bul
    }

    void Update()
    {
        //if (targetPlayer == null)
        //{
        //    FindNearestPlayer(); // Hedef yoksa tekrar bul
        //}
        //else
        //{
        //    agent.SetDestination(targetPlayer.position);
        //}
    }

    void FindNearestPlayer()
    {
        FirstPersonController[] players = FindObjectsOfType<FirstPersonController>();
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (FirstPersonController p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = p.transform;
            }
        }

        targetPlayer = closest;

        if (targetPlayer != null)
            Debug.Log($"🎯 Found nearest player: {targetPlayer.name}");
    }
}
