namespace PolyWars.API.Strategies {
    public interface IMoveStrategy {
        void Move(IMoveable moveable, decimal deltaTime);
    }
}
