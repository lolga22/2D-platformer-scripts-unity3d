using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int health; // жизни
    [SerializeField] private float jumpForce = 15f; // сила прыжка

    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public bool isAttaking = false;
    public bool isRecharged = true;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Hero Instance {  get; set; }

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f && !isAttaking) State = States.idle;
       // else State = States.jump;

        if (!isAttaking && Input.GetButton("Horizontal"))
            Run();

        if (!isAttaking && Mathf.Abs(rb.velocity.y) < 0.005f && Input.GetButtonDown("Jump"))
        {
            State = States.jump;
            Jump();
        }

        if(Input.GetButtonDown("Fire1")) Attack();

        if (Mathf.Abs(rb.velocity.y) > 0.005f && Input.GetButtonDown("Fire1")) {
            State = States.attack;
            Attack();
        }

        if (health > lives) health = lives;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = aliveHeart;
            else hearts[i].sprite = deadHeart; 

            if(i < lives) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }

    }

    private void Run()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f) State = States.run;


        Vector3 dir = transform.right * Input.GetAxis("Horizontal");


        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }




    private void Attack()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f && isRecharged)
        {
            State = States.attack;
            isAttaking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator AttackAnimation()
    {   
        yield return new WaitForSeconds(0.5f);
        isAttaking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.6f);
        isRecharged = true;
    }



    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    public override void GetDamage()
    {
        health -= 1;
        Debug.Log("колличество вашего хп:" + health);
        if (health == 0) 
        {
            foreach (var h in hearts) h.sprite = deadHeart;
            Die(); 
        }
    }

}

public enum States
{
    idle,
    run,
    jump,
    attack
}
