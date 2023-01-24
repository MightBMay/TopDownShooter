using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(EnemyStats), typeof(EnemyMovement) )]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]Enemy thisEnemy;
    public EnemyStats eStats;
    public EnemyMovement eMove;
    Transform player;
    public Stats pStats;
    bool inRange,rangeEntered,rangeExited,attackCD;
    public Coroutine attackCorutine;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("PlayerBody").transform;
        pStats = player.GetComponent<Stats>();
        eStats = GetComponent<EnemyStats>();
        eMove = GetComponent<EnemyMovement>();
        eStats.cStats.canAttack = true;
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        RangeCheck();
    }
    public void AttackCoroutine(float time)
    {
       attackCorutine = StartCoroutine(AttackCooldown(time));
    }
    public IEnumerator AttackCooldown(float timer)
    {
        if (attackCD) { yield return null; }
        eStats.cStats.canAttack = false;
        attackCD = true;
        yield return new WaitForSeconds(timer);
        eStats.cStats.canAttack = true;
        attackCD = false;
    }
    private void InitializeEnemy()
    {
        thisEnemy = new BasicEnemy();
        thisEnemy.eBehaviour = this;
        thisEnemy.eMove = eMove;
        thisEnemy.eStats = eStats;
    }

    public void RangeCheck()
    {
        try {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= eStats.cStats.attackRange)
            {
                if (!inRange)
                {
                    inRange = true;
                    if (!rangeEntered)
                    {
                        rangeEntered = true;
                        thisEnemy.OnEnterRange();
                    }
                }

                thisEnemy.OnStayRange();
            }
            else
            {
                if (inRange)
                {
                    inRange = false;
                    if (!rangeExited)
                    {
                        rangeExited = true;
                        thisEnemy.OnExitRange();
                    }
                }
            } 
        }catch { Debug.LogWarning("Player Transform Not Found/Destroyed"); }
    
        
    }
}
[System.Serializable]
public class Enemy
{
    public EnemyBehaviour eBehaviour;
    public EnemyStats eStats;
    public EnemyMovement eMove;
    public string enemyName;
    public Enemy()
    {
        this.enemyName = "enemy";
    }
    public virtual void OnEnterRange()
    {
        
    }
    public virtual void OnStayRange()
    {

    }
    public virtual void OnExitRange()
    {

    }
    public virtual void Attack()
    {

    }
}
[System.Serializable]
public class MeleeEnemy: Enemy
{

}
[System.Serializable]
public class RangedEnemy : Enemy
{

}
[System.Serializable]
public class BasicEnemy : MeleeEnemy
{
    public BasicEnemy() {
        this.enemyName = "Basic Enemy";
        
    }
    public override void OnEnterRange()
    {
        Debug.Log("enter");
    }
    public override void OnStayRange()
    {
        if (eStats.cStats.canAttack) {  Attack();   }
    }
    public override void OnExitRange()
    {
        Debug.Log("exit");
    }
    public override void Attack()
    {
        eBehaviour.pStats.TakeDamage(eStats.cStats.attack,0);
        eBehaviour.AttackCoroutine(1/eStats.cStats.FireRate);

    }
}
