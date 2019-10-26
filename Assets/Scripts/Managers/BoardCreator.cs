using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private Material material2;
    void Start()
    {
        EntityManager manager = World.Active.EntityManager;
        EntityArchetype boardArchetype, pieceArchetype, tileArchetype;
        DefineArchetypes(manager, out boardArchetype, out pieceArchetype, out tileArchetype);
        SpawnBoard(manager, boardArchetype, tileArchetype);
    }

    private void SpawnBoard(EntityManager manager, EntityArchetype boardArchetype, EntityArchetype tileArchetype)
    {
        var board = manager.CreateEntity(boardArchetype);
        SpawnBoardBackground(manager);
        SpawnBoardTiles(manager, tileArchetype, board);
    }

    private void SpawnBoardTiles(EntityManager manager, EntityArchetype tileArchetype, Entity board)
    {
        var boardSizeX = 3f;
        var boardSizeY = 3f;

        var spacing = 1.1f;
        var offset = spacing / 2;

        manager.SetComponentData(board, new BoardComponent { gridX = boardSizeX, gridY = boardSizeY });

        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                var entity = manager.CreateEntity(tileArchetype);
                manager.SetComponentData(entity, new Translation { Value = new float3(i * spacing - offset, j * spacing - offset, 0) });
                manager.SetComponentData(entity, new TileComponent { occupier = Players.NONE, posX = i, posY = j });
                manager.SetSharedComponentData(entity, new RenderMesh { material = material, mesh = mesh });
            }
        }
    }

    private void SpawnBoardBackground(EntityManager manager)
    {
        var backgroundArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Scale)
        );

        var entity2 = manager.CreateEntity(backgroundArchetype);
        manager.SetComponentData(entity2, new Scale { Value = 4f });
        manager.SetComponentData(entity2, new Translation { Value = new float3(0.5f, 0.5f, 2f) });
        manager.SetSharedComponentData(entity2, new RenderMesh { material = material2, mesh = mesh });
    }

    void DefineArchetypes(
        EntityManager manager,
        out EntityArchetype boardArchetype,
        out EntityArchetype pieceArchetype,
        out EntityArchetype tileArchetype)
    {
        boardArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(BoardComponent)
        );
        pieceArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(PieceComponent)
        );
        tileArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(TileComponent)
        );
    }
}