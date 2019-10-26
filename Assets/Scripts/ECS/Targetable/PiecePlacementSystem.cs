using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;
using Unity.Collections;

public class PiecePlacementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        GameComponent gameComponent = AccessGameComponent();

        if (Input.GetMouseButtonDown(0) && gameComponent.state == GameState.IN_PROGRESS)
        {
            float3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new float3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                4.5f));

            var gridSize = 0.55f;



            Entities.ForEach((RenderMesh mesh, ref TileComponent tile, ref Translation translation) =>
            {
                if (EntitySelected(translation, mouseWorldPos, gridSize))
                {
                    if (tile.occupier != Players.NONE) return;

                    UpdateGameState();
                    tile.occupier = gameComponent.activePlayer;
                    PieceSpawner.Instance.SpawnPiece(translation, gameComponent.activePlayer);
                }
            });
        }
    }

    private GameComponent AccessGameComponent()
    {
        NativeArray<GameComponent> m_Group = GetEntityQuery(ComponentType.ReadOnly<GameComponent>())
                                            .ToComponentDataArray<GameComponent>(Allocator.TempJob);

        GameComponent gameComponent = m_Group[0];
        m_Group.Dispose();
        return gameComponent;
    }

    private void UpdateGameState()
    {
        Entities.ForEach((ref GameComponent game) =>
        {
            game.state = GameState.PIECE_PLAYED;
        });
    }

    private bool EntitySelected(Translation translation, float3 mouseWorldPos, float gridSize)
    {
        if (translation.Value.x < mouseWorldPos.x + gridSize &&
                        translation.Value.x > mouseWorldPos.x - gridSize)
        {
            if (translation.Value.y < mouseWorldPos.y + gridSize &&
         translation.Value.y > mouseWorldPos.y - gridSize)
            {
                return true;
            }
        }
        return false;
    }
}