using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5f;
	[SerializeField]
	private float sprintMultiplier = 2f;

	private Rigidbody2D rb;
	private InputAction moveAction;
	private InputAction sprint;
	private InputAction interact;

	private Vector2 moveInput;
	private bool isSprinting;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		moveAction = InputSystem.actions.FindAction("Move");
		sprint = InputSystem.actions.FindAction("Sprint");
		interact = InputSystem.actions.FindAction("Interact");

		interact.performed += Interact;
	}

	void Update()
	{
		moveInput = moveAction.ReadValue<Vector2>();
		isSprinting = sprint.ReadValue<float>() > 0.5f;
	}

	void FixedUpdate()
	{
		float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
		rb.linearVelocity = moveInput * currentSpeed;
	}

	void Interact(CallbackContext context)
	{
		// Added this incase we want to use it in the future
		// Interact was one of the default actions in the input system
	}
}
