using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedMesh : MonoBehaviour {

    public Mesh mesh;
    public int offset = 0;
    public int numObjects = 0;

    public Matrix4x4[] transforms;
    public Material combineMaterial;
    public List<GameObject> sourceList = new List<GameObject>();
    public CombinedMeshPart[] partsArray;
    public Transform[] partsTransforms;
    public bool transformsChanged = false;

    //The number of frames in which all transforms are checked;
    //default is 1, as in update all transforms in 1 frame; depending on the number of objects and CPU performance this can take a while, like 13ms on an Android device for 3000 objects
        //by putting 2, or 3, the time per frame spent in times is 1/2 or 1/3 but the objects, if they move, their shadow will appear to move every 2 or 3 frames
    [Range(1,5)]
    public int updateFrequency = 1;
    public void Initialize()
    {
        mesh = new Mesh();
        transforms = new Matrix4x4[ CombinedShadows.maxMatrices() ];
        sourceList = new List<GameObject>();
    }
    // Use this for initialization
    void Start () {
		
	}

    void UpdateMatrices()
    {
        combineMaterial.SetMatrixArray("_meshMatrices", transforms);
    }
    int sliceIndex = 0;
    int maxSlices = 0;
    // Update is called once per frame
    void Update ()
    {
        if (partsArray != null)
        {
            int updatesPerFrame = partsArray.Length / updateFrequency;
            int offset = 0;
            if (sliceIndex > 0)
                offset = updatesPerFrame * sliceIndex;
            if (sliceIndex == updateFrequency - 1)
                updatesPerFrame += partsArray.Length % updateFrequency;

            for (int i = offset; i < offset + updatesPerFrame; i++)
            {
                CombinedMeshPart part = partsArray[i];
                if (part != null)
                {
                    if ( partsTransforms[i].hasChanged )
                        partsArray[i].UpdateManual();
                }
            }

            sliceIndex++;
            if (sliceIndex >= updateFrequency)
                sliceIndex = 0;
        }

        if (transformsChanged)
        {
            UpdateMatrices();
            transformsChanged = false;
        }
	}
}