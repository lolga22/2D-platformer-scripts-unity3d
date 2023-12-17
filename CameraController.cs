using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 pos;

    private void Awake()
    {
        if(!player)
        {
            player = FindAnyObjectByType<Hero>().transform;
        }
    }

    private void Update()
    {
        pos = player.position;
        pos.y = player.position.y+2.3f;
        pos.z = -10f;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
