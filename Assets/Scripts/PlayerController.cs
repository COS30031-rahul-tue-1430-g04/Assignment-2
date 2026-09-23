using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5f;

	[SerializeField]
	private float sprintMultiplier = 2f;

	[SerializeField]
	private float interactionRadius = 1.5f;

	private Rigidbody2D rb;

    private float terrainSpeedMultiplier = 1f;

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

		Debug.Log("Move Action: " + moveAction);
		Debug.Log("Sprint Action: " + sprint);
		Debug.Log("Interact Action: " + interact);

		if (moveAction != null)
			moveAction.Enable();

		if (sprint != null)
			sprint.Enable();

		if (interact != null)
			interact.Enable();
	}

	void Update()
	{
		// Movement
		if (moveAction != null)
		{
			moveInput = moveAction.ReadValue<Vector2>();
		}

		// Sprint
		if (sprint != null)
		{
			isSprinting = sprint.ReadValue<float>() > 0.5f;
		}

		// Interaction
		if (interact != null && interact.WasPressedThisFrame())
		{
			Debug.Log("E WAS PRESSED!");

			TryInteract();
		}
	}

	void FixedUpdate()
	{
		if (rb == null)
			return;

		float currentSpeed =
			isSprinting
				? moveSpeed * sprintMultiplier
				: moveSpeed;

        currentSpeed *= terrainSpeedMultiplier; //depending on which terrain the player is

        rb.linearVelocity = moveInput * currentSpeed;
	}

	private void TryInteract()
	{
		Collider2D[] objects =
			Physics2D.OverlapCircleAll(
				transform.position,
				interactionRadius
			);

		Debug.Log("Checking for nearby assets...");

		foreach (Collider2D obj in objects)
		{
			AssetInteraction asset =
				obj.GetComponent<AssetInteraction>();

			if (asset != null)
			{
				Debug.Log("ASSET FOUND: " + asset.assetName);

				asset.SelectAsset();
				return;
			}
		}

		Debug.Log("NO ASSET NEARBY!");
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(
			transform.position,
			interactionRadius
		);
	}

	//trigger based suits well for our scenario
	//own colliders to tell player on what terrain he currently is
    private void OnTriggerEnter2D(Collider2D other)
    {
        TerrainZone zone = other.GetComponent<TerrainZone>();
        if (zone != null)
            terrainSpeedMultiplier = zone.speedMultiplier;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TerrainZone zone = other.GetComponent<TerrainZone>();
        if (zone != null)
            terrainSpeedMultiplier = 1f; // reset
    }
}

//Trigger Overlap Problem
//only when character changes terrains, the speed change applies
//after staying in new terrain, the new speed is not applied
//--> because two tilemaps overlap?