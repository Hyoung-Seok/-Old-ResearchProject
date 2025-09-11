using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float speed = 30f;
    
    private void Update()
    {
        transform.RotateAround(anchor.position, Vector3.down, speed * Time.deltaTime);
    }
}
