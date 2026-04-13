using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float velocity = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move para esquerda
        transform.Translate(Vector2.left * velocity * Time.deltaTime);

        // Se sair da tela, se destrói para não pesar o jogo
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
            Destroy(gameObject); //enemy morre
            Destroy(collider.gameObject); //player perde sua fireball
        }
    }
}
