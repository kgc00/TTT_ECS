using Unity.Entities;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    void Start () {
        EntityManager manager = World.Active.EntityManager;
        EntityArchetype playerArchetype = manager.CreateArchetype (
            typeof (PlayerComponent)
        );
        SpawnPlayers (manager, playerArchetype);
    }

    private void SpawnPlayers (EntityManager manager, EntityArchetype playerArchetype) {
        for (int i = 0; i < 2; i++) {
            var entity = manager.CreateEntity (playerArchetype);
            manager.SetComponentData (entity, new PlayerComponent { id = (Players) i });
        }
    }
}