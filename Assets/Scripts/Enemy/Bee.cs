// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Bee : Enemy
// {
//     [Header("Movement Range")]
//     public float patrolRadius;
//     protected override void Awake()
//     {
//         base.Awake();
//         patrolState = new BeePatrolState();
//         chaseState = new BeeChaseState();
//     }

//     public override bool FoundPlayer()
//     {
//         var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
//         if (obj)
//         {
//             attacker = obj.transform;
//         }
//         return obj;
//     }

//     public override void OnDrawGizmosOnSelected()
//     {
//         Gizmos.DrawWireSphere(transform.position, checkDistance);
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireSphere(transform.position, patrolRadius);
//     }

//     public override Vector3 GetNewPoint()
//     {
//         var targetX = Random.Range(-patrolRadius, patrolRadius);
//         var targetY = Random.Range(-patrolRadius, patrolRadius);

//         return spawnPoint + new Vector3(targetX, targetY);
//     }

//     public override void Move()
//     {
        
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("Move area")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }
    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }

    public override void Move()
    {

    }
}
