using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{

    // singleton structure
    public static LevelManager Instance { get; private set; }
    private int totalNumberOfBalls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalNumberOfBalls = GameObject.Find("Spheres").transform.childCount;
    }

    public void DecrementTotalNumberOfBalls()
    {
        totalNumberOfBalls--;
        Debug.Log(totalNumberOfBalls);
        // winning condition
        if(totalNumberOfBalls == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // losing condition
    public void OppositeColourCollision()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
