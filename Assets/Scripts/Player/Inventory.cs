using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Inventory : MonoBehaviour
{
    [HideInInspector]   
    public List<Weapon> weapons = new List<Weapon>();
    [HideInInspector] public StatMultipliers statsMult;
    public ShotHandler shotHandler;
    public GameObject proj;
    bool shotTimer = false;
    bool isReloading = false;

    void Awake()
    {
        shotHandler = GetComponent<ShotHandler>();
        statsMult = GetComponent<Stats>().statMultipliers;
        AddWeapon(new CombatKnife(shotHandler,this));

    }
    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }
    /// <summary>
    /// Sets the first inventory slot, your current weapon to newWeapon. keepWeapon will add the original item at slot "To" back to the inventory.
    /// </summary>
    public void SetInventorySlot(Weapon newWeapon,bool keepWeapon)
    {
        if (keepWeapon) { weapons.Add(weapons[0]); }
        weapons[0] = newWeapon;
    }
    /// <summary>
    /// Sets inventory slot number "to" to newWeapon. keepWeapon will add the original item at slot "To" back to the inventory.
    /// </summary>
    public void SetInventorySlot(int to, Weapon newWeapon, bool keepWeapon)
    {
        if (keepWeapon) { weapons.Add(weapons[to]); }
        weapons[to] = newWeapon;
    }
    /// <summary>
    ///  Swaps weapon in slot "from" to slot "to" in the inventory.
    /// </summary>
    public void SwapInventorySlot(int from, int to)
    {
        Weapon temperaryStore = weapons[to];
        weapons[to] = weapons[from];
        weapons[from] = temperaryStore;
    }
    // used for autofiring shots. takes the weapon you fired, sets it so it cannot fire until the delay is met.
    public IEnumerator ShotTimer(Weapon wep, float delay)
    {
        if (shotTimer) { yield break; }
        wep.canFire = false;
        shotTimer = true;
        yield return new WaitForSeconds(delay);
        wep.canFire = true;
        shotTimer = false;

    }
    public IEnumerator ReloadWeapon(Weapon weapon, float reloadTime)
    {
        // takes a weapon as a ranged weapon so you can access ranged specific fields.
        RangedWeapon wep = weapon as RangedWeapon;
        // check if coroutine is already running.
        if (isReloading) { yield break; }
        // if you have no ammo, runs the no ammo method on the weapon, and returns.
        if(wep.storedAmmo <= 0) { 
            wep.OnNoAmmo();
            isReloading = false;
            wep.canFire = true;
            yield break;
        }
        // stops you from firing while reloading, waits for the reload to finish.
        isReloading = true;
        wep.canFire = false;
        yield return new WaitForSeconds(reloadTime);
        // takes the difference in mag size to current amount of bullets in the mag, then subtracts it from your stored ammo and fills the mag again.
        int bulletsUsed = wep.magSize - wep.curMag;
        if (bulletsUsed >= wep.storedAmmo)
        {// in the case where you dont have enough ammo for a full mag, it will fill it as much as it can.
            bulletsUsed = wep.storedAmmo;
            wep.curMag = wep.storedAmmo;
            wep.storedAmmo = 0;
        }
        else {
            wep.curMag = wep.magSize;
            wep.storedAmmo -= bulletsUsed; 
        }
        
        isReloading = false;
        wep.canFire = true;
    }


}
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}
public enum MeleeType
{
    Stab,
    Slash,
}


[System.Serializable]
public class Weapon
{
    public ShotHandler sHandler;
    public Inventory inv;
    public string Name;
    public Rarity rarity;
    public float bulletDespawnTimer;
    public float bulletSpeed;
    public float damageFlat;
    public float attackRate;
    public bool canFire;
    //generic shooting. checks if you can fire.
    public virtual void Shoot()
    {
    }
    /// <summary>
    /// returns an index related to weapon rarity.
    /// </summary>

