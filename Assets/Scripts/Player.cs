using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;

    private PlayerInputActions playerInputActions;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    private Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementVector();

        inputVector = inputVector.normalized;

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }
}
