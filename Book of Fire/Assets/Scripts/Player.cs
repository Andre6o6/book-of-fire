using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO heartbeat (stamina + sanity) - if high starts damagin

public class Player : MonoBehaviour {
    public SpriteRenderer[] hands;
   
    public float moveSpeed = 6;
    public float sprintMlt = 1.5f;
    public float accelerationTime = .1f;
    float speedMlt = 1;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex;

    float maxJumpVelocity;
    float minJumpVelocity;

    float velocityXSmoothing;
    bool grounded;

    Vector2 directionalInput;

    [HideInInspector]    public Health hp;
    Rigidbody2D rigid;
    Animator anim;
    CollisionChecker checker;
    HitController hitController;
    Magic fireBook;

    private void Awake()
    {
        hp = GetComponent<Health>();
        rigid = GetComponent<Rigidbody2D>();
        checker = GetComponent<CollisionChecker>();
        anim = GetComponent<Animator>();
        
        
    }

    private void Start()
    {
        CalculateGravity();
    }

    public void Move(Vector2 input /*mod (like SHIFT)*/)
    {
        //don't turn while attacking
        if (hitController!= null && hitController.attacking)
        {
            if (transform.localScale.x * input.x < 0)
                input.x = 0;

            return;
        }

        Vector2 velocity = rigid.velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, input.x * moveSpeed * speedMlt, ref velocityXSmoothing, accelerationTime);

        grounded = checker.CheckVertical(velocity.y * Time.deltaTime);

        rigid.velocity = velocity;

        if (grounded)
        {
            velocity.y = 0;
        }

        if (input.x == 0)
            anim.SetFloat("HorizontalSpeedAbs", 0);
        else
            anim.SetFloat("HorizontalSpeedAbs", Mathf.Abs(velocity.x));
        anim.SetFloat("VerticalSpeed", velocity.y);

        if (input.x < 0 && velocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 0);
        if (input.x > 0 && velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 0);
    }

    public void Hit()
    {
        Vector2 direction;
        direction.x = transform.localScale.x;

        if (hitController != null)
        {
            if (hitController.TryHit())
                anim.SetTrigger("Hit");
        }
    }

    public void Shoot()
    {
        if (fireBook != null)
        {
            if (fireBook.TryShoot())
            {
                hp.GetDamage(5);
                anim.SetTrigger("Shoot");
            }
        }
    }

    public void Heal()
    {
        if (fireBook != null)
        {
            hp.HealCharge();
            anim.SetBool("Heal", true);
            speedMlt = 0;
        }
    }
    public void HealReset()
    {
        if (fireBook != null)
        {
            hp.HealReset();
            anim.SetBool("Heal", false);
            speedMlt = 1;
        }
    }


    public void GetItem(int index)
    {
        hands[index].enabled = false;
        hands[index + 2].enabled = true;

        if (index == 0)
            hitController = GetComponent<HitController>();
        else
            fireBook = GetComponent<Magic>();

    }

    //JUMPING
    public void OnJumpInputDown()
    {
        if (grounded)
        {
            Vector2 velocity = rigid.velocity;
            velocity.y = maxJumpVelocity;
            rigid.velocity = velocity;
        }
    }

    public void OnJumpInputUp()
    {
        Vector2 velocity = rigid.velocity;

        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
            rigid.velocity = velocity;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var contact = collision.gameObject.GetComponent<ContactDamage>();

        if (contact != null)
        {
            if (hp.TryGetDamage(contact.damage))
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                Vector2 knockbackDirection;
                knockbackDirection.x = (direction.x > 0) ? 1 : -1;
                knockbackDirection.y = 0.4f;

                rigid.velocity += knockbackDirection * contact.knockback;
            }
        }
    }

    //Auxilary
    private void CalculateGravity()
    {
        if (timeToJumpApex == 0)
            return;

        Physics2D.gravity = new Vector2(0, -2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2));
        maxJumpVelocity = Mathf.Abs(Physics2D.gravity.y) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * minJumpHeight);
    }
}
