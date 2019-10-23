using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public static PieceSpawner Instance;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Mesh mesh2;
    [SerializeField] private Material material;
    [SerializeField] private Material material2;
    EntityManager manager;
    EntityArchetype pieceArchetype;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Debug.Log(Instance);
        manager = World.Active.EntityManager;
        pieceArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(PieceComponent)
        );
    }

    public void SpawnPiece(Translation translation)
    {
        float3 spawnPos = translation.Value;
        spawnPos.z -= 0.1f;
        var entity = manager.CreateEntity(pieceArchetype);
        manager.SetComponentData(entity, new Translation { Value = spawnPos });
        manager.SetSharedComponentData(entity, new RenderMesh { material = material, mesh = mesh });
    }
}