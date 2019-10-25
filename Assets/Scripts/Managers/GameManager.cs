using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EntityManager manager;
    EntityArchetype gameArchetype;
    void Start()
    {
        manager = World.Active.EntityManager;
        gameArchetype = manager.CreateArchetype(
            typeof(GameComponent)
        );
        SpawnManager();
    }

    void SpawnManager()
    {
        var startingPlayer = (Players)UnityEngine.Random.Range(1, 3);
        var entity = manager.CreateEntity(gameArchetype);
        manager.SetComponentData(entity, new GameComponent { activePlayer = startingPlayer, state = GameState.NOT_STARTED });
    }
}