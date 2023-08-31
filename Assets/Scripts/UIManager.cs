using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedIndicator;
    
    
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

}
