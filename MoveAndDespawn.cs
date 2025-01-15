using UnityEngine;
using Lean.Pool;

public class MoveAndDespawn : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 5f; // Speed of movement

    private float defaultY; // The default Y position
    private bool isFirstEnable = true; // To track if this is the first enable

    [SerializeField] bool shouldRotate =true;

 


   

    private void Awake()
    {
        // Store the initial Y position
        defaultY = transform.position.y;
    }

    private void OnEnable()
    {
      
       if(shouldRotate) transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float randomOffset = Random.Range(0f, -1.25f);
        transform.position = new Vector3(transform.position.x, defaultY + randomOffset, transform.position.z);
        
    }
    private void Start()
    {
        // Set rotation to -90 degrees on the X-axis
        if (shouldRotate) transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float randomOffset = Random.Range(0f, -1.25f);
        transform.position = new Vector3(transform.position.x, defaultY + randomOffset, transform.position.z);
    }

    private void Update()
    {
        // Move the GameObject to the left at a constant speed
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

  
}
