using JigiJumper.Data;

namespace JigiJumper.Component
{
    public interface IPlanetEventHandler
    {
        void OnNewSpawnedPlanetInitialization(PlanetDataStructure data);
        void OnJumperEnter();
        void OnJumperExit();
        void OnDespawnedPreviousPlanet();
    }
}
