using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] public float speed = 1;

    [Header("Acceleration")]
    [SerializeField][Range(0f,1f)] public float acceleration = 0.2f;
    [SerializeField][Range(0f,1f)] public float baseAcceleration = 0.1f;
    [SerializeField][Range(0f,1f)] public float decaySpeed = 0.1f;

    [SerializeField] public GameObject projectilePrefab;

    public Vector2 minBoundary;
    public Vector2 maxBoundary;
    private Camera gameCamera;


    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        gameCamera = GameObject.FindGameObjectWithTag("GameMainCamera").GetComponent<Camera>();

        minBoundary = gameCamera.ViewportToWorldPoint(Vector3.forward * 10);
        maxBoundary = gameCamera.ViewportToWorldPoint((Vector3.right + Vector3.up)+(Vector3.forward*10));
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if(Input.GetButton("Fire1"))
            Fire();
        if(Input.GetButton("Fire2"))
            Hit();
    }

    void Movement()
    {
        direction = Vector3.MoveTowards(direction, Vector2.zero, speed * decaySpeed * Time.deltaTime);

        float val = Input.GetAxis("Horizontal");
        direction.x += (val * ( (speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;
        val = Input.GetAxis("Vertical");
        direction.y += (val * ( (speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftArrow))
            direction.x -= acceleration * Time.deltaTime;
        if(Input.GetKey(KeyCode.RightArrow))
            direction.x += acceleration * Time.deltaTime;
        if(Input.GetKey(KeyCode.UpArrow))
            direction.y += acceleration * Time.deltaTime;
        if(Input.GetKey(KeyCode.DownArrow))
            direction.y -= acceleration * Time.deltaTime;

        direction.x = Mathf.Clamp(direction.x, -speed, speed);
        direction.y = Mathf.Clamp(direction.y, -speed, speed);

        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x+direction.x, minBoundary.x, maxBoundary.x);
        newPos.y = Mathf.Clamp(newPos.y+direction.y, minBoundary.y, maxBoundary.y);
        transform.position = newPos;
    }

    void Fire()
    {
        Instantiate(projectilePrefab,transform.position, Quaternion.identity);
    }

    void Hit()
    {
        Debug.Log("Hit");
    }
}
