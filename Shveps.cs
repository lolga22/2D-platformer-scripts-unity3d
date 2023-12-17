using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shveps : Entity
{
    private float speed = 2f;
    private Vector3 dir;
    private SpriteRenderer sprite;

    private void Start()
    {
        dir = transform.right;
        lives = 3;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, 0.01f);

        if (hit.collider != null && hit.collider.gameObject != Hero.Instance.gameObject)
        {
            dir *= -1f;
        }

        transform.Translate(dir * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }

}