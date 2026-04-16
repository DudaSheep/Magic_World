using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speedMovement = 10f;
    public float forceJump = 5f;
    private float horizontalMove;
    public Animator animator;

    // Flippar o Personagem
    private bool facingRight = true;

    // FireBall
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Captura os botoes AD <- ->
        horizontalMove = Input.GetAxisRaw("Horizontal");
        Debug.Log(horizontalMove);

        // Flipar o personagem
        if(horizontalMove > 0 && !facingRight)
        {
            Flip();
        }
        else if(horizontalMove < 0 && facingRight)
        {
            Flip();
        }

        // Checar se esta querendo PULAR
        if(Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x, forceJump);
            Debug.Log("Pulo");
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");            
            //Attack();
        }

        // Crouch (left shift ou c)
        if (Input.GetButton("Fire3") || Input.GetKey(KeyCode.C))
        {
            animator.SetBool("Crouch", true);
            horizontalMove = 0;
        }
        else
        {
            animator.SetBool("Crouch", false);
        }
    }

    void FixedUpdate()
    {
        // MOVER o player
        rb.velocity = new Vector2(horizontalMove * speedMovement, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Colidiu com o chão");
            animator.SetBool("Jump", false);
        }

    }

    // FLIPAR o sprite do personagem
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ATACAR
    public void Attack()
    {
        // fireball na posicao do cajadp
        GameObject fireBall = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // velocidade a firrball
        Rigidbody2D rbFireball = fireBall.GetComponent<Rigidbody2D>();
        float direction = facingRight ? 1f : -1f;
        rbFireball.velocity = new Vector2(direction * fireballSpeed, 0f);


        // destruir fireball 2s
        Destroy(fireBall, 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // checa se o que encostou no player tem a tag "Enemy" (enemy fireball)
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("O Mago foi atingido!");
            // Reinicia a cena atual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
