using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Sphere : MonoBehaviour
{
    private TouchManager touchManager;
    private UIManager uIManager;
    private LevelManager levelManager;

    [HideInInspector]
    public bool isSphereInAction;
    [HideInInspector]
    public bool isSpehereStationary;
    
    private String nameOfTheClickedObject;
    private int numberOfTheClickedColour;

    private float speedOfTheSphere;

    private void Awake()
    {
        uIManager = UIManager.Instance;
        touchManager = TouchManager.Instance;
        levelManager = LevelManager.Instance;

        numberOfTheClickedColour = 0;
        speedOfTheSphere = 5f;
        isSphereInAction = false;
        isSpehereStationary = true;
    }

    private void OnEnable()
    {
        touchManager.OnSphereClick += OnClickItself;
        uIManager.OnChangeTheSphereSpeed += SphereSpeedAdjuster;
    }

    private void OnDisable()
    {
        touchManager.OnSphereClick -= OnClickItself;
        uIManager.OnChangeTheSphereSpeed -= SphereSpeedAdjuster;
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
                        // making the other non-clicked same tagged spheres as not stationary
                        obj.GetComponent<Sphere>().isSpehereStationary = false;
                        // getting transform of the non-clicked objects in every iteration
                        Transform otherSameTaggedNotClickedObjectTransforms = obj.transform;
                        // getting direction of the movement from non-clicked object to clicked one
                        Vector3 movementDirectionNormalized = Vector3.Normalize(clickedObjectTransform.position - otherSameTaggedNotClickedObjectTransforms.position);
                        // giving the necessary velocity of the non-clicked object to move towards to clicked one
                        obj.GetComponent<Rigidbody>().velocity = movementDirectionNormalized * speedOfTheSphere;

                    }
                }
            }
        }   
    }

    private void SphereSpeedAdjuster(float speed)
    {
        speedOfTheSphere = speed;
        GameObject allSpheres = GameObject.Find("Spheres");
        Transform transformOfTheParentShperes = allSpheres.transform;
        int childCount = transformOfTheParentShperes.childCount;
        
        for (int i = 0; i < childCount; i++)
        {
            Transform ball = transformOfTheParentShperes.GetChild(i);
            if (!(ball.GetComponent<Sphere>().isSpehereStationary))
            {
                Rigidbody tempRigidBody = ball.GetComponent<Rigidbody>();
                tempRigidBody.velocity = Vector3.Normalize(tempRigidBody.velocity) * speed;
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

                levelManager.DecrementTotalNumberOfBalls();
                levelManager.DecrementTotalNumberOfBalls();
                numberOfTheClickedColour = 0;
            }
            else if (tag == other.tag)
            {
                Destroy(other.gameObject);
                numberOfTheClickedColour--;
                levelManager.DecrementTotalNumberOfBalls();
            }
        }
        ///////////////////////////////
        // iki kez cagriliyor
        // losing condition
        if(tag != other.tag)
        {
            levelManager.OppositeColourCollision();
        }
        ///////////////////////////////
    }
}
