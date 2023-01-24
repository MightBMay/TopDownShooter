using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class DroppedWeapon : MonoBehaviour
{
    ShotHandler sh;
    Inventory inv;
    Renderer rend;
    TextMeshProUGUI dropText;
    Canvas droppedWeaponStats;
    bool StatsOpened;
    public bool setDrop;
    public string dropName;
    public List<Material> RarityMats;

    Dictionary<string, System.Type> allWeapons = new Dictionary<string, System.Type>()
    {
        {"Basic Pistol", typeof(Pistol) },
        {"Combat Knife", typeof(CombatKnife) },
        {"Basic Shotgun", typeof(Shotgun) }
    };
    [SerializeField]Weapon droppedWeapon;


    System.Random rand = new System.Random();



    //randomWeapon = GetRandomWeapon();
   
    private void Awake()
    {
        inv = FindObjectOfType<Inventory>();
        sh = inv.shotHandler;
        droppedWeapon = GetRandomWeapon();
        rend = GetComponent<Renderer>();
        droppedWeaponStats = GetComponentInChildren<Canvas>(true);
        dropText = GetComponentInChildren<TextMeshProUGUI>(true);
        dropText.text = droppedWeapon.GetStatString();

        try
        {
            rend.material = RarityMats[droppedWeapon.GetRarityColour()];
            gameObject.name = droppedWeapon.Name;
        }
        catch { }


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenStats();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        AddToInv(droppedWeapon);
    }
    public void AddToInv(Weapon weapon)
    {
        inv.AddWeapon(weapon);
    }

    public void OpenStats()
    {
        droppedWeaponStats.enabled = true;
    }
    public void CloseStats()
    {
        droppedWeaponStats.enabled = false;
    }


    Weapon SetWeapon(string dName)
    {

        try { Type classType = allWeapons[dName];
            return (Weapon)Activator.CreateInstance(classType, sh, inv);
        }
        catch { Debug.LogError("DroppedWeapon.SetWeapon: Weapon with name :\" " + dName + " \" could not be found."); return null; }

    }
    Weapon GetRandomWeapon()
    {
        if (setDrop) { return SetWeapon(dropName); }
        var keys = allWeapons.Keys.ToList();
        var randomKey = keys[rand.Next(keys.Count)];
        return (Weapon)Activator.CreateInstance(allWeapons[randomKey],sh,inv );

    }
    

}
