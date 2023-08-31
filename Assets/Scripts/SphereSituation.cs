using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SphereSituation : MonoBehaviour
{

    private UIManager uIManager;

    private String nameOfTheClickedObject;
    private int numberOfTheClickedColour;


    [HideInInspector]
    public float speedOfTheSphere;
    [HideInInspector]
    public bool isSphereInAction;
    [HideInInspector]
    public bool isSphereStationary;

    [SerializeField]
    public bool isSphereAlreadyMoving;

    [SerializeField]
    public bool verticalMovement;
    [SerializeField]
    public bool firstUp;

    [SerializeField]
    public bool horizontalMovement;
    [SerializeField]
    public bool firstRight;

    private void Awake()
    {
        uIManager = UIManager.Instance;
        isSphereInAction = false;
        isSphereStationary = true;
        speedOfTheSphere = 3f;
        numberOfTheClickedColour = 0;

    }

    private void OnEnable()
    {
        uIManager.OnChangeTheSphereSpeed += SphereSpeedAdjuster;
    }

    private void OnDisable()
    {
        uIManager.OnChangeTheSphereSpeed -= SphereSpeedAdjuster;
    }




    /// <summary>
    /// Number of the clicked colour adjustments
    /// </summary>
    public void IncrementNumberOfTheClickedColour()
    {
        numberOfTheClickedColour++;
    }
    public void DecrementNumberOfTheClickedColour()
    {
        numberOfTheClickedColour--;
    }
    public int GetNumberOfTheClickedColour()
    {
        return numberOfTheClickedColour;
    }


    /// <summary>
    /// ClickedSphereName adjustments
    /// </summary>
    public String GetClickedNameOfSphere()
    {
        return nameOfTheClickedObject;
    }
    public void SetClickedNameOfSphere(string newClickedName)
    {
        nameOfTheClickedObject = newClickedName;
    }


    /// <summary>
    /// When speed is adjusted bu using ui speed changes in sphere properties
    /// </summary>
    private void SphereSpeedAdjuster(float speed)
    {
        speedOfTheSphere = speed;
    }
    

    

    
}
