using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InescapableAnomaly : MonoBehaviour
{
    public float liftSpeed = 3.0f;
    public float killHeight = 15.0f;
    public GameObject xrOrigin;
    public Vector3 respawnPosition;

    private CharacterController controller;
    private bool isLifting = false;
    private float lockedX;
    private float lockedZ;

    void Start()
    {
        if (xrOrigin != null)
            controller = xrOrigin.GetComponent<CharacterController>();
    }

    void OnTriggerStay(Collider other)
    {
        // Tagged "Player"
        if (other.CompareTag("Player") && !isLifting)
        {
            float dot = Vector3.Dot(Camera.main.transform.forward, Vector3.up);
            if (dot > 0.8f)
            {
                isLifting = true;
                // LOCK the X and Z so you can't walk away
                lockedX = xrOrigin.transform.position.x;
                lockedZ = xrOrigin.transform.position.z;

                if (controller != null) controller.enabled = false;
            }
        }
    }

    void Update()
    {
        if (isLifting)
        {
            // FORCE the position (this kills joystick movement)
            float newY = xrOrigin.transform.position.y + (liftSpeed * Time.deltaTime);
            xrOrigin.transform.position = new Vector3(lockedX, newY, lockedZ);

            // Absolute check for Y
            if (xrOrigin.transform.position.y >= killHeight)
            {
                DoRespawn();
            }
        }
    }

    void DoRespawn()
    {
        isLifting = false;

        // Move to respawn point
        xrOrigin.transform.position = respawnPosition;

        // Re-enable physics
        if (controller != null) controller.enabled = true;
    }
}