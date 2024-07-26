using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; 
    public float smoothSpeed = 0.50f; 
    public Vector3 offset; 

    void LateUpdate()
    {
        if (player != null)
        {
            
            Vector3 desiredPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);

            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            
            transform.position = smoothedPosition;
        }
    }
}