    public int GetRarityColour()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return 1;
            case Rarity.Uncommon:
                return 2;
            case Rarity.Rare:
                return 3;
            case Rarity.Epic:
                return 4;
            case Rarity.Legendary:
                return 5;
            default:
                return 0;
        }
    }
    /// <summary>
    /// return a string of all relevant stats
    /// </summary>
    public virtual string GetStatString()
    {
        string str;
        str = "Name: " + Name +
              "\nDamage: " + damageFlat +
              "\nAttack Rate: " + attackRate +
              "\nBullet Despawn Timer: " + bulletDespawnTimer +
              "\nBullet Speed: " + bulletSpeed;
              

        return str;
    }

}
[System.Serializable]
public class RangedWeapon : Weapon
    {
        public int curMag;
        public int magSize;
        public int storedAmmo;
        public float reloadSpeed;
        public GameObject bulletObject;
        public Color lightColour;
        public float lightRange;
        public float lightIntensity;
        public bool isreloading;

        public override void Shoot()
        {
            if (!canFire||curMag<=0) { return; }
            if (!MagCheck()) { inv.StartCoroutine(inv.ReloadWeapon(this, reloadSpeed)); }
            sHandler.Shoot();
            curMag--;
            if (!MagCheck()) { inv.StartCoroutine(inv.ReloadWeapon(this, reloadSpeed)); }
            inv.StartCoroutine(inv.ShotTimer(this, 1 / (attackRate * (1 + inv.statsMult.attackRateMultiplier))));



        }
        /// <summary>
        /// checks if there is enough ammo currently in your magazine to fire your weapon.
        /// </summary>
        /// <returns></returns>
        public virtual bool MagCheck()
        {
            if (curMag > 0) return true;
            else { return false; }

        }
        /// <summary>
        /// returns string containing the ratio of current ammo in magazine to total stored ammo.
        /// </summary>
        /// <returns></returns>
        public string GetMagState()
        {
        return curMag + "/" + storedAmmo;
        }
        public virtual void OnNoAmmo()
        {
            Debug.Log("no ammo");
        }
        public override string GetStatString()
         {
            string str;
            str = "Name:\n" + Name +
            "\nDamage: " + damageFlat +
            "\nAttack Rate: " + attackRate +
            "\nAttack Range: " + bulletDespawnTimer +
            "\nBullet Speed: " + bulletSpeed +
            "\nMag Size: " + magSize +
            "\nReload Speed: " + reloadSpeed;


                return str;
        }
}
[System.Serializable]
public class MeleeWeapon : Weapon
    {
        public float attackRange;
        public float attackArc;
        public MeleeType meleeType;
        
        public override void Shoot()
        {

            if (!canFire) { return; }
            if(meleeType == MeleeType.Stab) { Stab(); }
            else if(meleeType == MeleeType.Slash) { Slash(attackArc, attackRange); }
            inv.StartCoroutine(inv.ShotTimer(this, 1 / (attackRate * (1 + inv.statsMult.attackRateMultiplier))));

        }
        public virtual void Stab()
        {
            sHandler.Stab();
        }

        public virtual void Slash(float angle, float radius)
        {

        
        }
    public override string GetStatString()
        {
            string str;
            str = "Name: " + Name +
            "\nDamage: " + damageFlat +
            "\nAttack Rate: " + attackRate +
            "\nBullet Despawn Timer: " + bulletDespawnTimer +
            "\nBullet Speed: " + bulletSpeed +
            "\nAttack Range: " + attackRange +
            "\nAttack Arc: " + attackArc;


            return str;
        }

}

public class BasicPistol : RangedWeapon
    {
        public BasicPistol(ShotHandler sh, Inventory inv)
        {   this.sHandler =sh;
            this.inv = inv;
            this.Name = "Basic Pistol";
            this.rarity = Rarity.Common;
            this.damageFlat = 1;
            this.attackRate = 1;
            this.bulletDespawnTimer = 5;
            this.bulletSpeed = 5;
            this.canFire = true;
            this.magSize = 7;
            this.curMag = 7;
            this.storedAmmo = 28;
            this.reloadSpeed = 1;
            this.bulletObject = null;
            this.lightColour = new Color(255, 0, 0);
            this.lightRange = 10;
            this.lightIntensity = 0.01f;
        }
        public BasicPistol(ShotHandler sh, Inventory inv,GameObject projectile)
        {   this.sHandler =sh;
            this.inv = inv;
            this.Name = "Basic Pistol";
            this.rarity = Rarity.Common;
            this.damageFlat = 0;
            this.attackRate = 1;
            this.bulletDespawnTimer = 5;
            this.bulletSpeed = 5;
            this.canFire = true;
            this.magSize = 7;
            this.curMag = 7;
            this.storedAmmo = 28;
            this.reloadSpeed = 1;
            this.bulletObject = projectile;
            this.lightColour = new Color(255, 0, 0);
            this.lightRange = 10;
            this.lightIntensity = 0.01f;
        }


        public override void OnNoAmmo()
        {
            Debug.Log("no ammo");
        }



}

public class CombatKnife : MeleeWeapon {

        public CombatKnife(ShotHandler sh, Inventory inv)
        {
            this.sHandler = sh;
            this.inv = inv;
            this.Name = "Combat Knife";
            this.rarity = Rarity.Common;
            this.damageFlat = 0;
            this.attackRate = 1;
            this.attackRange = 2;
            this.attackArc = 180;
            this.bulletDespawnTimer = 0;
            this.bulletSpeed = 0;
            this.canFire = true;
            this.meleeType = MeleeType.Stab;

        }

    } 

