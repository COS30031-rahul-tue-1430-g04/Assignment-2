using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// =========================================================
	// MOVEMENT
	// =========================================================

	[Header("Movement")]
	public float moveSpeed = 5f;
	public float sprintMultiplier = 1.5f;


	// =========================================================
	// INTERACTION
	// =========================================================

	[Header("Interaction")]
	public float interactionRadius = 2f;


	// =========================================================
	// MOVEMENT CONTROL
	// =========================================================

	[Header("Movement Control")]
	public bool canMove = true;


	// =========================================================
	// TERRAIN
	// =========================================================

	[Header("Terrain")]
	public float terrainSpeedMultiplier = 1f;

	public bool isSwimming = false;

	[Tooltip("Extra movement multiplier while in water.")]
	public float swimmingMultiplier = 0.7f;


	// =========================================================
	// WATER VISUAL
	// =========================================================

	[Header("Water Visual")]
	public SpriteMask waterMask;


	// =========================================================
	// ANIMATION
	// =========================================================

	[Header("Animation")]
	public Animator animator;

	[Tooltip("Animator parameter controlling horizontal direction.")]
	public string moveXParameter = "MoveX";

	[Tooltip("Animator parameter controlling vertical direction.")]
	public string moveYParameter = "MoveY";

	[Tooltip("Animator parameter telling the Animator if the player is moving.")]
	public string isMovingParameter = "IsMoving";


	// =========================================================
	// PRIVATE VARIABLES
	// =========================================================

	private TerrainZone currentTerrain;

	private Rigidbody2D rb;

	private Vector2 moveInput;

	private bool isSprinting;

	// Last direction the player was facing
	private Vector2 lastDirection = Vector2.down;


	// =========================================================
	// AWAKE
	// =========================================================

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		// Automatically find Animator if not assigned
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}

		// Water mask should be off at the beginning
		if (waterMask != null)
		{
			waterMask.enabled = false;
		}
	}


	// =========================================================
	// UPDATE
	// =========================================================

	private void Update()
	{
		if (!canMove)
		{
			moveInput = Vector2.zero;
			isSprinting = false;

			UpdateAnimation();

			return;
		}

		HandleInput();

		UpdateAnimation();
	}


	// =========================================================
	// FIXED UPDATE
	// =========================================================

	private void FixedUpdate()
	{
		if (!canMove)
		{
			if (rb != null)
			{
				rb.linearVelocity = Vector2.zero;
			}

			return;
		}

		MovePlayer();
	}


	// =========================================================
	// INPUT
	// =========================================================

	private void HandleInput()
	{
		moveInput = Vector2.zero;

		if (Keyboard.current == null)
			return;


		// -----------------------------------------------------
		// UP
		// -----------------------------------------------------

		if (Keyboard.current.wKey.isPressed ||
			Keyboard.current.upArrowKey.isPressed)
		{
			moveInput.y += 1f;
		}


		// -----------------------------------------------------
		// DOWN
		// -----------------------------------------------------

		if (Keyboard.current.sKey.isPressed ||
			Keyboard.current.downArrowKey.isPressed)
		{
			moveInput.y -= 1f;
		}


		// -----------------------------------------------------
		// LEFT
		// -----------------------------------------------------

		if (Keyboard.current.aKey.isPressed ||
			Keyboard.current.leftArrowKey.isPressed)
		{
			moveInput.x -= 1f;
		}


		// -----------------------------------------------------
		// RIGHT
		// -----------------------------------------------------

		if (Keyboard.current.dKey.isPressed ||
			Keyboard.current.rightArrowKey.isPressed)
		{
			moveInput.x += 1f;
		}


		// -----------------------------------------------------
		// SPRINT
		// -----------------------------------------------------

		isSprinting =
			Keyboard.current.leftShiftKey.isPressed;


		// -----------------------------------------------------
		// INTERACTION
		// -----------------------------------------------------

		if (Keyboard.current.eKey.wasPressedThisFrame)
		{
			TryInteract();
		}


		// Prevent diagonal movement being faster
		moveInput = moveInput.normalized;


		// Save last direction
		if (moveInput.sqrMagnitude > 0.01f)
		{
			lastDirection = moveInput;
		}
	}


	// =========================================================
	// MOVEMENT
	// =========================================================

	private void MovePlayer()
	{
		float currentSpeed = moveSpeed;


		// Apply terrain speed
		currentSpeed *= terrainSpeedMultiplier;


		// -----------------------------------------------------
		// WATER
		// -----------------------------------------------------

		if (isSwimming)
		{
			currentSpeed *= swimmingMultiplier;

			// No sprinting in water
			isSprinting = false;
		}
		else
		{
			if (isSprinting)
			{
				currentSpeed *= sprintMultiplier;
			}
		}


		// -----------------------------------------------------
		// MOVE
		// -----------------------------------------------------

		Vector2 movement =
			moveInput *
			currentSpeed *
			Time.fixedDeltaTime;

		rb.MovePosition(
			rb.position + movement
		);
	}


	// =========================================================
	// ANIMATION
	// =========================================================

	private void UpdateAnimation()
	{
		if (animator == null)
			return;

		bool moving = moveInput.sqrMagnitude > 0.01f;

		animator.SetBool("IsMoving", moving);

		if (!moving)
			return;

		if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
		{
			if (moveInput.x > 0)
				animator.SetInteger("Direction", 3); // Right
			else
				animator.SetInteger("Direction", 2); // Left
		}
		else
		{
			if (moveInput.y > 0)
				animator.SetInteger("Direction", 1); // Up
			else
				animator.SetInteger("Direction", 0); // Down
		}
	}


	// =========================================================
	// ENTER TERRAIN
	// =========================================================

	public void EnterTerrain(
		TerrainZone terrain,
		float multiplier,
		bool water,
		string terrainName
	)
	{
		currentTerrain = terrain;

		terrainSpeedMultiplier = multiplier;

		isSwimming = water;


		// -----------------------------------------------------
		// WATER MASK
		// -----------------------------------------------------

		if (waterMask != null)
		{
			waterMask.enabled = isSwimming;
		}


		Debug.Log(
			"Entered terrain: " +
			terrainName +
			" | Speed Multiplier: " +
			multiplier +
			" | Water: " +
			water
		);
	}


	// =========================================================
	// EXIT TERRAIN
	// =========================================================

	public void ExitTerrain(
		TerrainZone terrain
	)
	{
		if (currentTerrain != terrain)
			return;


		currentTerrain = null;

		terrainSpeedMultiplier = 1f;

		isSwimming = false;


		// Turn water mask off
		if (waterMask != null)
		{
			waterMask.enabled = false;
		}


		Debug.Log(
			"Left terrain. Returning to normal movement."
		);
	}


	// =========================================================
	// ASSET INTERACTION
	// =========================================================

	private void TryInteract()
	{
		if (!canMove)
			return;


		Collider2D[] objects =
			Physics2D.OverlapCircleAll(
				transform.position,
				interactionRadius
			);


		Debug.Log(
			"Checking for nearby assets..."
		);


		AssetInteraction closestAsset = null;

		float closestDistance =
			Mathf.Infinity;


		foreach (Collider2D obj in objects)
		{
			AssetInteraction asset =
				obj.GetComponent<AssetInteraction>();

			if (asset == null)
				continue;

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
			Debug.Log(
				"NO AVAILABLE ASSET NEARBY!"
			);
		}
	}


	// =========================================================
	// DISABLE MOVEMENT
	// =========================================================

	public void DisableMovement()
	{
		canMove = false;

		moveInput = Vector2.zero;

		isSprinting = false;


		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
		}


		UpdateAnimation();


		Debug.Log(
			"PLAYER MOVEMENT DISABLED"
		);
	}


	// =========================================================
	// ENABLE MOVEMENT
	// =========================================================

	public void EnableMovement()
	{
		canMove = true;

		moveInput = Vector2.zero;

		isSprinting = false;


		UpdateAnimation();


		Debug.Log(
			"PLAYER MOVEMENT ENABLED"
		);
	}


	// =========================================================
	// GIZMOS
	// =========================================================

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireSphere(
			transform.position,
			interactionRadius
		);
	}
}