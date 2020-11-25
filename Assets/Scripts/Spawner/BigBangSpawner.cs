using System.Collections;
using UnityEngine;

using static JigiJumper.Utils.Utility;

namespace JigiJumper.Spawner
{
    public class BigBangSpawner : MonoBehaviour
    {
        public float speedOffset = .01f;
        public float lengthMultiplier = 40f;
        public int numToSpawn = 200;
        public PE2D.WrapAroundType wrapAround;

        IEnumerator Start()
        {
            Camera camera = Camera.main;
            yield return new WaitForSeconds(1.5f);

            while (true)
            {
                SpawnExplosion(GetRandomPosOnScreen(camera));
                yield return new WaitForSeconds(Random.Range(0.3f, 2));
            }
        }

        private void SpawnExplosion(Vector2 position)
        {
            float hue1 = Random.Range(0, 6);
            float hue2 = (hue1 + Random.Range(0, 2)) % 6f;
            Color colour1 = PE2D.StaticExtensions.Color.FromHSV(hue1, 0.5f, 1);
            Color colour2 = PE2D.StaticExtensions.Color.FromHSV(hue2, 0.5f, 1);

            for (int i = 0; i < numToSpawn; i++)
            {
                float speed = (18f * (1f - 1 / Random.Range(1f, 10f))) * speedOffset;

                var state = new PE2D.ParticleBuilder()
                {
                    velocity = PE2D.StaticExtensions.Random.RandomVector2(speed, speed),
                    wrapAroundType = wrapAround,
                    lengthMultiplier = lengthMultiplier,
                    velocityDampModifier = 0.94f,
                    removeWhenAlphaReachesThreshold = true
                };

                var colour = Color.Lerp(colour1, colour2, Random.Range(0, 1));

                float duration = 320f;
                var initialScale = new Vector2(2f, 1f);


                PE2D.ParticleFactory.instance.CreateParticle(position, colour, duration, initialScale, state);
            }
        }
    }
}