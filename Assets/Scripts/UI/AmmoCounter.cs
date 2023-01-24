using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AmmoCounter : MonoBehaviour
{
    TextMeshProUGUI text;
    Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponentInParent<Inventory>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inv.weapons[0] is RangedWeapon) {
            RangedWeapon currentWep = (RangedWeapon)inv.weapons[0];
            text.text = currentWep.GetMagState();
        }
        else {  }
        
    }
}
