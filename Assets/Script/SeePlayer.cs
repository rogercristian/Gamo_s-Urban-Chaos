using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SeePlayer : MonoBehaviour
{

    private AIDestinationSetter destinationSetter;

    [SerializeField]
    private float waitForSeek = .2f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector2 detectArea = new Vector2 (1f,1f);

    [SerializeField]
    private float detectAngle = 3f;

    public LayerMask maskLayer;


    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();

        destinationSetter.enabled = false;
    }
    private void Update()
    {
        //StartCoroutine(WaitToSeek());
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D[] detectPlayer = Physics2D.OverlapBoxAll(target.position, detectArea, detectAngle, maskLayer);

        foreach (Collider2D hit in detectPlayer)
        {
            StartCoroutine(WaitToSeek());
            if (!hit)
            {
                destinationSetter.enabled = false;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (target == null)
            return;

        // Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireCube(target.position, detectArea);
    }
    IEnumerator WaitToSeek()
    {
        yield return new WaitForSeconds(waitForSeek);
        destinationSetter.enabled = true;
    }
}
