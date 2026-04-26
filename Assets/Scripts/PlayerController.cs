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
    private Vector3 targetedMousePos;


    [Header("Poder Especial")]
    public int specialPowerCost = 150; // Custo do especial
    public int numberOfProjectiles = 24; // numero de fireballs no especial

    [Header("Sons")]
    public AudioSource audioSource;
    public AudioClip fireballSound;
    public AudioClip deathSound;

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
        if (horizontalMove > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0 && facingRight)
        {
            Flip();
        }

        // PULAR
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x, forceJump);
            Debug.Log("Pulo");
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // ATIRAR fireball - ataque normal
        if (Input.GetButtonDown("Fire1"))
        {
            // Salva a posicao exata do clique
            targetedMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetedMousePos.z = 0;

            animator.SetTrigger("Attack");
        }

        // ATIRAR - ataque especial (Botão Direito ou E)
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E))
        {
            TryActivateSpecial();
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

    void TryActivateSpecial()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        // Verifica se tem score suficiente para usar o especial
        if (gm != null && gm.SpendScore(specialPowerCost))
        {
            animator.SetTrigger("Attack");
            Debug.Log("Especial Ativado!");
            SpecialAttack();
        }
        else
        {
            Debug.Log("Score insuficiente para o especial!");
        }
    }

    // ATAQUE ESPECIAL 360 graus de fireballs
    void SpecialAttack()
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        audioSource.PlayOneShot(fireballSound, 0.7f); //mesmo audio reduzido

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calcula a direção baseada no ângulo para formar um círculo
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector2 bulletDir = new Vector2(bulletDirX, bulletDirY).normalized;

            // Instancia a fireball
            GameObject bullet = Instantiate(fireballPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

            // Aplica velocidade na direção do raio do circulo
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                rbBullet.velocity = bulletDir * fireballSpeed;
            }

            Destroy(bullet, 2f);
            angle += angleStep;
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

    // ATACAR com FIREBALL
    public void Attack()
    {
        CheckMirrorFlip();

        audioSource.PlayOneShot(fireballSound);

        Vector2 direction = (targetedMousePos - firePoint.position).normalized;

        // Rotacao da fireball para "olhar"
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // instancia a fireball
        GameObject fireBall = Instantiate(fireballPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // velocidade a fireball
        Rigidbody2D rbFireball = fireBall.GetComponent<Rigidbody2D>();
        rbFireball.velocity = direction * fireballSpeed;

        // destruir fireball 2s
        Destroy(fireBall, 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // checa se o que encostou no player tem a tag "Enemy" (enemy fireball)
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("O Mago foi atingido!");
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

            FindObjectOfType<GameManager>().ShowGameOver();
            // inpede que o gamer ande morto
            gameObject.SetActive(false);
        }
    }

    // Flipar o personagem de acordo com a pos do mouse
    void CheckMirrorFlip()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Se o mouse estiver a direita e o player estiver olhando para a esquerda
        if (mousePos.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (mousePos.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

}
