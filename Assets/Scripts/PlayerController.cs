using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 5f;
	public float sprintMultiplier = 1.5f;

	[Header("Interaction")]
	public float interactionRadius = 2f;

	[Header("Movement Control")]
	public bool canMove = true;

	private Rigidbody2D rb;

	private Vector2 moveInput;
	private bool isSprinting;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// If movement is disabled, stop reading movement input
		if (!canMove)
		{
			moveInput = Vector2.zero;
			isSprinting = false;
			return;
		}

		HandleInput();
	}

	private void FixedUpdate()
	{
		if (!canMove)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}

		MovePlayer();
	}

	private void HandleInput()
	{
		// WASD / Arrow Keys
		moveInput = Vector2.zero;

		if (Keyboard.current != null)
		{
			if (Keyboard.current.wKey.isPressed ||
				Keyboard.current.upArrowKey.isPressed)
			{
				moveInput.y += 1f;
			}

			if (Keyboard.current.sKey.isPressed ||
				Keyboard.current.downArrowKey.isPressed)
			{
				moveInput.y -= 1f;
			}

			if (Keyboard.current.aKey.isPressed ||
				Keyboard.current.leftArrowKey.isPressed)
			{
				moveInput.x -= 1f;
			}

			if (Keyboard.current.dKey.isPressed ||
				Keyboard.current.rightArrowKey.isPressed)
			{
				moveInput.x += 1f;
			}

			// Shift = Sprint
			isSprinting =
				Keyboard.current.leftShiftKey.isPressed ||
				Keyboard.current.rightShiftKey.isPressed;

			// E = Interact
			if (Keyboard.current.eKey.wasPressedThisFrame)
			{
				TryInteract();
			}
		}

		moveInput = moveInput.normalized;
	}

	private void MovePlayer()
	{
		float currentSpeed = moveSpeed;

		if (isSprinting)
		{
			currentSpeed *= sprintMultiplier;
		}

		Vector2 movement =
			moveInput * currentSpeed * Time.fixedDeltaTime;

		rb.MovePosition(rb.position + movement);
	}

	private void TryInteract()
	{
		if (!canMove)
		{
			return;
		}

		Collider2D[] objects =
			Physics2D.OverlapCircleAll(
				transform.position,
				interactionRadius
			);

		Debug.Log("Checking for nearby assets...");

		AssetInteraction closestAsset = null;
		float closestDistance = Mathf.Infinity;

		foreach (Collider2D obj in objects)
		{
			AssetInteraction asset =
				obj.GetComponent<AssetInteraction>();

			if (asset == null)
				continue;

			// Ignore assets already selected
			if (asset.alreadySelected)
				continue;

			float distance =
				Vector2.Distance(
					transform.position,
					asset.transform.position
				);

			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestAsset = asset;
			}
		}

		if (closestAsset != null)
		{
			Debug.Log(
				"CLOSEST ASSET: " +
				closestAsset.assetName
			);

			closestAsset.SelectAsset();
		}
		else
		{
			Debug.Log("NO AVAILABLE ASSET NEARBY!");
		}
	}

	// Disable player movement
	public void DisableMovement()
	{
		canMove = false;

		moveInput = Vector2.zero;
		isSprinting = false;

		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
		}

		Debug.Log("PLAYER MOVEMENT DISABLED");
	}

	// Enable player movement
	public void EnableMovement()
	{
		canMove = true;

		moveInput = Vector2.zero;
		isSprinting = false;

		Debug.Log("PLAYER MOVEMENT ENABLED");
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireSphere(
			transform.position,
			interactionRadius
		);
	}
}