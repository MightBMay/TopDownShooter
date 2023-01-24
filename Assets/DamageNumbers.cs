using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public Vector3 numScale;
    public GameObject dmgNum;
    public void CreateDamageNumber(float damage, float trueDamage)
    {
        string dmgStr = (damage ==0)?"":damage.ToString() , truedmgStr= (trueDamage==0)? "":trueDamage.ToString() ;
        GameObject text = Instantiate(dmgNum,transform);
        TextMeshProUGUI tm = text.GetComponent<TextMeshProUGUI>();
        string comma = (dmgStr == "" || truedmgStr == "") ? "" : " , ";
        string sign = (damage > 0)? "-":"+";
        Color colour = (damage > 0) ? new Color(255, 0, 0) : new Color(0, 255, 0);
        tm.color = colour;
        tm.text = sign+ dmgStr + comma + truedmgStr;

        Destroy(text, 1);
    }
}
