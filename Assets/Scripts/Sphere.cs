using UnityEngine;



public class Sphere : MonoBehaviour
{
    private TouchManager touchManager;
    private LevelManager levelManager;
    private SphereSituation sphereSituation;
    private UIManager uIManager;




    private void Awake()
    {
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
                        // making the other non-clicked same tagged spheres as not stationary
                        obj.GetComponent<SphereSituation>().isSphereStationary = false;
                        // getting transform of the non-clicked objects in every iteration
                        Transform otherSameTaggedNotClickedObjectTransforms = obj.transform;
                        // getting direction of the movement from non-clicked object to clicked one
                        Vector3 movementDirectionNormalized = Vector3.Normalize(clickedObjectTransform.position - otherSameTaggedNotClickedObjectTransforms.position);
                        // giving the necessary velocity of the non-clicked object to move towards to clicked one
                        obj.GetComponent<Rigidbody>().velocity = movementDirectionNormalized * sphereSituation.speedOfTheSphere;

                    }
                    // for the clicked object (its speed is going to be zero)
                    else
                    {
                        // making its velocity zero
                        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        // stopping the corresponding 
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
            if (!(ball.GetComponent<SphereSituation>().isSphereStationary))
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
}
