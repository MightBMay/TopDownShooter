using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Stats : MonoBehaviour
{
    public MovementStats mStats;
    public CombatStats cStats;
    public StatMultipliers statMultipliers;
    public GameStats gameStats;
    DamageNumbers damageNumber;
    private void Start()
    {
        damageNumber = GetComponentInChildren<DamageNumbers>();
    }
    private void FixedUpdate()
    {
        gameStats.timeAlive = Time.time;
    }
    public void TakeDamage(float damage,float trueDamage)
    {
        cStats.curHp -= trueDamage;
        cStats.curHp -= damage / (1 + (cStats.defence / 100));
        damageNumber.CreateDamageNumber(damage, trueDamage);
        //if (cStats.curHp <= 0) { Kill(0); }------------------------------------------------------------------------------------------------------------------------------------------------
    }
    public void Kill(float delay)
    {
        Destroy(gameObject, delay);
    }
}


[System.Serializable]
public class CombatStats
{
    public float curHp;
    public float maxHp;
    public float attack;
    // allows me to have a range of dmg values, example attack = 10, variance = 2, real damage could be 8-12.
    public float AttackVariance;
    public float defence;
    public float FireRate;
    public float GetHealthPercent(){
        return curHp / maxHp;
        
    }
    public string GetHealthString()
    {
        return Mathf.Round(curHp*10)/10   + "/" + maxHp;
    }
}
[System.Serializable]
public class MovementStats
{
    public float moveSpeed;

}

[System.Serializable]
public class StatMultipliers
{
    public float maxHpMultiplier;
    public float attackMultiplier;
    public float defenceMultiplier;
    public float attackRateMultiplier;
    public float reloadSpeedMultiplier;
    public float moveSpeedMultiplier;
    public float bulletSpeedMultiplier;
}
[System.Serializable]
public class GameStats
{
    public float kills;
    public float timeAlive;
    public float money;
}
