using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Stats stats;
    public Rigidbody rb;
    public Light bLight;
    public Inventory inv;
    public Vector3 bulletTarget;
    Coroutine destroy;


    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bLight = GetComponent<Light>();
        inv = FindObjectOfType<Inventory>();
        gameObject.AddComponent<BoxCollider>();

        PullStats();
        destroy = StartCoroutine(Despawn(inv.weapons[0].bulletDespawnTimer));
        // stops rotation.
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    // moves bullet towards its target direction assigned when it was spawned.
    private void FixedUpdate()
    {
          rb.velocity = bulletTarget * inv.weapons[0].bulletSpeed*(1+stats.statMultipliers.bulletSpeedMultiplier); 
    }
    
    public void PullStats()
    {
        var stat = inv.weapons[0] as RangedWeapon;
        bLight.range = stat.lightRange;
        bLight.intensity = stat.lightIntensity;
        bLight.color = stat.lightColour;
        bLight.renderMode = LightRenderMode.ForcePixel;

        Material mat = GetComponent<Renderer>().material;
        mat.color = stat.lightColour;
        mat.SetColor("_EmissionColor", stat.lightColour);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(destroy!= null)
        {
            StopCoroutine(destroy);
        }
        Destroy(gameObject);
    }
    

    // takes a timer to despawn bullets so they dont last forever.
    IEnumerator Despawn(float delay)
    {
      yield return new WaitForSeconds(delay);
      if(destroy!= null)
      {
            Destroy(gameObject);
      }
        

    }
}
