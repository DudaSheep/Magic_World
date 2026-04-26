using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float velocity = 5f;

    public AudioClip enemyDeathSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move para esquerda
        transform.Translate(Vector2.left * velocity * Time.deltaTime);

        // Se sair da tela, se destr�i para n�o pesar o jogo
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }

    }

    // Quando bater na fireball do Player ou Player
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Projectile")) // Tag fireball do player
        {
            AudioSource.PlayClipAtPoint(enemyDeathSound, transform.position);
            // Recebe os pontos pelo acerto a fireball inimiga
            FindObjectOfType<GameManager>().AddBonus(10);

            Destroy(gameObject); //enemy morre
            Destroy(collider.gameObject); //player perde sua fireball
        }
    }
}
