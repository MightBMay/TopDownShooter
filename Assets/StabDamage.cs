using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabDamage : MonoBehaviour
{

    Coroutine dest;
    public CombatStats pStats;
    // Start is called before the first frame update
    void Start()
    {

        dest = StartCoroutine(DestroySlash(.1f));
        
    }



    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy")) { collider.GetComponent<EnemyStats>().TakeDamage(pStats.attack,0); }
    }
    IEnumerator DestroySlash(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
