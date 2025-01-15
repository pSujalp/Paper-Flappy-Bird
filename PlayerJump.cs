using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]  float jumpForce = 5f; // The force applied for the jump
    private Rigidbody rb;

    [SerializeField] GameManager gameManager;

    [SerializeField] Animator animator;


    bool isVisible;

    [SerializeField] GameObject DeathSound;

    [SerializeField] GameObject FlapSound;

    private void OnBecameVisible()
    {
        isVisible = true;
        
    }

    private void OnBecameInvisible()
    {
        isVisible = false; 
    }
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Ensure the Rigidbody exists
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the player.");
        }
    }

    void FixedUpdate()
    {
        // Check for a touch or mouse click
        if (Input.GetMouseButtonDown(0) && isVisible==true ) // 0 = Left mouse button or touch
        {
            Jump();
        }
        animator.SetFloat("gravity", rb.linearVelocity.y);
    }
    public void GO()
    {
        jumpForce = 0;
        GameObject obj = Lean.Pool.LeanPool.Spawn(DeathSound, transform.position, Quaternion.identity);
        Lean.Pool.LeanPool.Despawn(obj, 1.25f);
    }
    bool shouldFlapSound = true;
    private void FlappySound()
    {
        if(shouldFlapSound)
        {
            GameObject obj = Lean.Pool.LeanPool.Spawn(FlapSound, transform.position, Quaternion.identity);
            Lean.Pool.LeanPool.Despawn(obj, 0.75f);
            shouldFlapSound=false;
            Invoke(nameof(SoundON), 1f);
        }

    }
    void SoundON()
    {
        shouldFlapSound=true;
    }
    void Jump()
    {
        if (rb != null)
        {
            // Apply an upward force to the Rigidbody
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            FlappySound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("pipe"))
        {
            gameManager.GameOver();
           
        }
    }
}
