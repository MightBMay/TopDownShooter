using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{ 
        public MovementStats mStats;
        public EnemyCombatStats cStats;
        public StatMultipliers statMultipliers;
        DamageNumbers damageNumbers;
    private void Start()
    {
        damageNumbers = GetComponentInChildren<DamageNumbers>();
    }
    public void TakeDamage(float damage,float trueDamage)
    {
         cStats.curHp -= trueDamage;
         cStats.curHp -= damage / (1 + (cStats.defence / 100));
         damageNumbers.CreateDamageNumber(damage, trueDamage);
         if(cStats.curHp <= 0) { Kill(0); }
    }
    public void Kill(float delay)
    {
        Destroy(gameObject, delay);
    }

}
[System.Serializable]
public class EnemyCombatStats : CombatStats
{
    public bool canAttack;
    public float attackRange;

}