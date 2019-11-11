using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParralax : MonoBehaviour
{
    //Variable Declaration, note Public keyword used to allow fine tuning in editor as code is running.
    public Transform cam;
    public int depth;
    public float parralax;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    //Move quad to camera background position, then offset texture proportional to parralax tuning variable
    void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;
        transform.position = new Vector3(cam.position.x, cam.position.y, depth);
        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x / parralax;
        offset.y = transform.position.y / transform.localScale.y / parralax;

        mat.mainTextureOffset = offset;


    }
}
