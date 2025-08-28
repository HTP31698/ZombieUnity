using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public Item coin;
    public Item ammo;
    public Item heart;

    public float nextspawntimer = 0f;
    public float spawntimer = 6f;

    public float range = 1f;
    public Vector3 center;
    public Vector3 rand;
    public int randomitem;



    private void Update()
    {
       
        if (Time.time >= nextspawntimer)
        {
            SpawnItem();
            nextspawntimer = Time.time + spawntimer;
        }
    }
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        rand = new Vector3(Random.Range(-7, 7), 0.01f, Random.Range(-7, 7));
        Vector3 randomPoint = center + rand * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    public void SpawnItem()
    {
        Vector3 position;
        randomitem = Random.Range(0, 3);
        if (RandomPoint(center, range, out position))
        {
            if(randomitem == 0)
            {
                Instantiate(coin, position, Quaternion.identity);
            }
            else if(randomitem == 1)
            {
                Instantiate(ammo, position, Quaternion.identity);
            }
            else if(randomitem == 2)
            {
                Instantiate(heart, position, Quaternion.identity);
            }
        }
    }

}
