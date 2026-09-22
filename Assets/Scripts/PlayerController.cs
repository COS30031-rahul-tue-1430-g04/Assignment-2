using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5f;
	[SerializeField]
	private float sprintMultiplier = 2f;

	private static readonly int XDirHash = Animator.StringToHash("XDir");
	private static readonly int YDirHash = Animator.StringToHash("YDir");
	private Animator animator;
	private Rigidbody2D rb;
	private InputAction moveAction;
	private InputAction sprint;

	private Vector2 moveInput;
	private bool isSprinting;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		TryGetComponent(out animator);

		moveAction = InputSystem.actions.FindAction("Move");
		sprint = InputSystem.actions.FindAction("Sprint");
	}

	void Update()
	{
		moveInput = moveAction.ReadValue<Vector2>();
		isSprinting = sprint.ReadValue<float>() > 0.5f;
		if (animator != null)
		{
			animator.SetInteger(XDirHash, (int)Math.Round(moveInput.x));
			animator.SetInteger(YDirHash, (int)Math.Round(moveInput.y));
		}
	}

	void FixedUpdate()
	{
		float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
		rb.linearVelocity = moveInput * currentSpeed;
	}
}
