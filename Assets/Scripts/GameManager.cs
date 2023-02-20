using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool _isGameOver;
  

    void Start()
    {


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        
    }

    public void GameOver()
    {
        _isGameOver = true;

    }

}
