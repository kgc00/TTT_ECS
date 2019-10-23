using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

public class TargetableSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Event currentEvent = Event.current;
            float3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new float3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                4.5f));

            var threshhold = 0.55f;

            Entities.ForEach((RenderMesh mesh, ref TileComponent tile, ref Translation translation, ref TargetableComponent targetable) =>
            {
                if (EntitySelected(translation, mouseWorldPos, threshhold))
                {
                    tile.occupier = Players.PLAYER_ONE;
                    Debug.Log("Heya " + tile.occupier);
                    PieceSpawner.Instance.SpawnPiece(translation);
                }
            });
        }
    }

    private bool EntitySelected(Translation translation, float3 mouseWorldPos, float threshhold)
    {
        if (translation.Value.x < mouseWorldPos.x + threshhold &&
                        translation.Value.x > mouseWorldPos.x - threshhold)
        {
            if (translation.Value.y < mouseWorldPos.y + threshhold &&
         translation.Value.y > mouseWorldPos.y - threshhold)
            {
                return true;
            }
        }
        return false;
    }
}