using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private soulType type;
    private Rigidbody2D soulRigidbody;
    private bool pickable = false;
    public soulType Type
    {
        get
        {
            return type;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger && pickable)
        {
            switch (Type)
            {
                case Soul.soulType.small:
                    Debug.Log("Opa chirik");
                    collision.GetComponent<SoulsController>().Replenishment(10);
                    Destroy(gameObject);
                    break;
                case Soul.soulType.medium:
                    collision.GetComponent<SoulsController>().Replenishment(50);
                    Destroy(gameObject);
                    break;
                case Soul.soulType.large:
                    collision.GetComponent<SoulsController>().Replenishment(200);
                    Destroy(gameObject);
                    break;
            }

        }
    }
    private void Start()
    {
        soulRigidbody = GetComponent<Rigidbody2D>();
        soulRigidbody.AddForce(new Vector2(Random.Range(-100,100), Random.Range(100, 200)));
        StartCoroutine(pickUpSoul());
    }
    private IEnumerator pickUpSoul()
    {
        yield return new WaitForSeconds(0.4f);
        pickable = true;
    }
    public enum soulType
    {
        small,
        medium,
        large
    }
}
