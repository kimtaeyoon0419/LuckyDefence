// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float currentAttackSpeed;
    [SerializeField] protected float attackRange;

    [Header("CurrentEnemy")]
    [SerializeField] protected Monster currentEnemy;
    [SerializeField] protected LayerMask enemyLayer;

    private void Update()
    {
        currentEnemy = FindEnemy();
        SetAttackSpeed();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }

    protected void SetAttackSpeed()
    {
        if(currentAttackSpeed <= 0)
        {
            if (currentEnemy != null)
            {
                Attack();
                currentAttackSpeed = attackSpeed;
            }
            else
            {
                currentAttackSpeed = 0;
            }
        }
        else if(currentAttackSpeed > 0)
        {
            currentAttackSpeed -= Time.deltaTime;
        }
    }

    protected Monster FindEnemy()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);
        Debug.Log(collider != null ? "Collider found!" : "Collider is null");

        if (collider != null)
        {
            Monster monster = collider.GetComponent<Monster>();
            return monster;
        }

        return null;
    }

    protected abstract void Attack();
}
