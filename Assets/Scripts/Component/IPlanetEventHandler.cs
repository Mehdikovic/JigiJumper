using JigiJumper.Actors;
using JigiJumper.Data;

namespace JigiJumper.Component
{
    public interface IPlanetEventHandler
    {
        void OnInitialDataReceived(JumperController jumper,PlanetDataStructure data);

        void OnJumperEnter();
        void OnJumperExit();
    }
}
