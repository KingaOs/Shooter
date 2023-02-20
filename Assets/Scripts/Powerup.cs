using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerUpSoundEffect;

    private bool _isMagnetActive;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.Find("Player");

        Player.OnMagnetActive += AciveMagnet;
    }

    // Update is called once per frame
    void Update()
    {

        if (_isMagnetActive)
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 5 * Time.deltaTime);
        else
            transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5.8f)
        {

            Destroy(this.gameObject);
        }
    }

    public void AciveMagnet()
    {
        _isMagnetActive = true;
        StartCoroutine(MagnetCoolDown());
    }

    IEnumerator MagnetCoolDown()
    {
        yield return new WaitForSeconds(5);
        _isMagnetActive = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player;
            player = collision.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUpSoundEffect, transform.position);
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBarChargeUp();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoRefill();
                        break;
                    case 4:
                        player.Health();
                        break;
                    case 5:
                        player.SuperLaserActive();
                        break;
                    case 6:
                        player.CantShoot();
                        break;                    
                    case 7:
                        player.HomingProjectile();
                        break;
                    default:
                        Debug.Log("Powerup unkown");
                        break;


                }

            }

            Destroy(this.gameObject);
        }
    }
}
