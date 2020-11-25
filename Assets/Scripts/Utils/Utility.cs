using UnityEngine;


namespace JigiJumper.Utils
{
    public static class Utility
    {
        static public T[] Shuffle<T>(T[] array, int seed)
        {
            System.Random rand = new System.Random(seed);
            for (int i = 0; i < array.Length - 1; ++i)
            {
                int randomIndex = rand.Next(i, array.Length - 1);
                T temp = array[randomIndex];
                array[randomIndex] = array[i];
                array[i] = temp;
            }
            return array;
        }

        static public float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        static public float Interpolate(float percent)
        {
            return 4 * (-Mathf.Pow(percent, 2) + percent);
        }

        static public Vector2 GetRandomPosOnScreen(Camera camera)
        {
            float spawnY = Random.Range(
                    camera.ScreenToWorldPoint(new Vector2(0, 0)).y,
                    camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range(
                    camera.ScreenToWorldPoint(new Vector2(0, 0)).x,
                    camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            return new Vector2(spawnX, spawnY);
        }

        static public Vector3 ToVector3(this Vector2 vec2, float z = 0)
        {
            return new Vector3(vec2.x, vec2.y, z);
        }
    }
}