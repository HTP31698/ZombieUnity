using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody rb;
    private Animator animator;

    //필수 vlftn
    public static readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 회전 Rotation
        //input.Rotate * rotateSpeed * Time.deltaTime;
        var rotation = Quaternion.Euler(0f, input.Rotate * rotateSpeed * Time.deltaTime, 0f);
        
        rb.MoveRotation(rb.rotation * rotation);
        // 이동 Move
        var distance = input.Move * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + distance * transform.forward);

        animator.SetFloat(hashMove, input.Move);
        // >> 같은 작용 animator.SetFloat("Move", input.Move);
    }
}
