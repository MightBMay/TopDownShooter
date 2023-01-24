using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Stats) )]

public class ShotHandler : MonoBehaviour
{
    Camera cam;
    Stats stats;
    public Transform bulletHolder;
    Inventory inv;
    int bulletLayer;
    
  
    void Start()
    {
        cam = Camera.main;
        stats = GetComponent<Stats>();
        bulletHolder = GameObject.Find("BulletHolder").transform;
        inv = FindObjectOfType<Inventory>();
        PlayerMovement child = gameObject.GetComponentInChildren<PlayerMovement>();
        PlayerMovement parent = gameObject.GetComponentInParent<PlayerMovement>();

        if (child is null && parent is null)
        {
            bulletLayer = 0;
        }
        else { bulletLayer = 6; }
    }
    // if no target is given, use the mouse position.
    public void Shoot()
    {
            Vector3 removeHeight;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            removeHeight = hit.point-transform.position;
            removeHeight.y = 0;
            Spawnbullet(removeHeight.normalized);
        }
        

        
        
    }
    // if target is given, use the targets direction.
    public void Shoot(Transform target)
    {
        Vector3 removeHeight = target.position - transform.position;
        removeHeight.y = 0;
        
        Spawnbullet(removeHeight.normalized);

    }

    // if no specific projectile model/gameobject is given, spawn basic cube and assign it bullet properties.
    public void Spawnbullet(Vector3 dir)
    {
        GameObject bullet;
        if (inv.weapons[0] is RangedWeapon)
        {
            var temp = inv.weapons[0] as RangedWeapon;
            if (temp.bulletObject == null)
            {
                bullet = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bullet.transform.localScale = new Vector3(.125f, .25f, .25f);
            }
            else { bullet = Instantiate(temp.bulletObject, bulletHolder); }
            bullet.transform.parent = bulletHolder;
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.layer = bulletLayer;     // player layer. doesnt interact with other player layer objects.
            bullet.AddComponent(typeof(Bullet));
            Bullet b = bullet.GetComponent<Bullet>();
            b.bulletTarget = dir;
            b.stats = stats;

            
            
            bullet.AddComponent(typeof(Rigidbody));
            bullet.AddComponent(typeof(Light)); 
            bullet.GetComponent<Rigidbody>().useGravity = false;
            
            
            
        }


    }
    public void Stab()
    {
            GameObject slash;
            var temp = inv.weapons[0] as MeleeWeapon;
        //slash = new GameObject("Slash");
            slash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slash.transform.localScale = new Vector3(.5f, .5f, temp.attackRange);
            slash.transform.parent = bulletHolder;
            slash.transform.position = transform.position+ transform.forward*(temp.attackRange/2);
            slash.transform.rotation = transform.rotation;
            slash.layer = 7;     // player layer. doesnt interact with other player layer objects.
            StabDamage s = (StabDamage)slash.AddComponent(typeof(StabDamage));
            s.pStats = stats.cStats;
            
            slash.GetComponent<BoxCollider>().isTrigger = true;
           // slash.AddComponent(typeof(Light));
    }



}

