using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedShadows : MonoBehaviour {
    public Shader combinedMeshShader;
    public List<GameObject> solidObjectsList = new List<GameObject>();
    public List<GameObject> alphaTestedObjectsList = new List<GameObject>();
    #if UNITY_2017_3_OR_NEWER
        public UnityEngine.Rendering.IndexFormat combinedMeshIndexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    #endif
    // Use this for initialization

        //This depends solely on the shader platform
    public static int maxMatrices()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D11 ||
            SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D12 ||
            SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.PlayStation4 ||
            SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.XboxOne)

            return 1022;
        else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Vulkan)
            return 980;
        else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D9)
            return 60;
        else//assuming GL
            return 200;
    }
    void GatherObjects()
    {
        MeshRenderer[] meshRenderers = Object.FindObjectsOfType<MeshRenderer>();

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            MeshRenderer mr = meshRenderers[i];
            Material mat = mr.sharedMaterial;
            if (mr.enabled && !mr.gameObject.isStatic)
            {
                //If you have custom shaders that have leaves, this is where you need to separate solids from AlphaTested
                if ( mat.shader && (mat.shader.name.Contains("SpeedTree") || mat.shader.name.Contains("Hidden/Nature/Tree") ))
                    alphaTestedObjectsList.Add(mr.gameObject);
                else
                    solidObjectsList.Add(mr.gameObject);
            }
        }
    }
    int GetNumCombineInstances(List<GameObject> meshesForShadows)
    {
        int numInstances = 0;
        for (int i = 0; i < meshesForShadows.Count; i++)
        {
            GameObject obj = meshesForShadows[i];
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            MeshFilter mf = obj.GetComponent<MeshFilter>();

            numInstances += mf.sharedMesh.subMeshCount;
        }

        return numInstances;
    }
    void ClampList( ref List<GameObject> meshesForShadows, int max )
    {
        if (meshesForShadows.Count <= max)
            return;

        meshesForShadows.RemoveRange( max, meshesForShadows.Count - max );
    }
    int[] GetSubmeshCounts(List<GameObject> meshesForShadows)
    {
        int[] counts = new int[meshesForShadows.Count];
        for (int i = 0; i < meshesForShadows.Count; i++)
        {
            GameObject obj = meshesForShadows[i];
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            MeshFilter mf = obj.GetComponent<MeshFilter>();
            int submeshes = mf.sharedMesh.subMeshCount;

            counts[i] = submeshes;
        }

        return counts;
    }
    void SplitInstances( CombineInstance[] initial, out CombineInstance[] forCombine, out CombineInstance[] remaining
         #if UNITY_2017_3_OR_NEWER
            , UnityEngine.Rendering.IndexFormat indexFormat
        #endif
        )
    {
        int vertexCount = 0;
        for(int i =0; i <initial.Length; i++)
        {
            Mesh mesh = initial[i].mesh;
            int meshVertices = GetSubMeshVertexCount2(mesh, initial[i].subMeshIndex);
            vertexCount += meshVertices;

            bool isUint16Format = true;
            #if UNITY_2017_3_OR_NEWER
                isUint16Format = (indexFormat == UnityEngine.Rendering.IndexFormat.UInt16);
            #endif
            if ( (isUint16Format  && vertexCount > 65500) || i >= maxMatrices() )
            {
                forCombine = new CombineInstance[ i ];
                remaining = new CombineInstance[ initial.Length - i ];

                for (int j = 0; j < i; j++)
                {
                    forCombine[j] = initial[j];
                }
                for (int j = 0; j < remaining.Length; j++)
                {
                    remaining[j] = initial[i + j ];
                }

                return;
            }
        }

        forCombine = initial;
        remaining = new CombineInstance[ 0 ];
    }
    
    List<CombinedMesh> CombineMeshes(CombineInstance[] instances, bool mergeSubMeshes, bool useMatrices, GameObject Parent, bool alphaTested
#if UNITY_2017_3_OR_NEWER
        ,UnityEngine.Rendering.IndexFormat indexFormat
#endif
        )
    {
        List<CombinedMesh> combinedMeshesList = new List<CombinedMesh>();
        
        int objectsOffset = 0;
        int index = 0;
        CombineInstance[] forCombine;
        CombineInstance[] remaining = new CombineInstance[1];
        while (remaining.Length > 0)
        {
            SplitInstances(instances, out forCombine, out remaining
#if UNITY_2017_3_OR_NEWER
                , indexFormat
#endif
                );

            string name = "Combined Mesh For Shadows " + index;
            if (alphaTested)
                name += " - AlphaTest";
            else
                name += " - Solid";
            GameObject combinedObject = new GameObject( name );
            if (Parent != null)
                combinedObject.transform.SetParent( Parent.transform );

            CombinedMesh newCombinedMesh = combinedObject.AddComponent<CombinedMesh>();
            newCombinedMesh.Initialize();
            newCombinedMesh.offset = objectsOffset;
            newCombinedMesh.numObjects = forCombine.Length;

            combinedMeshesList.Add(newCombinedMesh);

#if UNITY_2017_3_OR_NEWER
                newCombinedMesh.mesh.indexFormat = indexFormat;
#endif
            newCombinedMesh.mesh.CombineMeshes(forCombine, mergeSubMeshes, useMatrices);

            instances = remaining;
            objectsOffset += forCombine.Length;
            index++;
        }        

        return combinedMeshesList;
    }
    int GetSubMeshVertexCount2(Mesh mesh, int submesh)
    {
        bool submeshHasAllVertices = true;
        #if UNITY_2018_1_OR_NEWER
            submeshHasAllVertices = false;
        #endif

        if (submeshHasAllVertices)
        {
            return mesh.vertexCount;
        }
        else
        {
            int[] triangles = mesh.GetTriangles(submesh);
            int min = mesh.vertexCount;
            int max = 0;
            for (int i = 0; i < triangles.Length; i++)
            {
                int index = triangles[i];
                min = Mathf.Min(index, min);
                max = Mathf.Max(index, max);
            }

            int count = max - min + 1;
            return count;
        }
    }    
    void CombineObjects(List<GameObject> meshesForShadows, bool useMatrices, GameObject Parent, bool isAlphaTested )
    {
        if (meshesForShadows.Count == 0)
            return;
        
        int maxInstances = GetNumCombineInstances(meshesForShadows);

        CombineInstance[] instances = new CombineInstance[ maxInstances ];
        int[] vertexCounts = new int[maxInstances];
        int[] transformIndices = new int[maxInstances];
        int maxVerts = 0;
        Bounds[] allBounds = new Bounds[meshesForShadows.Count];
        MeshRenderer[] meshRenderers = new MeshRenderer[meshesForShadows.Count];
        int currentInstace = 0;
        for (int i=0; i< meshesForShadows.Count; i++)
        {
            GameObject obj = meshesForShadows[i];
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            MeshFilter mf = obj.GetComponent<MeshFilter>();
            meshRenderers[i] = mr;

            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            
            int submeshes = mf.sharedMesh.subMeshCount;
            allBounds[i] = mr.bounds;

            for (int submesh = 0; submesh < submeshes; submesh++ )
            {
                instances[currentInstace].mesh = mf.sharedMesh;
                instances[currentInstace].transform = obj.transform.localToWorldMatrix;
                instances[currentInstace].subMeshIndex = submesh;
                vertexCounts[currentInstace] = GetSubMeshVertexCount2( mf.sharedMesh, submesh );
                transformIndices[currentInstace] = i;
                maxVerts += vertexCounts[currentInstace];

                currentInstace++;
            }
        }

        List<CombinedMesh> combinedMeshList = CombineMeshes(instances, true, useMatrices, Parent, isAlphaTested
#if UNITY_2017_3_OR_NEWER
            , combinedMeshIndexFormat
#endif
            );

        if (!useMatrices)
        {
            for (int i = 0; i < combinedMeshList.Count; i++)
            {
                CombinedMesh combinedMesh = combinedMeshList[i];
                Mesh mesh = combinedMesh.mesh;
                int vertexIndex = 0;
                Vector2[] uv2 = mesh.uv2;
                if (uv2 == null || uv2.Length < mesh.vertexCount)
                    uv2 = new Vector2[mesh.vertexCount];

                Bounds combinedBounds = new Bounds();
                //Assign object indices
                int transformOffset = transformIndices[combinedMesh.offset];

                for (int j = combinedMesh.offset; j < combinedMesh.offset + combinedMesh.numObjects; j++)
                {                    
                    int transformID = transformIndices[j];
                    int transformIndex = transformID - transformOffset;
                    GameObject gameObject = meshesForShadows[transformID];

                    CombinedMeshPart comp = gameObject.GetComponent<CombinedMeshPart>();
                    if (comp == null || comp.combinedMesh != combinedMesh)
                        comp = gameObject.AddComponent<CombinedMeshPart>();

                    comp.combinedMesh = combinedMesh;
                    comp.index = transformIndex;

                    if (!combinedMesh.sourceList.Find(obj => obj == gameObject))
                        combinedMesh.sourceList.Add( gameObject );

                    Bounds sourceBounds = meshRenderers[transformID].bounds;
                    combinedBounds.Encapsulate( sourceBounds );

                    for (int u = 0; u < vertexCounts[j]; u++)
                    {
                        uv2[vertexIndex].x = transformIndex;
                        vertexIndex++;
                    }
                    vertexIndex = vertexIndex;
                }

                mesh.uv2 = uv2;
                mesh.bounds = combinedBounds;
                mesh.UploadMeshData(false);
            }
        }

        for (int i = 0; i < combinedMeshList.Count; i++)
        {
            CombinedMesh combinedMesh = combinedMeshList[i];
            combinedMesh.combineMaterial = new Material( combinedMeshShader );

            if ( isAlphaTested )
            {
                combinedMesh.combineMaterial.EnableKeyword("ENABLE_ALPHA_TEST");
                if (combinedMesh.sourceList.Count > 0)
                {
                    GameObject source = combinedMesh.sourceList[0];
                    MeshRenderer mr = source.GetComponent<MeshRenderer>();
                    Material referenceMaterial = mr.sharedMaterials[mr.sharedMaterials.Length - 1];

                    //if you only use custom shaders, you could choose a different texture to use for alpha testing
                    Texture alphaTestTexture = referenceMaterial.GetTexture("_MainTex");
                    if (alphaTestTexture != null)
                        combinedMesh.combineMaterial.SetTexture( "_MainTex", alphaTestTexture);
                }
            }

            Mesh mesh = combinedMesh.mesh;

            GameObject combinedObject = combinedMesh.gameObject;

            combinedObject.AddComponent<MeshFilter>().mesh = mesh;            
            MeshRenderer combinedMR = combinedObject.AddComponent<MeshRenderer>();

            combinedMesh.partsArray = new CombinedMeshPart[combinedMesh.sourceList.Count];
            combinedMesh.partsTransforms = new Transform[combinedMesh.sourceList.Count];
            for (int j = 0; j < combinedMesh.sourceList.Count; j++)
            {
                GameObject source = combinedMesh.sourceList[j];
                CombinedMeshPart part = source.GetComponent<CombinedMeshPart>();
                part.transform.hasChanged = true;
                part.UpdateManual();
                combinedMesh.partsArray[ j ] = part;
                combinedMesh.partsTransforms[ j ] = source.transform;
                part.combineIndex = j;
            }

            combinedMesh.transformsChanged = true;

            combinedMR.sharedMaterial = combinedMesh.combineMaterial;
            combinedMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            if (Parent != null )
                combinedObject.transform.SetParent(Parent.transform);
        }
    }
    void Start ()
    {

    }
    void OnEnable()
    {
        StartCoroutine( EnableCoroutine() );
    }
    IEnumerator EnableCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (solidObjectsList.Count == 0 && alphaTestedObjectsList.Count == 0)
        {
            GatherObjects();
        }

        if (combinedMeshShader == null)
        {
            combinedMeshShader = Shader.Find("RE/CombineMesh");
            if (combinedMeshShader == null)
            {
                Debug.LogError("Could not find shader RE/CombineMesh, make sure you attach it to this script or put it in the always included shaders list !");
            }
        }
        else
        {
            CombineObjects(solidObjectsList, false, null, false);
            CombineObjects(alphaTestedObjectsList, false, null, true);
        }
    }
    void OnDisable()
    {
        CombinedMesh[] combinedMeshes = Object.FindObjectsOfType<CombinedMesh>();
        for (int i = 0; i < combinedMeshes.Length; i++)
        {
            CombinedMesh cm = combinedMeshes[i];
            for (int u = 0; u < cm.sourceList.Count; u++)
            {
                GameObject source = cm.sourceList[u];
                if (source != null)
                {
                    MeshRenderer mr = source.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                }
            }
        }
        for (int i = 0; i < combinedMeshes.Length; i++)
        {
            CombinedMesh cm = combinedMeshes[i];
            Destroy(cm.gameObject);
        }

            CombinedMeshPart[] combinedMeshParts = Object.FindObjectsOfType<CombinedMeshPart>();
        for (int i = 0; i < combinedMeshParts.Length; i++)
        {
            Destroy(combinedMeshParts[i]);
        }
    }
        // Update is called once per frame
    void Update ()
    {
		
	}    
}
