using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShot;
    private bool _isSpeedBoost;
    [SerializeField]
    private GameObject _tripleShot;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private bool _isShieldActive;
    [SerializeField]
    private GameObject _shield;

    private int _shieldCount;

    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundEffect;

    [SerializeField]
    private AudioSource _audioSource;


    private int _ammoCount = 15;
    [SerializeField]
    private AudioClip _noAmmo;

    [SerializeField]
    private GameObject _superLaser;

    [SerializeField]
    private Image _speedBar;

    [SerializeField]
    private Animator _CameraAnim;

    [SerializeField]
    private bool _canShoot = true;

    public delegate void Magnet();
    public static event Magnet OnMagnetActive;

    private bool _homingProjectileActive;

    [SerializeField]
    private GameObject _missle;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager.UpdateAmmoCount(_ammoCount);



        if (_uiManager == null)
        {

            Debug.Log("The UI Manager is NULL.");
        }


        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is NULL");
        if (_audioSource == null)
            Debug.Log("Audio Source not found!");
        else
            _audioSource.clip = _laserSoundEffect;

    }



    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _canShoot)
        {
            _canFire = Time.time + _fireRate;
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_speedBar.fillAmount > 0)
            {
                _speed = 10;
                _speedBar.fillAmount -= 0.005f;
                SpeedBarChangeColor();
            }
            else
                _speed = 5;

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 5;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (OnMagnetActive != null)
                OnMagnetActive();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * Time.deltaTime * _speed);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
    }

    void FireLaser()
    {

        if (_ammoCount <= 0)
        {
            _audioSource.clip = _noAmmo;
            _audioSource.Play();
            return;
        }

        _canFire = Time.time + _fireRate;

        if (_isTripleShot)
        {
            Instantiate(_tripleShot, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }
        else if(_homingProjectileActive)
        {
            _homingProjectileActive = false;
            Instantiate(_missle, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        }
        else 
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        }
        _audioSource.clip = _laserSoundEffect;
        _audioSource.Play();

        _ammoCount -= 1;
        _uiManager.UpdateAmmoCount(_ammoCount);

    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldCount -= 1;
            _uiManager.UpdateShield(_shieldCount);
            if (_shieldCount == 0)
            {
                _isShieldActive = false;
                _shield.SetActive(false);

            }
            return;
        }
        _lives -= 1;

        _CameraAnim.SetTrigger("Shake");

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPLayerDeath();
            Destroy(this.gameObject);
        }

        if (_lives == 2)
            _leftEngine.SetActive(true);
        else if (_lives == 1)
            _rightEngine.SetActive(true);
    }


    public void TripleShotActive()
    {
        _isTripleShot = true;
        StartCoroutine(CoolDownPowerup());
    }

    IEnumerator CoolDownPowerup()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShot = false;

    }

    //public void SpeedBoostActive()
    //{
    //    _speed *= 2;
    //    StartCoroutine(SpeedBoostCoolDownRoutine());
    //}

    //IEnumerator SpeedBoostCoolDownRoutine()
    //{
    //    yield return new WaitForSeconds(5.0f);
    //    _speed = 5f;
    //}

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        _shieldCount = 3;
        _uiManager.UpdateShield(_shieldCount);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoRefill()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    public void Health()
    {
        _lives++;
        if (_lives > 3)
            _lives = 3;
        _uiManager.UpdateLives(_lives);

        if (_lives == 3)
            _leftEngine.SetActive(false);
        else if (_lives == 2)
            _rightEngine.SetActive(false);

    }
    IEnumerator CoolDownSuperLaser()
    {
        yield return new WaitForSeconds(5f);
        _superLaser.SetActive(false);

    }
    public void SuperLaserActive()
    {
        _superLaser.SetActive(true);
        StartCoroutine(CoolDownSuperLaser());
    }

    public void SpeedBarChargeUp()
    {
        _speedBar.fillAmount += 0.20f;
        SpeedBarChangeColor();

    }
    private void SpeedBarChangeColor()
    {
        if (_speedBar.fillAmount < 0.3f)
            _speedBar.color = Color.red;
        else if (_speedBar.fillAmount < 0.6f)
            _speedBar.color = Color.yellow;
        else
            _speedBar.color = Color.green;
    }

    public void CantShoot()
    {
        _canShoot = false;
        StartCoroutine(CoolDownCantShoot());
    }

    IEnumerator CoolDownCantShoot()
    {
        yield return new WaitForSeconds(5f);
        _canShoot = true;
    }

    public void HomingProjectile()
    {
        _homingProjectileActive = true;

    }

}
