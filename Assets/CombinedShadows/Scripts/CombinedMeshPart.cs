using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedMeshPart : MonoBehaviour {

    public CombinedMesh combinedMesh;

    public int index = -1;//index in the matrix array, you're not supposed to ever change this
    public int combineIndex = -1;//index into the array of components to optimize Update calls
    //public int updateFrequency = 1;//the number of frames at which transform updates are checked; this should be >1 for static objects
                                    //fast moving objects should have 1
    int framesSinceLastUpdate = 1;//must be equal to updateFrequency so its position gets initialized
    // Use this for initialization
    void Start ()
    {
        transform.hasChanged = false;
	}

    // Update is called once per frame
    public void UpdateManual ()
    {
        //framesSinceLastUpdate++;
        //if (framesSinceLastUpdate >= updateFrequency)
        {
            //framesSinceLastUpdate = 0;

            //if (transform.hasChanged && combinedMesh != null)
            {
                combinedMesh.transforms[index] = this.transform.localToWorldMatrix;
                combinedMesh.transformsChanged = true;
                transform.hasChanged = false;
            }
        }
    }
    void EnableDisable( bool enable )
    {
        if (combinedMesh == null)
            return;

        if (enable)
        {
            combinedMesh.transforms[index] = this.transform.localToWorldMatrix;
        }
        else
        {
            combinedMesh.transforms[index] = Matrix4x4.zero;
        }

        combinedMesh.transformsChanged = true;
    }
    private void OnEnable()
    {
        EnableDisable( true );
    }
    private void OnDisable()
    {
        EnableDisable(false);
    }
    private void OnDestroy()
    {
        EnableDisable(false);
        if (combineIndex >= 0 && combineIndex < combinedMesh.partsArray.Length && combinedMesh.partsArray != null )
            combinedMesh.partsArray[ combineIndex ] = null;
    }
}
