using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedIndicator;
    [SerializeField]
    private TextMeshProUGUI LevelIndicator;

    private LevelManager levelManager;
    
    public float normalSpeed;
    public float slowerSpeed;
    public float slowestSpeed;


    // singleton structure
    public static UIManager Instance { get; private set; }

    // events
    public delegate void OnChangeTheSphereSpeedDelegate(float speed);
    public event OnChangeTheSphereSpeedDelegate OnChangeTheSphereSpeed;


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

        levelManager = LevelManager.Instance;
    }

    private void OnEnable()
    {
        levelManager.OnChangeTheScene += CurrentLevelIndicator;
    }

    private void OnDisable()
    {
        levelManager.OnChangeTheScene -= CurrentLevelIndicator;
    }

    public void ChangeItToNormalSpeed()
    {
        OnChangeTheSphereSpeed?.Invoke(normalSpeed);
        speedIndicator.text = "Normal";
    }
    public void ChangeItToSlowerSpeed()
    {
        OnChangeTheSphereSpeed?.Invoke(slowerSpeed);
        speedIndicator.text = "Slower";
    }
    public void ChangeItToSlowestSpeed()
    {
        OnChangeTheSphereSpeed?.Invoke(slowestSpeed);
        speedIndicator.text = "Slowest";
    }

    private void CurrentLevelIndicator(string currentLevel)
    {
        LevelIndicator.text = currentLevel;
    }

}
