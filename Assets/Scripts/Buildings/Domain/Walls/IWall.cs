using Buildings.Application;

namespace Buildings.Domain.Walls
{
    public interface IWall : IBuilding
    {
        WallConfigurator WallConfigurator { get; set; }
        
        byte IndexBase { get; set; }
        byte IndexFront { get; set; }
        byte IndexRight { get; set; }
        byte IndexBack { get; set; }
        byte IndexLeft { get; set; }
    }
}