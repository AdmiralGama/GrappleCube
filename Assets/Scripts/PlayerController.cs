using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject grapplePrefab;

    GameObject grapple = null;
    Vector3 grapplePoint = new Vector3(0, 0, 0);

    float grappleForce = 5.0f;

    public float raycastDistance = 50f; // Maximum raycast distance
    public LayerMask raycastLayer; // Specify the layers to consider for raycasting

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && grapple == null)
        {
            MakeGrappple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Destroy(grapple);
            grapple = null;
        }

        UpdateGrapple();

        if (grapple != null)
        {
            GetComponent<Rigidbody2D>().AddForce((grapplePoint - transform.position).normalized * grappleForce);
        }
    }

    void MakeGrappple()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, raycastLayer);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;

            Vector3 position = transform.position;
            Vector3 midPoint = (position + grapplePoint) / 2;

            grapple = Instantiate(grapplePrefab, midPoint, Quaternion.identity);
        }
    }

    void UpdateGrapple()
    {
        Vector3 position = transform.position;
        Vector3 midPoint = (position + grapplePoint) / 2;
        float length = (grapplePoint - position).magnitude;

        if (grapple != null)
        {
            grapple.transform.position = midPoint;
            grapple.transform.localScale = new Vector3(0.2f, length, 1.0f);

            Quaternion rotation = Quaternion.LookRotation(grapplePoint - grapple.transform.position, transform.TransformDirection(Vector3.forward));
            grapple.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
    }
}
