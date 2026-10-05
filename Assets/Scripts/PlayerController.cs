using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 5f;
	public float sprintMultiplier = 1.5f;

	[Header("Interaction")]
	public float interactionRadius = 2f;

	[Header("Movement Control")]
	public bool canMove = true;

	[SerializeField]
	private float defaultTerrainSpeedMultiplier = 0.8f;
	[SerializeField]
	private float waterHeightOffset = 0.3f;

	private static readonly int XDirHash = Animator.StringToHash("XDir");
	private static readonly int YDirHash = Animator.StringToHash("YDir");
	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;

	private float terrainSpeedMultiplier = 1f;
	private readonly List<TerrainZone> activeZones = new();

	private InputAction moveAction;
	private InputAction sprint;
	private InputAction interact;

	private Vector2 moveInput;
	private Vector2 targetVelocity;
	private bool isSprinting;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		TryGetComponent(out animator);
		TryGetComponent(out spriteRenderer);

		moveAction = InputSystem.actions.FindAction("Move");
		sprint = InputSystem.actions.FindAction("Sprint");
		interact = InputSystem.actions.FindAction("Interact");

		Debug.Log("Move Action: " + moveAction);
		Debug.Log("Sprint Action: " + sprint);
		Debug.Log("Interact Action: " + interact);

		if (moveAction != null)
		{
			moveAction.Enable();
			moveAction.performed += UpdateMoveInput;
			moveAction.canceled += UpdateMoveInput;
		}
		if (sprint != null)
		{
			sprint.Enable();
			sprint.performed += UpdateSprintInput;
			sprint.canceled += UpdateSprintInput;
		}
		if (interact != null)
		{
			interact.Enable();
			interact.started += ctx => TryInteract();
		}
	}
	void OnDestroy()
	{
		if (moveAction != null)
		{
			moveAction.performed -= UpdateMoveInput;
			moveAction.canceled -= UpdateMoveInput;
		}
		if (sprint != null)
		{
			sprint.performed -= UpdateSprintInput;
			sprint.canceled -= UpdateSprintInput;
		}
		if (interact != null)
		{
			interact.started -= ctx => TryInteract();
		}
	}
	void UpdateMoveInput(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
		UpdateVelocity();

		// Update animator parameters
		if (animator != null && canMove)
		{
			animator.SetInteger(XDirHash, (int)Math.Round(moveInput.x));
			animator.SetInteger(YDirHash, (int)Math.Round(moveInput.y));
		}
	}
	void UpdateSprintInput(InputAction.CallbackContext context)
	{
		isSprinting = context.ReadValue<float>() > 0.5f;
		UpdateVelocity();
	}
	void UpdateVelocity()
	{
		if (!canMove) return;

		float currentSpeed =
			isSprinting
				? moveSpeed * sprintMultiplier
				: moveSpeed;
		currentSpeed *= terrainSpeedMultiplier;
		targetVelocity = moveInput * currentSpeed;
	}

	void FixedUpdate()
	{
		if (rb != null)
			rb.linearVelocity = targetVelocity;
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out TerrainZone zone))
		{
			activeZones.Add(zone);
			if (zone.isWater)
			{
				if (spriteRenderer != null)
					spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				if (rb != null)
					//rb.MovePosition(rb.position + (Vector2.down * waterHeightOffset));
					Debug.Log("Entered water zone: " + other.gameObject.name);
			}
			Debug.Log(
				"Entered: " + other.gameObject.name +
				" | Speed Multiplier: " + zone.speedMultiplier
			);
			UpdateTerrainSpeedMultiplier();
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.TryGetComponent(out TerrainZone zone))
		{
			activeZones.Remove(zone);
			if (zone.isWater)
			{
				if (spriteRenderer != null)
					spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
				if (rb != null)
					//rb.MovePosition(rb.position + (Vector2.up * waterHeightOffset));
					Debug.Log("Exited water zone: " + other.gameObject.name);
			}
			Debug.Log(
				"Exited: " + other.gameObject.name +
				" | Speed Multiplier: " + zone.speedMultiplier
			);
			UpdateTerrainSpeedMultiplier();
		}
	}
	private void UpdateTerrainSpeedMultiplier()
	{
		terrainSpeedMultiplier = activeZones.Count > 0 ? activeZones[^1].speedMultiplier : defaultTerrainSpeedMultiplier;
		UpdateVelocity();
	}

	private void TryInteract()
	{
		if (!canMove) return;

		Debug.Log("E WAS PRESSED!");

		Collider2D[] objects =
			Physics2D.OverlapCircleAll(
				transform.position,
				interactionRadius
			);

		Debug.Log("Checking for nearby assets...");

		foreach (Collider2D obj in objects)
		{
			if (obj.TryGetComponent(out AssetInteraction asset))
			{
				Debug.Log("ASSET FOUND: " + asset.assetName);

				if (asset.SelectAsset())
					return;
			}
		}

		Debug.Log("NO ASSET NEARBY!");
	}

	// Disable player movement
	public void DisableMovement()
	{
		canMove = false;

		moveInput = Vector2.zero;
		isSprinting = false;
		targetVelocity = Vector2.zero;
		animator.SetInteger(XDirHash, 0);
		animator.SetInteger(YDirHash, 0);

		if (rb != null)
			rb.linearVelocity = Vector2.zero;

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
		Gizmos.DrawWireSphere(transform.position, interactionRadius);
	}
}