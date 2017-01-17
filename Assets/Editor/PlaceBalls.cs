//C# Example
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class PlaceBalls : EditorWindow
{
    public Terrain targetTerrain = null;
    public float ballCount = 15.0f;
    public AnimationCurve atCurveTest = AnimationCurve.Linear(0, 0, 1, 1);

    float[,,] terrainAlphaMaps;
    int terrainAlphaMapCount;

    int _max_vertices = 65530;

    bool usingTextureMask = false;
    List<Texture2D> textureList = new List<Texture2D>();

    // Add menu item named "My Window" to the Window menu
    [MenuItem("SK_Assets/[DEBUG]Place Balls")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        PlaceBalls window = (PlaceBalls)EditorWindow.GetWindow(typeof(PlaceBalls));
    }

    void AddBallsAndCombine()
    {
        float t_width = targetTerrain.terrainData.heightmapScale.x * targetTerrain.terrainData.heightmapResolution;
        float t_lenth = targetTerrain.terrainData.heightmapScale.z * targetTerrain.terrainData.heightmapResolution;

        Vector3 position = targetTerrain.transform.position;
        Vector3 orgPosition = position;

        GameObject container = new GameObject();
        container.transform.position = targetTerrain.GetPosition();
        container.name = "Balls";

        Debug.Log(t_width);

        for (position.x = orgPosition.x; position.x < orgPosition.x + t_width; position.x += (t_width / ballCount))
        {
            for (position.z = orgPosition.z; position.z < orgPosition.z + t_lenth; position.z += (t_lenth / ballCount))
            {
                GameObject ball = (GameObject)Instantiate(Resources.Load("_debug_Ball"), 
                    new Vector3(
                        position.x,
                        targetTerrain.SampleHeight(position) + orgPosition.y,
                        position.z) - container.transform.position,
                    Quaternion.identity);
                ball.transform.parent = container.transform;
                //ball.name = "ball";
            }
        }

        //Combine
        CombineGameObject(container);
    }

    void CombineGameObject(GameObject container)
    {
        MeshRenderer[] meshRenders = container.GetComponentsInChildren<MeshRenderer>();
        Material[] mats = new Material[1];
        mats[0] = meshRenders[0].sharedMaterial;
        MeshFilter[] meshFilters = container.GetComponentsInChildren<MeshFilter>();

        int vertTotal = 0, indexTotal = 0;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            vertTotal += meshFilters[i].sharedMesh.vertexCount;
            indexTotal += meshFilters[i].sharedMesh.triangles.Length;
        }

        int vertCount = 0, indexCount = 0;
        Vector3[] vert = new Vector3[vertTotal];
        Vector2[] uv1 = new Vector2[vertTotal];
        Vector2[] uv2 = new Vector2[vertTotal];
        Vector4[] tangent = new Vector4[vertTotal];
        Vector3[] normal = new Vector3[vertTotal];
        int[] index = new int[indexTotal];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            int j, tmp = vertCount;
            
            meshFilters[i].sharedMesh.uv.CopyTo(uv1, vertCount);
            meshFilters[i].sharedMesh.uv2.CopyTo(uv2, vertCount);

            for (j = 0; j < meshFilters[i].sharedMesh.vertexCount; j++)
            {
                //Why local to world matrix cannot work???????
                //DAMN ***
                //(well so I used pos + Q * local orz)

                Vector3 tmpVec3 = meshFilters[i].sharedMesh.vertices[j];
                tmpVec3.Scale(meshFilters[i].transform.lossyScale);

                vert[tmp] = meshFilters[i].transform.position + meshFilters[i].transform.rotation * tmpVec3;
                normal[tmp] = meshFilters[i].transform.rotation * meshFilters[i].sharedMesh.normals[j];
                tangent[tmp] = meshFilters[i].transform.rotation * meshFilters[i].sharedMesh.tangents[j];
                tmp++;
            }

            //indices
            for (j = 0; j < meshFilters[i].sharedMesh.triangles.Length; j++)
            {
                index[indexCount] = meshFilters[i].sharedMesh.triangles[j] + vertCount;
                indexCount++;
            }

            vertCount = tmp;

            DestroyImmediate(meshFilters[i].gameObject);
        }

        MeshRenderer mr = container.AddComponent<MeshRenderer>();
        MeshFilter mf = container.AddComponent<MeshFilter>();
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.vertices = vert;
        mf.sharedMesh.uv = uv1;
        mf.sharedMesh.uv2 = uv2;
        mf.sharedMesh.tangents = tangent;
        mf.sharedMesh.normals = normal;
        mf.sharedMesh.triangles = index;
        mr.sharedMaterials = mats;
    }

    void OnGUI()
    {
        targetTerrain = (Terrain)EditorGUILayout.ObjectField("Target Terrain", targetTerrain, typeof(Terrain), true);

        if(targetTerrain)
        {
            if (GUILayout.Button("Add a...a \"ball\""))
            {
                AddBallsAndCombine();

                //CombineGameObject(GameObject.Find("_debug_GrassQuad"));
            }
        }

        //EcoSystem
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("EcoSystem_Grass");
        atCurveTest = EditorGUILayout.CurveField("Altitude-dep Curve", atCurveTest);

        usingTextureMask = EditorGUILayout.BeginToggleGroup("Use Texture mask", usingTextureMask);
            for( int mIndex = 0; mIndex < textureList.Count; mIndex++ )
            {
                EditorGUILayout.BeginHorizontal();
                textureList[mIndex] = (Texture2D)EditorGUILayout.ObjectField(textureList[mIndex], typeof(Texture2D), true);
                if(GUILayout.Button("Remove"))
                {
                    textureList.RemoveAt(mIndex);
                    mIndex--;
                }
                EditorGUILayout.EndHorizontal();
            }
            if(usingTextureMask)
            {
                Texture2D tmp = null;
                tmp = (Texture2D)EditorGUILayout.ObjectField(tmp, typeof(Texture2D), true);

                if(tmp != null)
                {
                    textureList.Add(tmp);
                }
            }
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();

        if (GUILayout.Button("Populate"))
        {
            PopulateTestEcoSystem("_debug_Grass", atCurveTest);
        }
    }

    Vector3 ConvertToSplatMapCoordinate(Vector3 pos)
    {
        Vector3 vecRet = new Vector3();
        Vector3 terPosition = targetTerrain.transform.position;
        vecRet.x = ((pos.x - terPosition.x) / targetTerrain.terrainData.size.x) * targetTerrain.terrainData.alphamapWidth;
        vecRet.z = ((pos.z - terPosition.z) / targetTerrain.terrainData.size.z) * targetTerrain.terrainData.alphamapHeight;
        return vecRet;
    }

    public void PopulateTestEcoSystem(string prefabName, AnimationCurve atCurveTest)
    {
        //get material information
        terrainAlphaMaps = targetTerrain.terrainData.GetAlphamaps(0, 0, targetTerrain.terrainData.alphamapWidth, targetTerrain.terrainData.alphamapHeight);
        terrainAlphaMapCount = terrainAlphaMaps.Length / (targetTerrain.terrainData.alphamapWidth * targetTerrain.terrainData.alphamapHeight);

        //simple way.
        float baseStep = 0.35f;

        float totalAtitude = targetTerrain.terrainData.heightmapScale.y;
        float t_width = targetTerrain.terrainData.heightmapScale.x * targetTerrain.terrainData.heightmapResolution;
        float t_lenth = targetTerrain.terrainData.heightmapScale.z * targetTerrain.terrainData.heightmapResolution;

        int _test_verticesCount = 0;

        Vector3 position = targetTerrain.transform.position;
        Vector3 orgPosition = position;

        Vector3 instancePos = Vector3.zero, instanceScale = Vector3.zero;
        Quaternion instanceRot = Quaternion.identity;

        GameObject container = new GameObject();
        container.transform.position = targetTerrain.GetPosition();
        container.name = "_test_EcoSystem";

        for (position.x = orgPosition.x; position.x <= orgPosition.x + t_width; position.x += baseStep)
        {
            for(position.z = orgPosition.z; position.z <= orgPosition.z + t_lenth; position.z += baseStep)
            {
                //Random position & rotation & scaling
                instancePos.x = position.x + UnityEngine.Random.Range(-baseStep / 2.0f, baseStep / 2.0f);
                instancePos.z = position.z + UnityEngine.Random.Range(-baseStep / 2.0f, baseStep / 2.0f);
                instancePos.y = position.y + targetTerrain.SampleHeight(instancePos);

                instanceScale = Vector3.one * UnityEngine.Random.Range(1.2f, 1.6f);

                instanceRot = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);

                Vector3 uvCoord = ConvertToSplatMapCoordinate(instancePos);
                if( (int)uvCoord.x < 0 || (int)uvCoord.x > (targetTerrain.terrainData.alphamapWidth - 1) ||
                    (int)uvCoord.z < 0 || (int)uvCoord.z > (targetTerrain.terrainData.alphamapHeight - 1))
                {
                    //out of terrain
                    continue;
                }

                //total multiplyer on density
                float totalDenMul = atCurveTest.Evaluate(targetTerrain.SampleHeight(position) / totalAtitude);

                //textureMasking
                if(usingTextureMask)
                {
                    float maxRate = 0.0f;

                    foreach (var tex in textureList)
                    {
                        for(int tmp = 0; tmp < terrainAlphaMapCount; tmp++)
                        {
                            if(targetTerrain.terrainData.splatPrototypes[tmp].texture.Equals(tex))
                            {
                                float rate = terrainAlphaMaps[(int)uvCoord.z, (int)uvCoord.x, tmp];

                                if(rate > maxRate)
                                {
                                    maxRate = rate;
                                }

                                break;
                            }
                        }
                    }

                    totalDenMul *= maxRate;
                }

                //Create Instance
                if (UnityEngine.Random.Range(0, 1.0f) < totalDenMul)
                {
                    GameObject instance = (GameObject)Instantiate(Resources.Load(prefabName),
                        instancePos - container.transform.position,
                        instanceRot);
                    instance.transform.localScale = instanceScale;

                    if(_test_verticesCount + instance.GetComponent<MeshFilter>().sharedMesh.vertexCount > _max_vertices)
                    {
                        CombineGameObject(container);

                        container = new GameObject();
                        container.transform.position = targetTerrain.GetPosition();
                        container.name = "_test_EcoSystem";

                        _test_verticesCount = 0;
                    }

                    _test_verticesCount += instance.GetComponent<MeshFilter>().sharedMesh.vertexCount;

                    instance.transform.parent = container.transform;
                }
            }
        }

        CombineGameObject(container);
    }
}