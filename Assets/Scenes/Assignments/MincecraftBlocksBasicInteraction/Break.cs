using UnityEngine;

public class Break : MonoBehaviour
{
    public float rayDistance = 6f;
    public GameObject blockPrefab;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Destroy(hit.collider.gameObject);
            }

  
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 placePosition = hit.point + hit.normal * 0.5f;

                placePosition = new Vector3(
                    Mathf.Round(placePosition.x),
                    Mathf.Round(placePosition.y),
                    Mathf.Round(placePosition.z)
                );

                Instantiate(blockPrefab, placePosition, Quaternion.identity);
            }
        }
    }
}