using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using Unity.XR.CoreUtils; // This fixes the compiler error you had

public class UltimateAnomaly : MonoBehaviour
{
    [Header("Lift Settings")]
    public float liftSpeed = 3.0f;
    public float heightToRespawn = 15.0f;

    [Header("Teleport Settings")]
    public Transform respawnPoint;

    private GameObject playerRoot;
    private CharacterController controller;
    private bool isLifting = false;
    private float liftCooldown = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (liftCooldown > 0 || isLifting) return;

        if (other.CompareTag("Player"))
        {
            if (Vector3.Dot(Camera.main.transform.forward, Vector3.up) > 0.8f)
            {
                StartLift(other.transform.root.gameObject);
            }
        }
    }

    void StartLift(GameObject root)
    {
        isLifting = true;
        playerRoot = root;

        // Find the CharacterController on your XR Origin
        controller = playerRoot.GetComponentInChildren<CharacterController>();
        var providers = playerRoot.GetComponentsInChildren<LocomotionProvider>();

        // Disable physical collisions so you can float UP through geometry if needed
        if (controller) controller.enabled = false;
        foreach (var p in providers) p.enabled = false;
    }

    void Update()
    {
        if (liftCooldown > 0) liftCooldown -= Time.deltaTime;

        if (isLifting && playerRoot != null)
        {
            playerRoot.transform.position += Vector3.up * liftSpeed * Time.deltaTime;

            if (playerRoot.transform.position.y >= transform.position.y + heightToRespawn)
            {
                CompleteRespawn();
            }
        }
    }

    void CompleteRespawn()
    {
        if (playerRoot == null) return;

        // 1. Move to Respawn Point
        playerRoot.transform.position = respawnPoint.position;

        // 2. VR Rotation Reset (Forces vision to default forward)
        XROrigin rig = playerRoot.GetComponent<XROrigin>();
        if (rig != null)
        {
            rig.MatchOriginUpCameraForward(respawnPoint.up, respawnPoint.forward);
        }

        // 3. Sync Physics and Re-enable solid body
        Physics.SyncTransforms();

        // RE-ENABLE: This makes you solid again!
        if (controller) controller.enabled = true;

        var providers = playerRoot.GetComponentsInChildren<LocomotionProvider>();
        foreach (var p in providers) p.enabled = true;

        liftCooldown = 3.0f;
        isLifting = false;
        playerRoot = null;
    }
}