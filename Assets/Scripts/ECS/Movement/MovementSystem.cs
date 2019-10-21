using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : ComponentSystem {
    protected override void OnUpdate () {
        Entities.ForEach ((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent) => {
            translation.Value.y += moveSpeedComponent.moveSpeed * Time.deltaTime;

            if (translation.Value.y > 10 || translation.Value.y < -10) {
                moveSpeedComponent.moveSpeed *= -1;
            }
        });
    }
}