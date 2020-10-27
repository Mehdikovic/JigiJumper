using JigiJumper.Data;

namespace JigiJumper.Component
{
    public interface IPlanetEventHandler
    {
        void OnInitialDataReceived(PlanetDataStructure data);

        void OnJumperEnter();
        void OnJumperExit();
    }
}
