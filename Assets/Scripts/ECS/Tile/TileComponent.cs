using Unity.Entities;

public struct TileComponent : IComponentData
{
    public int eventArgs;
    public Players occupier;
}