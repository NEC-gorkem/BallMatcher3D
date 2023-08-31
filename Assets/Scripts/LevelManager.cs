using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-3)]
public class LevelManager : MonoBehaviour
{

    // singleton structure
    public static LevelManager Instance { get; private set; }
    private int totalNumberOfBalls;
    private int currentLevel;


    public delegate void OnChangeTheSceneDelegate(string nameOfTheScene);
    public event OnChangeTheSceneDelegate OnChangeTheScene;

    private void Awake()
    {
        currentLevel = 1;
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

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }


    // Null ERROR BU YUZDEN GELIYOR !!!!!
    private void Start()
    {
        //SceneManager.LoadScene("Level"+ currentLevel.ToString());
    }

    public void DecrementTotalNumberOfBalls()
    {
        totalNumberOfBalls--;
        // winning condition
        if(totalNumberOfBalls == 0)
        {
            currentLevel++;
            SceneManager.LoadScene("Level" + currentLevel.ToString());
        }
    }

    // losing condition
    public void OppositeColourCollision()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    private void OnActiveSceneChanged(Scene Previous, Scene Next)
    {
        totalNumberOfBalls = GameObject.Find("Spheres").transform.childCount;
        OnChangeTheScene.Invoke(Next.name);
    }
}
