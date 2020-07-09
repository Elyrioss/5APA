using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class TextureModifier : MonoBehaviour
{
    //public GameObject treeObject;
    //public Tree t;

    //Mat
    Material[] mats;
    public Texture2D savedTex;
    public Texture2D tex;

    public Material newMat;
    public Material previousMat;

    //Params
    public string newNameText;

    public int age;
    public float agingValue = 0.05f;

    //public int nbOrigin;
    //public int maxRadiusOrigin;
    //public float variationValue = 0.5f;

    //public List<Origin> originList;

    public Color colorRef = new Color(181, 97, 0, 0.0f);

    //private bool isCoroutineRunning = false;
    
    //public int iteration = 10;

    public void LaunchAgingTexture()
    {
        AgingTexture();
    }

    public void AgingTexture()
    {
        newMat = new Material(GetComponent<Renderer>().sharedMaterial);
        newMat.CopyPropertiesFromMaterial(GetComponent<Renderer>().sharedMaterial);
        
        Texture2D texTest = new Texture2D(newMat.mainTexture.width, newMat.mainTexture.height, TextureFormat.RGBA32, true);
        texTest.Apply();

        Graphics.CopyTexture(newMat.mainTexture, texTest);
        texTest.Apply();

        //CopyTextures(newMat.mainTexture as Texture2D, ref texTest);
        
        texTest = newMat.mainTexture as Texture2D;
        byte[] bytes = texTest.EncodeToPNG();

        File.WriteAllBytes("Assets/AgingAddon/Textures/NewTexture" + newNameText + ".png", bytes);

        AssetDatabase.Refresh();

        //texTest = Resources.Load("Assets/AgingAddon/Textures/NewTexture" + texTest.name + ".png") as Texture2D;

        //texTest.LoadImage((byte[])bytes);
        //texTest.filterMode = FilterMode.Point;

        texTest = EditorGUIUtility.Load("Assets/AgingAddon/Textures/NewTexture" + newNameText + ".png") as Texture2D;
        texTest.Apply();
        
        newMat.mainTexture = texTest;
        
        //AssetDatabase.CreateAsset(texTest, "Assets/AgingAddon/Textures/NewTexture.png");
        AssetDatabase.CreateAsset(newMat, "Assets/AgingAddon/Materials/NewSword" + newNameText + ".mat");

        GetComponent<Renderer>().sharedMaterial = newMat;

        //mats = GetComponent<MeshRenderer>().sharedMaterials;
        tex = newMat.mainTexture as Texture2D;
        //Texture2D tex2D = tex;

        /*if (mats.Length > 0)
        {
            originList.Clear();

            Texture2D tex2D = tex;

            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    tex.SetPixel(i, j, CombineColorsLerp(tex.GetPixel(i, j), Color.black, agingValue * age, false));
                }
            }

            tex2D.Apply();

            tex = tex2D;

            tex.Apply();
        }*/

        /*if (mats.Length > 0)
        {*/
            //t = treeObject.GetComponent<Tree>();

            //t.maxWalkers = age * 15;
            //if(t.maxWalkers > 750)
            //{
            //    t.maxWalkers = 750;
            //}

            //originList.Clear();

            //for (int i = 0; i < nbOrigin; i++)
            //{
            //    Origin newCircle = new Origin(UnityEngine.Random.Range(0, tex.width), UnityEngine.Random.Range(0, tex.height), UnityEngine.Random.Range(5, maxRadiusOrigin));

            //    //t.InitTree(tex.width, tex.height);
            //    t.InitTree(600, 600);
            //    t.DrawTree();

            //    Vector3 diff = new Vector3(newCircle.x, newCircle.y, 0) - t.rootPosition;

            //    if(t.tree.Count > 1)
            //    {
            //        for (int j = 0; j < t.tree.Count; j++)
            //        {
            //            Color newColorRef = CombineColorsLerp(tex.GetPixel((int)(t.tree[j].position.x + diff.x), (int)(t.tree[j].position.y + diff.y)), colorRef, 0.5f, false);

            //            DrawCircle(tex, ref tex2D, newColorRef, (int)(t.tree[j].position.x + diff.x), (int)(t.tree[j].position.y + diff.y), (int)(t.tree[j].radius * (age * 0.05f)));
            //        }
            //    }

            //}

            //t.ClearGOWalker();

        Vector3 previousPosition = transform.position;

        transform.position = new Vector3(UnityEngine.Random.Range(1, 500), UnityEngine.Random.Range(1, 500), 3);

        //Init(tex.width, tex.height);
        FillTexture(tex);

        //tex2D.Apply();

        //tex = tex2D;

        tex.Apply();

        transform.position = previousPosition;
        
        //}
    }

    /*public void ClearTree()
    {
        t = treeObject.GetComponent<Tree>();
        t.ResetTree();
        t.ClearGO();
    }*/

    public void SaveMaterial()
    {
        previousMat = GetComponent<MeshRenderer>().sharedMaterial;

        Debug.Log("Material saved");
        /*Texture2D copyTex = new Texture2D(mats[0].mainTexture.width, mats[0].mainTexture.height, TextureFormat.RGBA32, false);
        CopyTextures(mats[0].mainTexture as Texture2D, ref copyTex);
        savedTex = copyTex;
        savedTex = GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture as Texture2D;
        savedTex.Apply();*/
    }

    public void RestoreMaterial()
    {
        GetComponent<MeshRenderer>().sharedMaterial = previousMat;

        Debug.Log("Material saved");

        /*mats = GetComponent<MeshRenderer>().sharedMaterials;

        Debug.Log("Texture restored");
        Texture2D copyTex = new Texture2D(savedTex.width, savedTex.height, TextureFormat.RGBA32, false);
        CopyTextures(savedTex, ref copyTex);
        //mats[0].mainTexture = savedTex;
        tex = copyTex;
        tex = GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture as Texture2D;
        tex.Apply();
        mats[0].mainTexture = tex;*/
    }

    public void CopyTextures(Texture2D aBaseTexture, ref Texture2D aToCopyTexture)
    {
        int aWidth = aBaseTexture.width;
        int aHeight = aBaseTexture.height;
        //Texture2D aReturnTexture = new Texture2D(aWidth, aHeight, TextureFormat.RGBA32, false);

        Color[] aBaseTexturePixels = aBaseTexture.GetPixels();
        Color[] aCopyTexturePixels = aToCopyTexture.GetPixels();
        //Color[] aColorList = new Color[aBaseTexturePixels.Length];
        int aPixelLength = aBaseTexturePixels.Length;

        for (int p = 0; p < aPixelLength; p++)
        {
            aCopyTexturePixels[p] = aBaseTexturePixels[p];
        }

        aToCopyTexture.SetPixels(aCopyTexturePixels);
        aToCopyTexture.Apply(false);
    }
    /*
    public void DrawCircle(Texture2D originTex, ref Texture2D tex, Color color, int x, int y, int radius = 3)
    {
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                {
                    //Debug.Log(1 - (((x - u) * (x - u) + (y - v) * (y - v)) / rSquared));
                    //tex.SetPixel(u, v, CombineColorsLerp(originTex.GetPixel(u, v), color, 1.0f - (((x - u) * (x - u) + (y - v) * (y - v)) / rSquared), true));
                    tex.SetPixel(u, v, CombineColorsLerp(originTex.GetPixel(u, v), color, 0.75f, false));
                }
    }
    */
    /*
    public Color CombineColorsLerp(Color a, Color b, float value, bool isVariationIncluded)
    {
        Color result = new Color(0, 0, 0, 0);

        float rand = UnityEngine.Random.Range(-variationValue, variationValue);

        if (!isVariationIncluded)
        {
            rand = 0;
        }

        result.r = Mathf.Lerp(a.r, b.r, value + rand);
        result.g = Mathf.Lerp(a.g, b.g, value + rand);
        result.b = Mathf.Lerp(a.b, b.b, value + rand);
        result.a = Mathf.Lerp(a.a, b.a, value + rand);
        
        return result;
    }*/

    public Color CombineColors(params Color[] aColors)
    {
        Color result = new Color(0, 0, 0, 0);
        foreach (Color c in aColors)
        {
            result += c;
        }
        result /= aColors.Length;
        return result;
    }






    //TEST

    [Range(2, 512)]
    public int resolution = 256;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;

    public NoiseMethodType type;

    public float alphaValue = 0.5f;

    public Gradient coloring;

    private Texture2D texture;

    private void OnEnable()
    {
        /*if (texture == null)
        {
            texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
        FillTexture();*/
    }

    public void Init(int width, int height)
    {
        //Debug.Log(texture);
        //if (texture == null)
        //{
            //Debug.Log("Texture est null : " + texture);
            texture = new Texture2D(width, height, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        //}
        //transform.position = new Vector3(UnityEngine.Random.Range(1, 40), UnityEngine.Random.Range(1, 40), 0);
    }

    private void Update()
    {
        /*if (transform.hasChanged)
        {
            transform.hasChanged = false;
            FillTexture();
        }*/
    }

    public void FillTexture(Texture2D originTexture)
    {
        /*if (texture.width != resolution)
        {
            texture.Resize(resolution, resolution);
        }*/

        int resolutionX = originTexture.width;
        int resolutionY = originTexture.height;

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        NoiseMethod method = NoiseSiggraph.methods[(int)type][dimensions - 1];
        float stepSize = 1f / resolution;
        float stepSizeX = 1f / resolutionX;
        float stepSizeY = 1f / resolutionY;


        coloring = new Gradient();

        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = alphaValue;
        colorKey[1].color = colorRef;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = alphaValue;

        coloring.SetKeys(colorKey, alphaKey);

        //Debug.Log("Res x : " + resolutionX);
        //Debug.Log("Res y : " + resolutionY);

        for (int y = 0; y < resolutionY; y++)
        {
            //Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSizeY);
            //Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSizeY);
            for (int x = 0; x < resolutionX; x++)
            {
                //Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSizeX);
                float sample = NoiseSiggraph.Sum(method, point, frequency, octaves, lacunarity, persistence);
                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }

                colorKey[0].color = originTexture.GetPixel(x, y);
                
                coloring.SetKeys(colorKey, alphaKey);

                //texture.SetPixel(x, y, coloring.Evaluate(sample));
                originTexture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }
        //texture.Apply();
        originTexture.Apply();
    }

}

[Serializable]
public class Origin
{
    public int x;
    public int y;
    public int radius;

    public Origin(int _x, int _y, int _radius)
    {
        x = _x;
        y = _y;
        radius = _radius;
    }
}