using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MassiveDrawer : eObject
{
    struct propDataInstance
    {
        public Matrix4x4 modelWorld;
        public Vector3 dirc;
        //public Vector3 tang;
        public bool visible;
    };

    public Terrain m_targetTerrain;
    public Texture randomMap, heightMap, denseMap;
    public ComputeShader m_computeShader;
    public BoxCollider m_boundColl;
    public Transform playerTransform;
    public Camera m_orthoCamera;
    public float yMax;

    ComputeBuffer m_argsBuffer;
    ComputeBuffer[] m_propBuffer = new ComputeBuffer[5];
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    propDataInstance[] bufferData = new propDataInstance[128];

    public Transform cameraTransform;

    int nThreadsShader = 256, kernelID;

    [Space]

    public Material RefMaterial;
    public Texture[] grassTexture;
    public Mesh[] instanceMesh;
    public Material[] instanceMaterial = new Material[5];

    public int[] instanceCount;
    public float[] m_squareSize;
    public float[] m_stepLenth;

    // Use this for initialization
    void Start ()
    {
        if(QualitySettings.GetQualityLevel() < 3)
        {
            gameObject.SetActive(false);
        }

        SetState("IgnoreProjectile");

        //Create Materials
        for (int i = 0; i < instanceMesh.Length; i++)
        {
            instanceMaterial[i] = new Material(RefMaterial);
            instanceMaterial[i].mainTexture = grassTexture[i];
            instanceMaterial[i].SetFloat("_Top", yMax);
        }

        for (int i = 0; i < instanceMesh.Length; i++)
        {
            m_propBuffer[i] = new ComputeBuffer(instanceCount[i], 20 * sizeof(float));
        }

        m_argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);

#if UNITY_STANDALONE_OSX
        kernelID = m_computeShader.FindKernel("CSMain");
#else	
		kernelID = m_computeShader.FindKernel("CSMain");
#endif

		m_computeShader.SetTexture(kernelID, "RandomImg", randomMap);
		m_computeShader.SetTexture(kernelID, "HeightMap", heightMap);

        for (int i = 0; i < instanceMesh.Length; i++)
        {
            instanceMaterial[i].SetBuffer("propBuffer", m_propBuffer[i]);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateBuffers();

        ShadowCastingMode castShadows = ShadowCastingMode.Off;

        //LODs
        for(int i = 0; i < instanceMesh.Length; i++)
        {
            UpdateShaderParam(m_squareSize[i], (i == 0 ? 0 : (m_squareSize[i - 1] - 5)), m_stepLenth[i]);
            m_computeShader.SetBuffer(kernelID, "buffer", m_propBuffer[i]);
            m_computeShader.Dispatch(kernelID, instanceCount[i] / nThreadsShader, 1, 1);

            uint numIndices = (instanceMesh[i] != null) ? (uint)instanceMesh[i].GetIndexCount(0) : 0;
            args[0] = numIndices;
            args[1] = (uint)instanceCount[i];
            m_argsBuffer.SetData(args);

            Graphics.DrawMeshInstancedIndirect(instanceMesh[i], 0, instanceMaterial[i], m_boundColl.bounds, m_argsBuffer, 0, null, castShadows, true);
        }
	}

    void UpdateShaderParam(float squareSize, float ignoreSize, float stepLenth)
    {
        m_computeShader.SetFloat("stepLenth", stepLenth);
        m_computeShader.SetFloat("stepCountf", (float)((int)(squareSize / stepLenth)));
        m_computeShader.SetInt("stepCount", (int)(squareSize / stepLenth));
        m_computeShader.SetFloat("squareSize", squareSize);
        m_computeShader.SetFloat("halfSquareSize", squareSize / 2.0f);
        m_computeShader.SetFloat("halfIgnoreSize", ignoreSize / 2.0f);        
        m_computeShader.SetFloat("rndRange", stepLenth * 1.2f);
        m_computeShader.SetFloat("halfRndRange", stepLenth * 0.6f);
    }

    void UpdateBuffers()
    {
        //Update texture
        m_computeShader.SetTexture(kernelID, "DenseMap", denseMap);

        //Update props buffer
        m_computeShader.SetFloat("startTime", Time.time);
        m_computeShader.SetVector("playerPos", new Vector4(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z, 1.0f));
        m_computeShader.SetVector("gridCameraPos", new Vector4(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z, 1.0f));
        m_computeShader.SetVector("cameraDirc", cameraTransform.rotation * new Vector4(0, 0, 1, 1));

        m_computeShader.SetVector("orthoCameraPos", new Vector4(m_orthoCamera.transform.position.x, m_orthoCamera.transform.position.y, m_orthoCamera.transform.position.z, 1.0f));
        m_computeShader.SetFloat("orthoCameraFar", m_orthoCamera.farClipPlane);
        m_computeShader.SetFloat("orthoCameraNear", m_orthoCamera.nearClipPlane);
        m_computeShader.SetFloat("orthoCameraSize", m_orthoCamera.orthographicSize);

        //m_propBuffer.GetData(bufferData);
        //Debug.Log(bufferData[0].modelWorld);
    }

    void OnDestroy()
    {
        for (int i = 0; i < instanceMesh.Length; i++)
        {
            m_propBuffer[i].Release();
        }
        m_argsBuffer.Release();
    }
}
