using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PiecePlacementSystem : ComponentSystem {
    EntityQuery m_Group;
    protected override void OnCreate () {
        m_Group = GetEntityQuery (ComponentType.ReadOnly<GameComponent> ());
    }

    protected override void OnUpdate () {
        using (NativeArray<GameComponent> gameComponentsArray = m_Group.ToComponentDataArray<GameComponent> (Allocator.TempJob)) {

            GameComponent gameComponent = gameComponentsArray[0];

            if (Input.GetMouseButtonDown (0) && gameComponent.state == GameState.IN_PROGRESS) {
                float3 mouseWorldPos = Camera.main.ScreenToWorldPoint (
                    new float3 (
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        4.5f));

                var gridSize = 0.55f;

                Entities.ForEach ((RenderMesh mesh, ref TileComponent tile, ref Translation translation) => {
                    if (EntitySelected (translation, mouseWorldPos, gridSize)) {
                        if (tile.occupier != Players.NONE) return;

                        UpdateGameState ();
                        tile.occupier = gameComponent.activePlayer;
                        PieceSpawner.Instance.SpawnPiece (translation, gameComponent.activePlayer);
                    }
                });
            }
        }
    }

    private void UpdateGameState () {
        Entities.ForEach ((ref GameComponent game) => {
            game.state = GameState.PIECE_PLAYED;
        });
    }

    private bool EntitySelected (Translation translation, float3 mouseWorldPos, float gridSize) {
        if (translation.Value.x < mouseWorldPos.x + gridSize &&
            translation.Value.x > mouseWorldPos.x - gridSize) {
            if (translation.Value.y < mouseWorldPos.y + gridSize &&
                translation.Value.y > mouseWorldPos.y - gridSize) {
                return true;
            }
        }
        return false;
    }
}