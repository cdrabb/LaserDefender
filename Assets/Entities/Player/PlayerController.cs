using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject projectile;
    public float speed = 15f;
    public float padding = 1f;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 250f;

    public AudioClip fireSound;

    float xmin;
    float xmax;

    void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
    }

    void Fire()
    {
        Vector3 offset = new Vector3(0, 0, 0);
        GameObject beam = Instantiate(projectile, transform.position+offset, Quaternion.identity);
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missle = collision.gameObject.GetComponent<Projectile>();

        if (missle)
        {
            Debug.Log("Player hit with missle");
            health -= missle.GetDamage();
            missle.Hit();

            if (health <= 0)
            {
                Die();
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        
        if(Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        //Restrict player to gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y);
    }

    void Die()
    {
        LevelManager manager = FindObjectOfType<LevelManager>();
        manager.LoadLevel("Win");
        Destroy(gameObject);
    }
}
