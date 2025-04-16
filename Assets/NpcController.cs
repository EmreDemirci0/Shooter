using Akila.FPSFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NpcController : MonoBehaviour
{
    [Header("General Settings")]
    public Transform gunTip;
    public float degree = 30f;
    public float rotationSpeed = 1.5f;

    [Header("Detection Distances")]
    public float startFiringDistancemin = 5f;
    public float startFiringDistancemax = 8f;
    public float stopFiringDistancemin = 8f;
    public float stopFiringDistancemax = 8f;

    [Header("Raycast & Damage")]
    public float rayLength = 20f;
    public float timeToDamage = 1f;
    public float damageAmount = 10f;

    private Transform targetPlayer;
    private float timeLookingAtPlayer = 0f;

    private NavMeshAgent agent;
    private Animator anim;
    private bool isFiring = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // NavMeshAgent rotasyonu yönetmesin, biz elle kontrol edeceğiz
        agent.updateRotation = false;
    }

    private bool hasRotatedToPlayer = false; // Dönüşün yapılıp yapılmadığını takip edeceğiz
    private bool wasFiring = false;
    void Update()
    {
        if (GetComponent<Damageable>().health <= 0)
        {
             agent.enabled = false;
            return;
        }
        FindNearestPlayer();

        if (targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, targetPlayer.position); 

            if (!isFiring && distance <= 5/*Random.Range(startFiringDistancemin,startFiringDistancemax)*/ )
            {
                isFiring = true;
                ////Debug.Log("🚨 Target within range — START FIRING");
            }
            else if (isFiring && distance > 8/*Random.Range(stopFiringDistancemin, stopFiringDistancemax)*/)
            {
                isFiring = false;
                ////Debug.Log("🔁 Target left range — STOP FIRING");
            }

            // Eğer daha önce oyuncuya dönülmediyse ve yürüyorsa, dönsün
            if (!hasRotatedToPlayer && anim.GetBool("isWalking") && !isFiring)
            {
                RotateTowardsTarget();
                hasRotatedToPlayer = true; // Bir kere dönme işlemi yapıldıktan sonra flag'i true yapıyoruz
            }

            // Diğer işlemler
            RotateTowardsTarget();

            if (CanSeePlayer())
            {
                if (isFiring)
                {
                    if (!wasFiring)
                    {
                        int randomFire = Random.Range(0, 2); // Sadece bir kere seç
                        anim.SetInteger("fireType", randomFire);
                        wasFiring = true;
                    }
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isFiring", true);
                  
                    agent.ResetPath();
                    //Debug.Log("🔫 Firing at visible player");
                }
                else
                {
                    wasFiring = false;
                    anim.SetBool("isFiring", false);
                    MoveTowardsTarget();
                }
            }
            else
            {
                anim.SetBool("isFiring", false);
                //Debug.Log("🙈 Player NOT in line of sight — repositioning");
                MoveTowardsTarget();
            }

            PerformRaycastCheck();
        }
        else
        {
            agent.ResetPath();
            anim.SetBool("isWalking", false);
            anim.SetBool("isFiring", false);
            isFiring = false;
            //Debug.Log("❌ No target found");
        }
    }

    void RotateTowardsTarget()
    {
        if (targetPlayer == null) return;

        if (isFiring)
        {
            // Ateş ederken hedefe anlık dön
            Vector3 lookPos = new Vector3(targetPlayer.position.x, transform.position.y, targetPlayer.position.z);
            Vector3 direction = (lookPos - transform.position).normalized;

            if (direction.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0, degree, 0);
                transform.rotation = targetRotation;
                //Debug.Log("🔄 Instantly rotated to FIRE (with offset)");
            }
        }
        else if (anim.GetBool("isWalking") && !isFiring && !hasRotatedToPlayer)
        {
            // Eğer isWalking true ve isFiring false ise, sadece bir kere oyuncuya dön
            Vector3 lookPos = new Vector3(targetPlayer.position.x, transform.position.y, targetPlayer.position.z);
            Vector3 direction = (lookPos - transform.position).normalized;

            if (direction.magnitude > 0.01f)
            {
                // Hemen dönüş yap
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
                //Debug.Log("🔄 Instantly rotated to PLAYER while walking");
            }

            // Bir kere dönüş yaptıktan sonra bayrağı true yapıyoruz
            hasRotatedToPlayer = true;
        }
        else if (agent.velocity.sqrMagnitude > 0.01f)
        {
            // Yürürken NavMeshAgent’in hareket yönüne doğru dön
            Quaternion moveRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, rotationSpeed * Time.deltaTime);
            //Debug.Log("🔄 Smooth rotating to WALK");
        }
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

    void MoveTowardsTarget()
    {
        if (agent != null && targetPlayer != null)
        {
            agent.SetDestination(targetPlayer.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                if (!anim.GetBool("isWalking"))
                    anim.SetBool("isWalking", true);

                if (anim.GetBool("isFiring"))
                    anim.SetBool("isFiring", false);

                ////Debug.Log("🏃 Moving toward player");
            }
            else
            {
                ////Debug.Log("🛑 Arrived near player");
            }
        }
    }

   
    bool CanSeePlayer()
    {
        if (gunTip == null || targetPlayer == null) return false;

        Vector3 directionToPlayer = (targetPlayer.position + Vector3.up * 0.5f - gunTip.position).normalized;
        Ray ray = new Ray(gunTip.position, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.GetComponent<FirstPersonController>())
            {
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);
                //Debug.Log("👁️ Görüş hattı AÇIK");
                return true;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow);
                //Debug.Log($"🚧 Line of sight BLOCKED by: {hit.collider.name}");
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.gray);
            //Debug.Log("🌫️ Raycast did not hit anything");
        }

        return false;
    }

    void PerformRaycastCheck()
    {
        if (gunTip == null || targetPlayer == null || !isFiring) return;

        Ray ray = new Ray(gunTip.position, gunTip.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            FirstPersonController playerHit = hit.collider.GetComponent<FirstPersonController>();
            if (playerHit)
            {
                timeLookingAtPlayer += Time.deltaTime;

                if (timeLookingAtPlayer >= timeToDamage)
                {
                    //Debug.Log("💥 Target locked for 1 sec — Apply damage");
                    //playerHit.TakeDamage(damageAmount);
                    timeLookingAtPlayer = 0f;
                }
            }
            else
            {
                timeLookingAtPlayer = 0f;
            }
        }
        else
        {
            timeLookingAtPlayer = 0f;
        }
    }
}