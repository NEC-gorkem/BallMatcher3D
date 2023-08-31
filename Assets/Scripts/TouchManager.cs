using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-3)]
public class TouchManager : MonoBehaviour
{
    // singleton structure
    public static TouchManager Instance { get; private set; }

    // events
    public delegate void onSphereClickDelegate(RaycastHit clickedObjectRaycastHit);
    public event onSphereClickDelegate OnSphereClick;

    private Ray ray;
    private RaycastHit hit;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void GetTouchInput(InputAction.CallbackContext context)
    {
        ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.name.Contains("Sphere"))
            {
                Debug.Log("event");
                OnSphereClick?.Invoke(hit);
            }
        }
    }

}
