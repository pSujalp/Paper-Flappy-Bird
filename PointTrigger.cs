using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    [SerializeField] private Counter counter;

    private void Awake()
    {
        if (counter == null)
        {
            counter = FindObjectOfType<Counter>();
            counter= FindFirstObjectByType<Counter>();
            counter = FindAnyObjectByType<Counter>();
        }
    }
    void OnEnable()
    {
        if (counter == null)
        {
            counter = FindAnyObjectByType<Counter>();
        }
    }

    private void Start()
    {
        if (counter == null)
        {
            counter = FindFirstObjectByType<Counter>();
            counter = FindAnyObjectByType<Counter>();
            Debug.LogError("Counter is not assigned and could not be found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (counter == null)
            {
                counter = FindAnyObjectByType<Counter>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (counter != null)
            {
                counter.IncreaseCount();
            }
            else
            {
                Debug.LogWarning("Counter is null. Cannot increase the count.");
            }
        }
       
    }
}
