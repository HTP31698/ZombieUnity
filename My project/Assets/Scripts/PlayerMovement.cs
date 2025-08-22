using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody rb;
    private Animator animator;

    //�ʼ� vlftn
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
        // ȸ�� Rotation
        //input.Rotate * rotateSpeed * Time.deltaTime;
        var rotation = Quaternion.Euler(0f, input.Rotate * rotateSpeed * Time.deltaTime, 0f);
        
        rb.MoveRotation(rb.rotation * rotation);
        // �̵� Move
        var distance = input.Move * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + distance * transform.forward);

        animator.SetFloat(hashMove, input.Move);
        // >> ���� �ۿ� animator.SetFloat("Move", input.Move);
    }
}
