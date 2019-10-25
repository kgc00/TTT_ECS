using Unity.Entities;

public struct TileComponent : IComponentData
{
    public Players occupier;
}