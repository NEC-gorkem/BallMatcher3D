using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;



    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.Log(Input.touchCount);
        // if (Input.touchCount > 0)
        // {
        //     Debug.Log("hmmm");
        //     ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //     Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
        //     if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //     {
        //         Debug.Log("Hit something.");
        //         Debug.Log(hit);
        //     }
        // }
    }

    public void GetTouchInput(InputAction.CallbackContext context)
    {
        ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit something.");
            Debug.Log(hit.transform.name);
        }
    }

}
