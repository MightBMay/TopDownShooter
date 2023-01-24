using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotHandler))]
public class PlayerMovement : MonoBehaviour
{
    MovementStats mStats;
    Rigidbody playerRb;
    Inventory Inv;
    Camera cam;
    DroppedWeapon currentDrop;
    bool droppedWeaponStats;

    void Start()
    {
        cam = Camera.main;
        mStats = GetComponent<Stats>().mStats;
        playerRb = GetComponent<Rigidbody>();
        Inv = FindObjectOfType<Inventory>();
    }
    private void Update()
    {
        TurnPlayerTo();
        GunInput();
        


    }

    void LateUpdate()
    {

        Move();

    }
    /// <summary>
    /// Takes WASD input and converts it into player movement along the X/Z Plane.
    /// </summary>
    public void Move()
    {
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.W)) { z = 1; }
        if (Input.GetKey(KeyCode.A)) { x = -1; }
        if (Input.GetKey(KeyCode.S)) { z = -1; }
        if (Input.GetKey(KeyCode.D)) { x = 1; }
        Vector3 move = new Vector3(x, 0, z);

        if (move.magnitude < .1f) { playerRb.velocity = Vector3.zero; }
        else
        {
            move = move.normalized;
            playerRb.velocity = move * mStats.moveSpeed;
        }
    }

    
    void GunInput()
    {
        if (Input.GetMouseButton(0)) { Inv.weapons[0].Shoot(); }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if(Inv.weapons[0] is RangedWeapon wep)
            {
                
                StartCoroutine(Inv.ReloadWeapon( wep, wep.reloadSpeed));
            }
        }
        
        
    }
    /// <summary>
    /// turns player model to face where the cursor is in game.
    /// </summary>
    public void TurnPlayerTo()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 removeY;

        if (Physics.Raycast(ray, out hit))
        {
            removeY = hit.point;
            removeY.y = transform.position.y;
            transform.LookAt(removeY);
            DroppedWeaponCheck(hit);


        }
    }
    /// <summary>
    /// turns player model towards a target.
    /// </summary>
    /// <param name="target"></param>
    public void TurnPlayerTo(Transform target)
    {
        Vector3 removeY;
        removeY = target.position;
        removeY.y = transform.position.y;
        transform.LookAt(removeY);
        
    }
    /// <summary>
    /// checks if the RaycastHit "hit" collided with a dropped weapon.
    /// </summary>
    /// <param name="hit"></param>
    public void DroppedWeaponCheck(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Dropped Weapon") && !droppedWeaponStats)
        {
            currentDrop = hit.collider.GetComponent<DroppedWeapon>();
            currentDrop.OpenStats();
            droppedWeaponStats = true;
        }
        else if (!hit.collider.CompareTag("Dropped Weapon") && droppedWeaponStats)
        {
            currentDrop.CloseStats();
            droppedWeaponStats = false;
        }
    }

}
