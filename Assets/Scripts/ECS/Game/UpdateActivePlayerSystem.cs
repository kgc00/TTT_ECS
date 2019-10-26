using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;

public class GameSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref GameComponent game) =>
        {
            if (game.state == GameState.PIECE_PLAYED)
            {
                using (NativeArray<TileComponent> tileGroup = GetEntityQuery(ComponentType.ReadOnly<TileComponent>())
                                            .ToComponentDataArray<TileComponent>(Allocator.TempJob))
                {
                    //Calculate win
                    if (CalculateWinViaColumn(tileGroup) ||
                        CalculateWinViaRow(tileGroup) ||
                        CalculateWinViaDiagonal(tileGroup))
                    {
                        game.state = GameState.FINISHED;
                    }
                    else
                    {
                        // swap from index 1 to index 2, skip index 0
                        game.activePlayer = (Players)(((int)game.activePlayer + (int)game.activePlayer) % 3);
                        game.state = GameState.IN_PROGRESS;
                    }
                }
            }
        });
    }

    private boolean CalculateWinViaColumn(NativeArray<TileComponent> tileGroup)
    {
        boolean win = false;
        int prevX = 0;
        Players lastOccupier = Players.NONE;
        var inARow = 0;
        for (int i = 0; i < tileGroup.Length; i++)
        {
            if (prevX != tileGroup[i].posX)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                prevX = tileGroup[i].posX;
            }

            if (tileGroup[i].occupier == Players.NONE)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                continue;
            }

            if (lastOccupier == Players.NONE || tileGroup[i].occupier == lastOccupier)
            {
                lastOccupier = tileGroup[i].occupier;
                inARow++;
            }

            if (inARow == 3)
            {
                win = true;
                break;
            }
        }
        return win;
    }

    private boolean CalculateWinViaRow(NativeArray<TileComponent> tileGroup)
    {

        boolean win = false;
        int prevY = 0;
        Players lastOccupier = Players.NONE;
        var inARow = 0;
        for (int i = 0; i < tileGroup.Length; i++)
        {
            // 2 => 5 => 8
            // 1 => 4 => 7
            // 0 => 3 => 6

            // grab every third index
            var formula = ((i * 3 % tileGroup.Length) + (int)Mathf.Floor(i * 0.35f));

            if (prevY != tileGroup[formula].posY)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                prevY = tileGroup[formula].posY;
            }

            if (tileGroup[formula].occupier == Players.NONE)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                continue;
            }

            if (lastOccupier == Players.NONE || tileGroup[formula].occupier == lastOccupier)
            {
                lastOccupier = tileGroup[formula].occupier;
                inARow++;
            }

            if (inARow == 3)
            {
                win = true;
                break;
            }
        }
        return win;
    }

    private boolean CalculateWinViaDiagonal(NativeArray<TileComponent> tileGroup)
    {
        if (CalculateUpwardsDiagonal(tileGroup)
        || CalculateDownwardsDiagonal(tileGroup))
        {
            return true;
        }
        return false;
    }

    private boolean CalculateUpwardsDiagonal(NativeArray<TileComponent> tileGroup)
    {
        boolean win = false;
        Players lastOccupier = Players.NONE;
        var inARow = 0;
        for (int i = 0; i < tileGroup.Length; i++)
        {
            // 2 => 5 => 8
            // 1 => 4 => 7
            // 0 => 3 => 6

            // grab every fourth index
            var formula = (i * 4);

            if (formula > tileGroup.Length)
            {
                break;
            }

            if (tileGroup[formula].occupier == Players.NONE)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                continue;
            }

            if (lastOccupier == Players.NONE || tileGroup[formula].occupier == lastOccupier)
            {
                lastOccupier = tileGroup[formula].occupier;
                inARow++;
            }

            if (inARow == 3)
            {
                win = true;
                break;
            }
        }
        return win;
    }

    private boolean CalculateDownwardsDiagonal(NativeArray<TileComponent> tileGroup)
    {
        boolean win = false;
        Players lastOccupier = Players.NONE;
        var inARow = 0;
        for (int i = 1; i < tileGroup.Length; i++)
        {
            // 2 => 5 => 8
            // 1 => 4 => 7
            // 0 => 3 => 6

            // grab every second index except the first
            var formula = (i * 2);

            if (formula > 6)
            {
                break;
            }

            if (tileGroup[formula].occupier == Players.NONE)
            {
                inARow = 0;
                lastOccupier = Players.NONE;
                continue;
            }

            if (lastOccupier == Players.NONE || tileGroup[formula].occupier == lastOccupier)
            {
                lastOccupier = tileGroup[formula].occupier;
                inARow++;
            }

            if (inARow == 3)
            {
                win = true;
                break;
            }
        }
        return win;
    }
}

