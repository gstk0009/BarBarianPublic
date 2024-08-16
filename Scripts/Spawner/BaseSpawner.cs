using UnityEngine;
using UnityEngine.AI;

public class BaseSpawner : MonoBehaviour
{
    int maxAttempts = 300;
    int attempts = 0;

    protected float spawnDistanceFromPlayer = 3f;
    public Vector3 mapMinBounds;
    public Vector3 mapMaxBounds;
    static int walkableAreaMask;

    private void Start()
    {
        walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");
    }

    protected Vector3 GetRandomPos(bool checkDistanceFromPlayer = true)
    {
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(mapMinBounds.x, mapMaxBounds.x),
                Random.Range(mapMinBounds.y, mapMaxBounds.y)
            );

            if (NavMesh.SamplePosition(randomPosition, out hit, 1.0f, walkableAreaMask))
            {
                finalPosition = hit.position;

                if (!checkDistanceFromPlayer)
                {
                    return finalPosition;
                }
                else
                {
                    if (IsPositionValidFromPlayer(finalPosition, Player.Instance.transform.position))
                    {
                        return finalPosition;
                    }
                }
            }

            attempts++;
        }

        return Vector3.zero;
    }

    private bool IsPositionValidFromPlayer(Vector3 spawnPos, Vector3 playerPos)
    {
        return Vector3.Distance(spawnPos, playerPos) >= spawnDistanceFromPlayer;
    }
}
