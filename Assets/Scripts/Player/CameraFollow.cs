using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform player;
    public float lerpVal;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        try { transform.position = Vector3.Lerp(transform.position, player.position, lerpVal) + offset; } catch { Debug.LogWarning("Player Transform Not Found/Destroyed"); }
    }
}
