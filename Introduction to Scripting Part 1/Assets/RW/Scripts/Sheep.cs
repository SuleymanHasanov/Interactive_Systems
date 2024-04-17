using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float speed = 1;
    public float gotHayDestroyDelay; // 2
    private bool hitByHay; // 3
    private bool dropped;

    public float dropDestroyDelay; // 1
    private Collider myCollider; // 2
    private Rigidbody myRigidbody; // 3

    private SheepSpawner sheepSpawner;

    public float heartOffset; // 1
    public GameObject heartPrefab; // 2

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
    }

    private void HitByHay()
    {
        GameStateManager.Instance.SavedSheep();

        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true; // 1
        speed = 0; // 2

        SoundManager.Instance.PlaySheepHitClip();
        
        Destroy(gameObject, gotHayDestroyDelay); // 3

        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; // 1
        tweenScale.targetScale = 0; // 2
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3
    }

    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // 2
        {
            Destroy(other.gameObject); // 3
            HitByHay(); // 4
        }
        else if (other.CompareTag("DropSheep") && !dropped)
        {
            Drop();
        }
    }

    private void Drop()
    {
        Debug.Log("SHEEP DROP");
        GameStateManager.Instance.DroppedSheep();
        dropped = true; // 1

        sheepSpawner.RemoveSheepFromList(gameObject);
        myRigidbody.isKinematic = false; // 1
        myCollider.isTrigger = false; // 2
        SoundManager.Instance.PlaySheepDroppedClip();
        Destroy(gameObject, dropDestroyDelay); // 3
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }
}
