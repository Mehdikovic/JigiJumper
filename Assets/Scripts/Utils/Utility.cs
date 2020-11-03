using UnityEngine;


namespace JigiJumper.Utils
{
    public class Utility
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
    }
}