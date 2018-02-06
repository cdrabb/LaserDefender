using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public float health = 150f;
    public float projectileSpeed = 10;
    public GameObject projectile;
    public float shotsPerSecond = 0.5f;
    public int pointValue = 150;

    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    private void Start()
    {
        scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
    }
    void Update()
    {
        float probability = shotsPerSecond * Time.deltaTime;

        if (Random.value < probability)
        {
            Fire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missle = collision.gameObject.GetComponent<Projectile>();
        if (missle)
        {
            health -= missle.GetDamage();
            missle.Hit();

            if(health <= 0)
            {
                Die();
            }
        }
    }

    void Fire()
    {
        Vector3 startPosition = transform.position + new Vector3(0, 0, 0);
        GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity);
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position, 2.0f);
    }

    void Die()
    {
        scoreKeeper.Score(pointValue);
        AudioSource.PlayClipAtPoint(deathSound, transform.position, 100.0f);
        Destroy(gameObject);
    }
}
