using UnityEngine;

public static class FalloffGenerator {

    public static float[,] GenerateFalloffMap(int x,int y) {
        float[,] map = new float[x,y];

        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                float X = i / (float)x * 2 - 1;
                float Y = j / (float)y * 2 - 1;

                float value = Mathf.Max (Mathf.Abs (X), Mathf.Abs (Y));
                map [i, j] = Evaluate(value);
            }
        }

        return map;
    }

    static float Evaluate(float value) {
        float a = 10f;
        float b = 15f;

        return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
    }
}

