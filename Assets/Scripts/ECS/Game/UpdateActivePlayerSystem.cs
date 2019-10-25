using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GameSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref GameComponent game) =>
        {
            if (game.state == GameState.PIECE_PLAYED)
            {
                // swap from index 1 to index 2, skip index 0
                game.activePlayer = (Players)(((int)game.activePlayer + (int)game.activePlayer) % 3);
                game.state = GameState.IN_PROGRESS;
            }
        });
    }
}