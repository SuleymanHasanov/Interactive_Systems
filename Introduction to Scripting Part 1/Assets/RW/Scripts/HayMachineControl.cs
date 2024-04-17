using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMachineControl : MonoBehaviour
{
    public float inputKeyValue;
    public float moveAmount = 1;
    public float thresholdShoot = 0.5f;
    float measureTime = 0;

    public GameObject hayShootObject;
    public Transform haySpawnpoint;

    public Transform modelParent; // 1
    // 2
    public GameObject blueModelPrefab;
    public GameObject yellowModelPrefab;
    public GameObject redModelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        measureTime = Time.time;
        LoadModel();
    }

    private void LoadModel()
    {
        Destroy(modelParent.GetChild(0).gameObject); // 1

        switch (GameSettings.hayMachineColor) // 2
        {
            case HayMachineColor.Blue:
                Instantiate(blueModelPrefab, modelParent);
                break;

            case HayMachineColor.Yellow:
                Instantiate(yellowModelPrefab, modelParent);
                break;

            case HayMachineColor.Red:
                Instantiate(redModelPrefab, modelParent);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Space) && (Time.time - measureTime) > thresholdShoot)
        {
            Shoot();
            measureTime = Time.time;
        }

    }

    void Move()
    {
        inputKeyValue = Input.GetAxis("Horizontal");
        Vector3 newPos = transform.position + Vector3.right * moveAmount * inputKeyValue;
        if (newPos.x > -20 && newPos.x < 20)
        {
            transform.Translate(Vector3.right * moveAmount * inputKeyValue);
        }

    }

    void Shoot()
    {
        Instantiate(hayShootObject, haySpawnpoint.position, Quaternion.identity);
        SoundManager.Instance.PlayShootClip();
    }
}
