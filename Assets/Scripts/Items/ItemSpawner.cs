using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public float minTime = 1f;
    public float maxTime = 100f;


    private GameObject[] itemSpawnPoints;

    [SerializeField]
    private GameObject item;

    private GameObject currentItem;

    private int RandomPointNumber = 0;



	// Use this for initialization
	void Awake() {
        itemSpawnPoints = GameObject.FindGameObjectsWithTag("Floor");

        //RandomPointNumber = Random.Range(0, itemSpawnPoints.Length);

        //SpawnItem();

        StartCoroutine(SpawnPerTime());

	}

    private IEnumerator SpawnPerTime() {
        while (true) {
            RandomPointNumber = Random.Range(0, itemSpawnPoints.Length);
            SpawnItem();
            float randTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randTime);
        }
    }

    private void SpawnItem() {
        
        if (itemSpawnPoints != null) {
            Vector3 position = itemSpawnPoints[RandomPointNumber].transform.position;
            position += new Vector3(0, 2f, 0);
            currentItem = Instantiate(item, position, Quaternion.identity) as GameObject;
            currentItem.GetComponent<ItemScript>().ChooseRandomState();

        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
