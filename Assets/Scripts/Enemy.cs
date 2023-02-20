using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;

    private Player _player;

    private Animator _anim;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool isDead = false;

    [SerializeField]
    private bool _newMovement = false;
    private bool _moveLeft, _moveRight = true;

    SpawnManager _spawnManager;

    [SerializeField]
    private bool _enemySideToSide;

    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shield;

    private bool _fireBehind;

    private Vector3 _spawnPos;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        var random = Random.Range(0, 2);
        if (random == 0)
        {
            _newMovement = true;
            _speed = 2;
        }

        if (Random.Range(0, 2) == 0)
        {
            _isShieldActive = true;
            _shield.SetActive(true);
        }
    }

    void Update()
    {
        if (_player != null && transform.position.y < _player.transform.position.y)
        {
            _fireBehind = true;
        }
        else
        {
            _fireBehind = false;
        }

        CalculateMovement();

        if (Time.time > _canFire && !isDead)
        {
            if (_fireBehind)
                _spawnPos = transform.position + new Vector3(0, 4.5f, 0);
            else
                _spawnPos = transform.position + new Vector3(0, -1, 0);
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            var enemyLaser = Instantiate(_laserPrefab, _spawnPos, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (var laser in lasers)
            {
                if (_fireBehind)
                    return;
                else
                    laser.AssignEnemyLaser();
            }

        }




    }
    void CalculateMovement()
    {


        if (_enemySideToSide == true && transform.position.x > 15)
        {
            transform.position = new Vector3(-13f, 5, 0);

        }
        if (_newMovement == true && _enemySideToSide == false)
        {
            if (transform.position.x < -9)
            {
                _moveLeft = false;
                _moveRight = true;
            }
            if (transform.position.x > 9)
            {
                _moveLeft = true;
                _moveRight = false;
            }
            if (_moveRight)
            {
                transform.Translate((Vector3.down + Vector3.right) * _speed * Time.deltaTime);
            }
            if (_moveLeft)
            {
                transform.Translate((Vector3.down + Vector3.left) * _speed * Time.deltaTime);
            }

        }
        else
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            if (other.tag == "Laser" || other.tag == "Missle")
            {
                Destroy(other.gameObject);
            }
            return;
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();
            Death();
        }
        else if (other.tag == "Laser" || other.tag == "Missle")
        {
            Destroy(other.gameObject);
            if (_player != null)
                _player.AddScore(10);

            Death();
        }
        else if (other.tag == "SuperLaser")
        {
            if (_player != null)
                _player.AddScore(10);
            Death();
        }

    }

    void Death()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        isDead = true;
        _audioSource.Play();
        _spawnManager._enemies.Remove(this.gameObject);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2);
    }
}
