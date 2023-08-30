using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class Sphere : MonoBehaviour
{
    private TouchManager touchManager;

    [HideInInspector]
    public bool isSphereInAction;
    
    private String nameOfTheClickedObject;
    private int numberOfTheClickedColour;

    private void Awake()
    {
        touchManager = TouchManager.Instance;
        numberOfTheClickedColour = 0;
        isSphereInAction = false;
    }

    private void OnEnable()
    {
        touchManager.OnSphereClick += OnClickItself;

    }

    private void OnDisable()
    {
        touchManager.OnSphereClick -= OnClickItself;
    }

    private void OnClickItself(RaycastHit clickedObjectRaycastHit)
    {
        // you cannot make an action of the same colour object if it is already in action
        if(!isSphereInAction)
        {
            // getting transform of the clicked object
            Transform clickedObjectTransform = clickedObjectRaycastHit.transform;
            // getting name of the clicked object to use in collider
            nameOfTheClickedObject = clickedObjectTransform.name;
            // first condition for selection of colours and other condition is selection of the clicked object
            if (tag == clickedObjectTransform.tag && name == clickedObjectTransform.name)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(clickedObjectTransform.tag))
                {
                    obj.GetComponent<Sphere>().isSphereInAction = true;
                    numberOfTheClickedColour++;
                    if (obj.name != clickedObjectTransform.name)
                    {
                        // getting transform of the non-clicked objects in every iteration
                        Transform otherSameTaggedNotClickedObjectTransforms = obj.transform;
                        // getting direction of the movement from non-clicked object to clicked one
                        Vector3 movementDirectionNormalized = Vector3.Normalize(clickedObjectTransform.position - otherSameTaggedNotClickedObjectTransforms.position);
                        // giving the necessary velocity of the non-clicked object to move towards to clicked one
                        obj.GetComponent<Rigidbody>().velocity = movementDirectionNormalized * 5;

                    }
                }
            }
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(nameOfTheClickedObject == name)
        {
            if (tag == other.tag && numberOfTheClickedColour == 2)
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
                numberOfTheClickedColour = 0;
            }
            else if (tag == other.tag)
            {
                Destroy(other.gameObject);
                numberOfTheClickedColour--;
            }
        }
        if(tag != other.tag)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
