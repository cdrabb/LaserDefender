using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelay = 0.5f;

    private bool movingRight = false;
    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start () {

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x;
        xmax = rightmost.x;

        SpawnUntilFull();
	}

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {

        if (!movingRight)
            transform.position += Vector3.left * speed * Time.deltaTime;
        else
            transform.position += Vector3.right * speed * Time.deltaTime;

        float rightEdgeOfFormation = transform.position.x + width / 2;
        float leftEdgeOfFormation = transform.position.x - width / 2;

        if (leftEdgeOfFormation < xmin)
        {
            movingRight = true;
        }
        else if(rightEdgeOfFormation > xmax)
        {
            movingRight = false;
        }

        if(AllMembersDead())
        {
            Debug.Log("Empty Formation");
            SpawnUntilFull();
        }
	}

    bool AllMembersDead()
    {
        foreach(Transform childPositonGameObject in transform)
        {
            if (childPositonGameObject.childCount > 0)
                return false;
        }

        return true;
    }

    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity);
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();

        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity);
            enemy.transform.parent = freePosition;
        }

        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    Transform NextFreePosition()
    {
        foreach(Transform childPosition in transform)
        {
            if (childPosition.childCount <= 0)
                return childPosition;
        }

        return null;
    }
}
