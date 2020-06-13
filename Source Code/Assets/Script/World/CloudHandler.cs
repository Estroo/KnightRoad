using System.Collections;
using UnityEngine;

public class CloudHandler : MonoBehaviour
{
    public GameObject CloudPrefab;
    public GameObject player;
    public float respawnTimer = 1.0f;

    void Start()
    {
        StartCoroutine(createCloud());
    }

    private void spawnCloud()
    {
        float newScale = Random.Range(0.8f, 1.2f);
        GameObject a = Instantiate(CloudPrefab) as GameObject;
        a.transform.parent = GameObject.Find("CloudContainer").transform;
        a.transform.localScale = new Vector3(newScale, newScale, newScale);
        a.transform.position = new Vector3(player.transform.position.x + 20, Random.Range(player.transform.position.y + 9.5f, player.transform.position.y + 8.5f), player.transform.position.z);
    }

    IEnumerator createCloud()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTimer);
            spawnCloud();
        }
    }
}
