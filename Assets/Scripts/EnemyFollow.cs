using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    public float speed = 2f;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // Busca o player pela tag 
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Move em diraçao ao player a cada frame - pathfinder
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // faz a bola "olhar" para o player
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            FindObjectOfType<GameManager>().AddBonus(20);

            Destroy(gameObject); //bola perseguidora morre
            Destroy(collider.gameObject);
        }
    }
}
