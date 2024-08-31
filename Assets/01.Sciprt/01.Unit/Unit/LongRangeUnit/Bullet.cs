using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Monster target;
    [SerializeField] public float attackDamage;

   

    public void SetupBullet(Monster target, float attackDamage)
    {
        this.target = target;
        this.attackDamage = attackDamage;

        StartCoroutine(Co_Attack());
    }

    private void Update()
    {
        if (target == null)
        {
            StopAllCoroutines();
            ObjectPool.Instance.ReturnToPool("UnitBullet", gameObject);
        }
    }

    IEnumerator Co_Attack()
    {
        while (target != null)
        { 

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            yield return null;

            if((transform.position - target.transform.position).sqrMagnitude < speed * Time.deltaTime * speed * Time.deltaTime)
            {
                transform.position = target.transform.position;
                break;
            }
        }
        target.TakeDamage(attackDamage);
        Die();
    }
    
    private void Die()
    {
        ObjectPool.Instance.ReturnToPool("UnitBullet", gameObject);
    }
}