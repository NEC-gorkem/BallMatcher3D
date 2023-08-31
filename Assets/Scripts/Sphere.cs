using System.Collections;
using UnityEngine;



public class Sphere : MonoBehaviour
{
    private TouchManager touchManager;
    private LevelManager levelManager;
    private SphereSituation sphereSituation;
    private UIManager uIManager;
    private Rigidbody rigidbodyItSelf;





    private void Awake()
    {
        rigidbodyItSelf = gameObject.GetComponent<Rigidbody>();
        sphereSituation = gameObject.GetComponent<SphereSituation>();
        touchManager = TouchManager.Instance;
        levelManager = LevelManager.Instance;
        uIManager = UIManager.Instance;   
    }

    private void OnEnable()
    {
        touchManager.OnSphereClick += OnClickItself;
        uIManager.OnChangeTheSphereSpeed += SphereSpeedSetterForMovingOnes;
    }

    private void OnDisable()
    {
        touchManager.OnSphereClick -= OnClickItself;
        uIManager.OnChangeTheSphereSpeed -= SphereSpeedSetterForMovingOnes;
    }

    private void Start()
    {
        if(sphereSituation.isSphereAlreadyMoving)
        {
            if (sphereSituation.horizontalMovement)
            {
                sphereSituation.SetCoroutineOfTheSphere(StartCoroutine(PeriodicHorizontalMovement()));
            }
            else if (sphereSituation.verticalMovement)
            {
                sphereSituation.SetCoroutineOfTheSphere(StartCoroutine(PeriodicVerticalMovement()));
            }
        }
    }

    private void OnClickItself(RaycastHit clickedObjectRaycastHit)
    {
        // you cannot make an action of the same colour object if it is already in action
        if(!sphereSituation.isSphereInAction)
        {
            // getting transform of the clicked object
            Transform clickedObjectTransform = clickedObjectRaycastHit.transform;
            // getting name of the clicked object to use in collider
            sphereSituation.SetClickedNameOfSphere(clickedObjectTransform.name);
            // first condition for selection of colours and other condition is selection of the clicked object
            // this loop iterates for the clicked sphere script
            if (tag == clickedObjectTransform.tag && name == clickedObjectTransform.name)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(clickedObjectTransform.tag))
                {
                    // all the same coloured spheres are going in action
                    obj.GetComponent<SphereSituation>().isSphereInAction = true;
                    // number of the same coloured spheres whose colour is selected
                    sphereSituation.IncrementNumberOfTheClickedColour();
                    // For all the non-clicked same coloured objects
                    if (obj.name != clickedObjectTransform.name)
                    {
                        SphereSituation otherSphereSituation = obj.GetComponent<SphereSituation>();
                        // if the other same non clicked objects are already in move by default
                        // 1-) their coroutines will be shoutdown.
                        if(otherSphereSituation.isSphereAlreadyMoving)
                        {
                            StopCoroutine(otherSphereSituation.GetCoroutineOfTheSphere());
                        }
                        // 2-) their velocity is set afterwards.
                        // making the other non-clicked same tagged spheres as not stationary
                        otherSphereSituation.isSphereStationary = false;
                        // getting transform of the non-clicked objects in every iteration
                        Transform otherSameTaggedNotClickedObjectTransforms = obj.transform;
                        // getting direction of the movement from non-clicked object to clicked one
                        Vector3 movementDirectionNormalized = Vector3.Normalize(clickedObjectTransform.position - otherSameTaggedNotClickedObjectTransforms.position);
                        // giving the necessary velocity of the non-clicked object to move towards to clicked one
                        obj.GetComponent<Rigidbody>().velocity = movementDirectionNormalized * sphereSituation.speedOfTheSphere;

                    }
                    // for the clicked object (its speed is going to be zero) if (it got already speed)
                    else if(sphereSituation.isSphereAlreadyMoving)
                    {
                        // stopping the corresponding coroutine of the sphere
                        StopCoroutine(sphereSituation.GetCoroutineOfTheSphere());
                        // making its velocity zero
                        obj.GetComponent<Rigidbody>().velocity = Vector3.zero; 
                    }
                }
            }
        }   
    }

    private void SphereSpeedSetterForMovingOnes(float speed)
    {
        
        GameObject allSpheres = GameObject.Find("Spheres");
        Transform transformOfTheParentShperes = allSpheres.transform;
        int childCount = transformOfTheParentShperes.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform ball = transformOfTheParentShperes.GetChild(i);
            SphereSituation ballsSphereSituation = ball.GetComponent<SphereSituation>();
            if (!(ballsSphereSituation.isSphereStationary) || ballsSphereSituation.isSphereAlreadyMoving)
            {
                Rigidbody tempRigidBody = ball.GetComponent<Rigidbody>();
                tempRigidBody.velocity = Vector3.Normalize(tempRigidBody.velocity) * speed;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(sphereSituation.GetClickedNameOfSphere() == name)
        {
            if (tag == other.tag && sphereSituation.GetNumberOfTheClickedColour() == 2)
            {

                Destroy(gameObject);
                Destroy(other.gameObject);

                // decrementation of total number of balls to checking winning condition
                levelManager.DecrementTotalNumberOfBalls();
                levelManager.DecrementTotalNumberOfBalls();

                // decrementation of same coloured number of balls to determine the condition of deleting the
                // clicked object
                sphereSituation.DecrementNumberOfTheClickedColour();
                sphereSituation.DecrementNumberOfTheClickedColour();

            }
            else if (tag == other.tag)
            {
                Destroy(other.gameObject);
                sphereSituation.DecrementNumberOfTheClickedColour();
                levelManager.DecrementTotalNumberOfBalls();
            }
        }
        if(tag != other.tag)
        {
            levelManager.OppositeColourCollision();
        }
    }

    private IEnumerator PeriodicHorizontalMovement()
    {
        if (sphereSituation.firstRight)
        {
            rigidbodyItSelf.velocity = new Vector3(sphereSituation.speedOfTheSphere, 0, 0);
        }
        else
            rigidbodyItSelf.velocity = new Vector3(-sphereSituation.speedOfTheSphere, 0, 0);
        yield return new WaitForSeconds(1);
        while (true)
        {
            rigidbodyItSelf.velocity = -rigidbodyItSelf.velocity;
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator PeriodicVerticalMovement()
    {
        if (sphereSituation.firstUp)
        {
            rigidbodyItSelf.velocity = new Vector3(0, 0, sphereSituation.speedOfTheSphere);
        }
        else
            rigidbodyItSelf.velocity = new Vector3(0, 0, -sphereSituation.speedOfTheSphere);
        yield return new WaitForSeconds(1);
        while (true)
        {
            rigidbodyItSelf.velocity = -rigidbodyItSelf.velocity;
            yield return new WaitForSeconds(2);
        }
    }
}
