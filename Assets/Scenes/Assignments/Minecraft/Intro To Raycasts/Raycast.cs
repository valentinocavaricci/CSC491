using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask pickupLayer;
    public float holdHeight = 2f;

    private Camera mainCam;
    private RaycastHit hit;

    private GameObject heldObject;
    private Rigidbody heldRB;

    private Vector3 holdPoint;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        
        if (heldObject == null)
        {
            if (Physics.Raycast(ray, out hit, rayDistance, pickupLayer))
            {
                Debug.Log("Hovering: " + hit.collider.name);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    heldObject = hit.collider.gameObject;
                    heldRB = heldObject.GetComponent<Rigidbody>();

                    if (heldRB != null)
                    {
                        heldRB.useGravity = false;
                        heldRB.linearVelocity = Vector3.zero;
                        heldRB.isKinematic = true;
                    }

                    holdPoint = hit.point;
                    Debug.Log("Pickeup: " + heldObject.name);
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, rayDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                holdPoint = hit.point;
            }

            Vector3 targetPos = holdPoint + Vector3.up * holdHeight;
            heldObject.transform.position = targetPos;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (heldRB != null)
                {
                    heldRB.useGravity = true;
                    heldRB.isKinematic = false;
                }

                Debug.Log("Dropped: " + heldObject.name);

                heldObject = null;
                heldRB = null;
            }
        }
    }
}