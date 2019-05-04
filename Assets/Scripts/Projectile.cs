using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public float speed = 10;
    public float speedRotation = 10;

    public bool isAutoGuide = true;
    public Vector3 direction = Vector3.up;
    private Vector3 target = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        if (isAutoGuide)
        {
            direction = transform.position - target;
            direction.Normalize();
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(direction,Vector3.back),speedRotation * Time.deltaTime);
        }
        else
        {    
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteore"))
        {
            target = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Meteore"))
        {
            target = Vector3.zero;
        }
    }

    // Update is called once per frame
}
