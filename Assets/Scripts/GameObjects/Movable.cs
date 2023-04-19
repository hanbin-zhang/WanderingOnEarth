using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Movable : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    [HideInInspector]
    private float rotateSpeed = 0.5f;
    public float gravity = -15.0f;
   
    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    [HideInInspector]
    private float rotation = 0;

    [HideInInspector]
    public float target = 0;

    [HideInInspector]
    private bool grounded = true;

    [HideInInspector]
    private float groundedRadius;

    [HideInInspector]
    private float verticalVelocity;

    [HideInInspector]
    private float terminalVelocity = 53f;

    [HideInInspector]
    private CharacterController controller;

    [HideInInspector]
    private Vector3 TerrainSize;

    private void Start() {
        controller = GetComponent<CharacterController>();
        groundedRadius = controller.radius;
        rotation = transform.rotation.eulerAngles.y;
        target = Random.Range(rotation - 100f, rotation + 100f);
        rotateSpeed = Random.Range(0.1f, 0.4f);

        TerrainSize = Terrain.activeTerrain.terrainData.size;
    }

    private void FixedUpdate() {
        GroundCheck();
        ApplyGravity();
        ApplyRotation();
        Move();
    }

    private void GroundCheck() {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void ApplyGravity() {
        if (grounded) {
            verticalVelocity = -2f;
        } else if (verticalVelocity < terminalVelocity) {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void ApplyRotation() {
        rotation += (rotation < target) ? rotateSpeed : -rotateSpeed;
        if (Math.Abs(target - rotation) < 1f) {
            target = Random.Range(0f, 360f);
            rotateSpeed = Random.Range(0.1f, 0.6f);
        }
    }

    private void Move()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        controller.Move(transform.forward * (moveSpeed * Time.fixedDeltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.fixedDeltaTime);

        // Get the position of the object after moving
        Vector3 currentPosition = transform.position;

        // Clamp the position of the object on the X and Z axes within the boundaries of the terrain
        float clampedX = Mathf.Clamp(currentPosition.x, 0, TerrainSize.x);
        float clampedY = Mathf.Clamp(currentPosition.y, 0, 100f);

        float clampedZ = Mathf.Clamp(currentPosition.z, 0, TerrainSize.z);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // Set the position of the object to the clamped position
        transform.position = clampedPosition;
    }

}
