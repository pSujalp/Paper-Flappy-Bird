using UnityEngine;

public class collecting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("pipe"))
        {
            Lean.Pool.LeanPool.Despawn(collision.gameObject);

        }
    }
}
