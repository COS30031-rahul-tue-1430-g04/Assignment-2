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

	[Header("Terrain")]
	[SerializeField]
	private float defaultTerrainSpeedMultiplier = 0.8f;

	[Header("Water")]
	[SerializeField]
	private float waterHeightOffset = 0.3f;

	[SerializeField]
	private SpriteMask waterMask;

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

	private void Start()
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

			// IMPORTANT:
			// Use a named method instead of a lambda.
			interact.started += OnInteract;
		}

		// Water mask should be OFF when the game starts.
		if (waterMask != null)
		{
			waterMask.enabled = false;
		}
	}

	private void OnDisable()
	{
		// Remove callbacks BEFORE the Player is disabled/destroyed.

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
			interact.started -= OnInteract;
		}
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		// Extra safety check.
		if (this == null || gameObject == null)
			return;

		TryInteract();
	}

	private void UpdateMoveInput(InputAction.CallbackContext context)
	{
		// Extra safety check.
		if (this == null || gameObject == null)
			return;

		moveInput = context.ReadValue<Vector2>();

		UpdateVelocity();

		// Update animator parameters
		if (animator != null && canMove)
		{
			animator.SetInteger(
				XDirHash,
				(int)Math.Round(moveInput.x)
			);

			animator.SetInteger(
				YDirHash,
				(int)Math.Round(moveInput.y)
			);
		}
	}

	private void UpdateSprintInput(InputAction.CallbackContext context)
	{
		if (this == null || gameObject == null)
			return;

		isSprinting = context.ReadValue<float>() > 0.5f;

		UpdateVelocity();
	}

	private void UpdateVelocity()
	{
		if (!canMove)
		{
			targetVelocity = Vector2.zero;
			return;
		}

		float currentSpeed =
			isSprinting
				? moveSpeed * sprintMultiplier
				: moveSpeed;

		currentSpeed *= terrainSpeedMultiplier;

		targetVelocity = moveInput * currentSpeed;
	}

	private void FixedUpdate()
	{
		if (rb != null && canMove)
		{
			rb.linearVelocity = targetVelocity;
		}
		else if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
		}
	}

	// =========================================================
	// WATER / TERRAIN
	// =========================================================

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent(out TerrainZone zone))
			return;

		// Prevent duplicate entries.
		if (!activeZones.Contains(zone))
		{
			activeZones.Add(zone);
		}

		if (zone.isWater)
		{
			Debug.Log(
				"Entered water zone: " +
				other.gameObject.name
			);

			if (spriteRenderer != null)
			{
				// If your SpriteMask covers the TOP half,
				// use VisibleInsideMask.
				spriteRenderer.maskInteraction =
					SpriteMaskInteraction.VisibleOutsideMask;
			}

			if (waterMask != null)
			{
				waterMask.enabled = true;
			}
		}

		Debug.Log(
			"Entered: " +
			other.gameObject.name +
			" | Speed Multiplier: " +
			zone.speedMultiplier
		);

		UpdateTerrainSpeedMultiplier();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.TryGetComponent(out TerrainZone zone))
			return;

		activeZones.Remove(zone);

		if (zone.isWater)
		{
			Debug.Log(
				"Exited water zone: " +
				other.gameObject.name
			);

			if (spriteRenderer != null)
			{
				spriteRenderer.maskInteraction =
					SpriteMaskInteraction.None;
			}

			if (waterMask != null)
			{
				waterMask.enabled = false;
			}
		}

		Debug.Log(
			"Exited: " +
			other.gameObject.name +
			" | Speed Multiplier: " +
			zone.speedMultiplier
		);

		UpdateTerrainSpeedMultiplier();
	}

	private void UpdateTerrainSpeedMultiplier()
	{
		terrainSpeedMultiplier =
			activeZones.Count > 0
				? activeZones[^1].speedMultiplier
				: defaultTerrainSpeedMultiplier;

		UpdateVelocity();
	}

	// =========================================================
	// INTERACTION
	// =========================================================

	private void TryInteract()
	{
		if (!canMove)
			return;

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
				Debug.Log(
					"ASSET FOUND: " +
					asset.assetName
				);

				if (asset.SelectAsset())
					return;
			}
		}

		Debug.Log("NO ASSET NEARBY!");
	}

	// =========================================================
	// MOVEMENT CONTROL
	// =========================================================

	public void DisableMovement()
	{
		canMove = false;

		moveInput = Vector2.zero;
		isSprinting = false;
		targetVelocity = Vector2.zero;

		if (animator != null)
		{
			animator.SetInteger(XDirHash, 0);
			animator.SetInteger(YDirHash, 0);
		}

		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
		}

		Debug.Log("PLAYER MOVEMENT DISABLED");
	}

	public void EnableMovement()
	{
		canMove = true;

		moveInput = Vector2.zero;
		isSprinting = false;
		targetVelocity = Vector2.zero;

		Debug.Log("PLAYER MOVEMENT ENABLED");
	}

	// =========================================================
	// DEBUG
	// =========================================================

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(
			transform.position,
			interactionRadius
		);
	}
}