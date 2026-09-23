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

    private Rigidbody2D rb;

    private float terrainSpeedMultiplier = 1f;
   // private List<TerrainZone> activeZones = new List<TerrainZone>();

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

        UnityEngine.Debug.Log("Move Action: " + moveAction);
        UnityEngine.Debug.Log("Sprint Action: " + sprint);
        UnityEngine.Debug.Log("Interact Action: " + interact);

        if (moveAction != null)
            moveAction.Enable();

        if (sprint != null)
            sprint.Enable();

        if (interact != null)
            interact.Enable();

        StartCoroutine(InitialTerrainCheck());
    }

    private IEnumerator InitialTerrainCheck()
    {
        yield return null;

        LayerMask groundLayer = LayerMask.GetMask("Ground");
        Collider2D[] startZones =
            Physics2D.OverlapCircleAll(transform.position, 1f, groundLayer);

        foreach (Collider2D col in startZones)
        {
            UnityEngine.Debug.Log("Found collider: " + col.gameObject.name);

            TerrainZone zone = col.GetComponent<TerrainZone>();

            if (zone != null)
            {
                terrainSpeedMultiplier = zone.speedMultiplier;

                UnityEngine.Debug.Log(
                    "Start Zone: " + col.gameObject.name +
                    " | Speed: " + zone.speedMultiplier
                );

                break;
            }
        }
    }

    void Update()
    {
        if (moveAction != null)
            moveInput = moveAction.ReadValue<Vector2>();

        if (sprint != null)
            isSprinting = sprint.ReadValue<float>() > 0.5f;

        if (interact != null && interact.WasPressedThisFrame())
        {
            UnityEngine.Debug.Log("E WAS PRESSED!");
            TryInteract();
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        float currentSpeed = isSprinting
            ? moveSpeed * sprintMultiplier
            : moveSpeed;

        currentSpeed *= terrainSpeedMultiplier;
        rb.linearVelocity = moveInput * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TerrainZone zone = other.GetComponent<TerrainZone>();

        if (zone != null)
        {
            terrainSpeedMultiplier = zone.speedMultiplier;

            UnityEngine.Debug.Log(
                "Entered: " + other.gameObject.name +
                " | Speed Multiplier: " + terrainSpeedMultiplier
            );
        }
    }

    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        TerrainZone zone = other.GetComponent<TerrainZone>();
        if (zone != null)
        {
            activeZones.Remove(zone);

            // Nimm die Zone die zuletzt betreten wurde (nicht die letzte im Stack)
            if (activeZones.Count > 0)
            {
                TerrainZone currentZone = activeZones[activeZones.Count - 1];
                terrainSpeedMultiplier = currentZone.speedMultiplier;
                UnityEngine.Debug.Log("Exit, now on: " + currentZone.speedMultiplier);
            }
            else
            {
                terrainSpeedMultiplier = 1f;
                UnityEngine.Debug.Log("Exit, default speed");
            }
        }
    }
    */

    private void TryInteract()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        UnityEngine.Debug.Log("Checking for nearby assets...");

        foreach (Collider2D obj in objects)
        {
            AssetInteraction asset = obj.GetComponent<AssetInteraction>();
            if (asset != null)
            {
                UnityEngine.Debug.Log("ASSET FOUND: " + asset.assetName);
                asset.SelectAsset();
                return;
            }
        }
        UnityEngine.Debug.Log("NO ASSET NEARBY!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}