using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private bool _isEnemyLaser = false;
    private bool _isMissle;

    private SpawnManager _spawnManager;


    private void Start()
    {
        
        if (gameObject.tag == "Missle")
        {
            _isMissle = true;
            _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
          
        }
    }

    void Update()
    {
        if (_isEnemyLaser == false)
            MoveUp();
        else
            MoveDown();
    }

    void MoveUp()
    {
        if (_isMissle)
        {
            if (_spawnManager._enemies != null)
            {
                float distance = 100;
                GameObject _closestEnemy = _spawnManager._enemies[0];
                foreach (var enemy in _spawnManager._enemies)
                {
                    var dist = Vector3.Distance(transform.position, enemy.transform.position);

                    if (dist < distance)
                    {
                        _closestEnemy = enemy;
                        distance = dist;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, _closestEnemy.transform.position, 7 * Time.deltaTime);
               
            }
            else
                _isMissle = false;
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 8)
            {
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            }
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }


    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();
        }
    }
}
