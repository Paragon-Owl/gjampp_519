using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float speed = 1;
    public float speedRotation = 1;

    public float dmg = 10;
    public float totalMultiplier = 1f;
    public Vector3 direction = Vector3.up;
    private Vector3 target = Vector3.zero;
    private List<int> asteroidAlreadyCollided = new List<int>();

    public float fireDmg = 0.5f;
    public float slowImp = 3f;
    public float thunderDmg = 1f;

    private CapsuleCollider2D cc;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        cc = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (CharacterController.Instance.hasAutoGuidShot)
        {
            if (target != Vector3.zero)
            {
                Debug.Log(target);
                direction = target - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.back);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speedRotation * Time.deltaTime);
            }
        }

        transform.position =
            Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] collider2Ds = new Collider2D[5];
        if (cc.GetContacts(collider2Ds) > 0)
        {
            if (other.gameObject.CompareTag("Meteore") && !asteroidAlreadyCollided.Contains(other.GetInstanceID()))
            {
                other.gameObject.GetComponent<Asteroid>().applyDmg(dmg * totalMultiplier,
                    CharacterController.Instance.hasFireShot ? fireDmg * totalMultiplier : 0,
                    CharacterController.Instance.hasIceShot ? slowImp : 0,
                    CharacterController.Instance.hasThunderShot ? thunderDmg * totalMultiplier : 0);
                if (!CharacterController.Instance.hasPiercingShot)
                    Destroy(gameObject);
                else
                {
                    if (CharacterController.Instance.hasAutoGuidShot)
                    {
                        target = Vector3.zero;
                    }
                }

                asteroidAlreadyCollided.Add(other.GetInstanceID());
            }
        }

        if (CharacterController.Instance.hasAutoGuidShot && other.gameObject.CompareTag("Meteore") &&
            target == Vector3.zero &&
            !asteroidAlreadyCollided.Contains(other.GetInstanceID()))
        {
            asteroidAlreadyCollided.Add(other.GetInstanceID());
            target = other.transform.position;
        }

        if (CharacterController.Instance.hasBouncingShot && other.gameObject.CompareTag("Wall"))
        {
            //TODO
            //If mur Top ou Bot change Y
            //If mur Right ou Left change X
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Meteore"))
        {
            target = Vector3.zero;
        }
    }
}