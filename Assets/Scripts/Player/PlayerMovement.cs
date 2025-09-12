using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float firePointDistance = 0.2f;

    [Header("References")]
    public Rigidbody rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public Transform firePoint;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Hareket
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed;
        Vector3 newVel = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        rb.linearVelocity = newVel;

        // Animasyon ve yön
        if (moveX != 0 || moveZ != 0)
        {
            anim.SetBool("isRun", true);

            // Karakter yönlendirmesi (sprite flip için)
            if (moveX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveX < 0)
            {
                spriteRenderer.flipX = true;
            }

            // FirePoint yönlendirmesi - hareket yönüne göre pozisyon ve rotasyon
            Vector3 moveDirection = new Vector3(moveX, 0f, 0f).normalized;
            if (moveDirection != Vector3.zero)
            {
                // FirePoint'i hareket yönüne çevir
                firePoint.rotation = Quaternion.LookRotation(moveDirection);
                
                // FirePoint'in pozisyonunu hareket yönünde karakterin önüne koy
                firePoint.position = transform.position + moveDirection * firePointDistance;
            }
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }
}