using System;
using System.Collections;
using System.Collections.Generic;
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

	private static readonly int XDirHash = Animator.StringToHash("XDir");
	private static readonly int YDirHash = Animator.StringToHash("YDir");
	private Animator animator;
	private Rigidbody2D rb;

    private float terrainSpeedMultiplier = 1f;
   // private List<TerrainZone> activeZones = new List<TerrainZone>();

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

        moveAction = InputSystem.actions.FindAction("Move");
        sprint = InputSystem.actions.FindAction("Sprint");
        interact = InputSystem.actions.FindAction("Interact");

        UnityEngine.Debug.Log("Move Action: " + moveAction);
        UnityEngine.Debug.Log("Sprint Action: " + sprint);
        UnityEngine.Debug.Log("Interact Action: " + interact);

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
	void UpdateMoveInput(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
		UpdateVelocity();

		// Update animator parameters
		if (animator != null)
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
        TerrainZone zone = other.GetComponent<TerrainZone>();

        if (zone != null)
        {
            terrainSpeedMultiplier = zone.speedMultiplier;

            Debug.Log(
                "Entered: " + other.gameObject.name +
                " | Speed Multiplier: " + terrainSpeedMultiplier
            );
        }
    }

	private void TryInteract()
	{
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

				asset.SelectAsset();
				return;
			}
		}

		Debug.Log("NO ASSET NEARBY!");
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}