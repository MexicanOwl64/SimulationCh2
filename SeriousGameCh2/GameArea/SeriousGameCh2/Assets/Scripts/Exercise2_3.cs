using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise2_3 : MonoBehaviour
{
    // Geometry defined in the inspector.
    public float floorY;
    public float leftWallX;
    public float rightWallX;
    public float topY;
    public Transform moverSpawnTransform;

    private List<Mover2_2> Movers = new List<Mover2_2>();
    // Define constant forces in our environment
    public float perlinNoiseX = 0f;
    public float perlinNoiseY = 0f;
    public float perlinNoiseZ = 0f;
    public float multiplier = 0f;
    private Vector3 wind = new Vector3(.0f, 0f, 0f);
    private Vector3 gravity = new Vector3(.0f, 0.4f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // Create copys of our mover and add them to our list
        

            Movers.Add(new Mover2_2(
                        moverSpawnTransform.position,
                        leftWallX,
                        rightWallX,
                        floorY,
                        topY
                    ));
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foreach (Mover2_2 mover in Movers)
        {
            
            perlinNoiseX = Mathf.PerlinNoise(wind.x, wind.y);
            perlinNoiseY = Mathf.PerlinNoise(wind.y, wind.z);
            perlinNoiseZ = Mathf.PerlinNoise(wind.z, wind.x);

            wind = new Vector3(perlinNoiseX, perlinNoiseY, 0f);

            mover.body.AddForce(wind, ForceMode.Impulse);
            
            mover.body.AddForce(gravity, ForceMode.Force);

            mover.CheckBoundaries();
            Debug.Log(wind.x + " " + wind.y + " " + wind.z);
            
        }
    }
}

public class Mover2_2
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Mover2_2(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that come with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        //using our own version of gravity
        body.useGravity = false;

        // Generate random properties for this mover
        // radius = Random.Range(0.1f, 0.4f);
        radius = 1f;
        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
            Debug.Log("Hitting floor");
        }
        if (body.position.y - radius > yMax)
        {   
            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);
            Debug.Log("Hitting top");
        }
        if (body.position.x - radius < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
            Debug.Log("Hitting Left Wall");

        }

        else if (body.position.x + radius > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
            Debug.Log("Hitting Right Wall");

        }
        body.velocity = restrainedVelocity;
    }
}
