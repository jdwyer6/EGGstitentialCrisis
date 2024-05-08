using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCoop : MonoBehaviour
{
    public bool hasChicken;
    public GameObject chicken;
    public GameObject eggPrefab;
    public float minTimeToLayEgg = 10f;
    public float maxTimeToLayEgg = 25f;
    public Transform eggLayLocation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LayEggTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddChicken() {
        hasChicken = true;
        chicken.SetActive(true);
    }

    private IEnumerator LayEggTimer() {
        yield return new WaitForSeconds(Random.Range(minTimeToLayEgg, maxTimeToLayEgg));
        if (hasChicken) {
            LayEgg();
        }
    } 

    private void LayEgg() {
        Instantiate(eggPrefab, eggLayLocation.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
    }

}
