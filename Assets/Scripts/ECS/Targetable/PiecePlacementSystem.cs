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
        // we want to access game state so we grab the game manager
        // using statement disposes of the native array for us
        using (NativeArray<GameComponent> gameComponentsArray = m_Group.ToComponentDataArray<GameComponent> (Allocator.TempJob)) {
            if (gameComponentsArray.Length == 0)
                Debug.LogError ("No game component was found in the scene");

            //grab the only entry
            GameComponent gameComponent = gameComponentsArray[0];

            // if the game is still in progress and we detect mouse input
            if (Input.GetMouseButtonDown (0) && gameComponent.state == GameState.IN_PROGRESS) {
                float3 mouseWorldPos = Camera.main.ScreenToWorldPoint (
                    new float3 (
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        4.5f));

                // we check to see if the input was over a tile
                Entities.ForEach ((RenderMesh mesh, ref TileComponent tile, ref Translation translation) => {
                    // find the specific tile 
                    if (EntitySelected (translation, mouseWorldPos)) {
                        // if there's already a piece on that tile do nothing
                        if (tile.occupier != Players.NONE) return;

                        // place a piece, update game's state on gameComponent
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

    private bool EntitySelected (Translation translation, float3 mouseWorldPos) {
        var gridSize = 0.55f;
        // AABB algorithm
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