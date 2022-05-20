using System.Collections;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

    public Transform player;
    public Transform bullet;
    public Transform reciever;

    private bool playerIsOverlapping = false;
    private bool bulletIsOverlapping = false;

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f)
            {
                // Teleport him!
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = reciever.position + positionOffset;

                playerIsOverlapping = false;
            }
        }

        if (bulletIsOverlapping)
        {
            Vector3 portalToBullet = bullet.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToBullet);

            // If this is true: The player has moved across the portal
            //if (dotProduct < 0f)
            //{
            // Teleport him!
            float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
            rotationDiff += 180;
            bullet.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToBullet;
            bullet.position = reciever.position + positionOffset;

            bulletIsOverlapping = false;
            //}
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }

        if (other.tag == "Bullet")
        {
            other.gameObject.GetComponent<TrailRenderer>().emitting = false;
            bulletIsOverlapping = true;
            bullet = other.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }

        if (other.tag == "Bullet")
        {
            bulletIsOverlapping = false;
            StartCoroutine(StartEmitting(other));
        }
    }

    IEnumerator StartEmitting(Collider other)
    {
        yield return new WaitForSeconds(0.1f);
        other.gameObject.GetComponent<TrailRenderer>().emitting = true;
    }
}
