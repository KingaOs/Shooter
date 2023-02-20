using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _soreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _shieldSprites;
    [SerializeField]
    private Image _shieldImg;

    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoCountText;

    [SerializeField]
    private Text _waveCount;

    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
            Debug.Log("Game Manager not found");
        _soreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }


    public void UpdateScore(int playerScore)
    {
        _soreText.text = "Score " + playerScore.ToString();
    }


    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprite[currentLives];

        if (currentLives == 0)
        {
            GameOver();
        }
    }

    public void UpdateShield(int shieldStrength)
    {
        if (shieldStrength > 0)
        {
            _shieldImg.gameObject.SetActive(true);
            _shieldImg.sprite = _shieldSprites[shieldStrength - 1];
        }
        else
            _shieldImg.gameObject.SetActive(false);
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.text = ammoCount.ToString();

    }

    private void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(Flickering());
        _gameManager.GameOver();

    }


    private IEnumerator Flickering()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    public void NextWave(int wave)
    {
        _waveCount.text = "Wave " + wave;
        StartCoroutine(WaveText());
    }
    private IEnumerator WaveText()
    {
        _waveCount.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        _waveCount.gameObject.SetActive(false);

    }

}
