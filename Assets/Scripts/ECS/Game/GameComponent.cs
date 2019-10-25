using Unity.Entities;

public struct GameComponent : IComponentData
{
    public Players activePlayer;
    public GameState state;
}