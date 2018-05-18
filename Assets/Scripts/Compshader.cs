using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compshader : MonoBehaviour {
    public move bot;
    public RenderTexture tex;
    public RenderTexture buf;
    public int texSize = 256;
    public ComputeShader shader;
    public ComputeShader roomShader;
    [Range(0.0f, 1024.0f)]
    public float div_x;
    [Range(0.0f, 1024.0f)]
    public float div_y;
    public float speed = 1;
    int dir;
    public float strength;
    public float size;
    // Use this for initialization
    void Awake () {
        tex = new RenderTexture(texSize, texSize, 24);
        tex.enableRandomWrite = true;
        tex.Create();

        buf = new RenderTexture(texSize, texSize, 24);
        buf.enableRandomWrite = true;
        buf.Create();

        ClearShader();
    }
	
	// Update is called once per frame
	void Update() { 
        
        if (Input.GetMouseButtonDown(1))
            ClearShader();
        else {
            //if (Input.GetMouseButtonDown(0))
            dir++;
            if (dir >= 4)
                dir = 0;
            
            //rayScript.hits[dir].x;
            //rayScript.hits[dir].z;
            //Vector2 forward = new Vector2(bot.forward.transform.position.x, bot.forward.transform.position.z);
            //Vector2 back = new Vector2(bot.back.transform.position.x, bot.back.transform.position.z);
            //Vector2 left = new Vector2(bot.left.transform.position.x, bot.left.transform.position.z);
            //Vector2 right = new Vector2(bot.right.transform.position.x, bot.right.transform.position.z);

            


            //RunShader(div_x, div_y);

            //RunShader4(forward, back, left, right);





            //div_x += Input.GetAxis("Horizontal") * speed;
            //div_y += Input.GetAxis("Vertical") * speed;
        }
        

    }




    private void OnGUI()
    {
        int s = 256;
        int w = Screen.width - (s/2);
        int h = Screen.height - (s / 2);
       
        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), tex);

    }
    public void RunShader(float xdiv, float ydiv)
    {
        int kernelHandle = shader.FindKernel("MakeHill");

        shader.SetFloat("strength", 1.0f/strength);
        shader.SetFloat("size", 1.0f / size);
        shader.SetFloat("divx", xdiv);
        shader.SetFloat("divy", ydiv);
        shader.SetFloat("pointx", (texSize / 256) * xdiv);
        shader.SetFloat("pointy", (texSize/256)*ydiv);
        shader.SetTexture(kernelHandle, "res", tex);
        shader.SetTexture(kernelHandle, "buf", buf);
        shader.Dispatch(kernelHandle, texSize / 4, texSize / 4, 1);

        Graphics.Blit(tex, buf);
    }


    public void RunShader4(Vector2 forward, Vector2 back, Vector2 left, Vector2 right, float fwdStr, float bckStr, float lftStr, float rghStr)
    {
        int kernelHandle = shader.FindKernel("MakeHill4");
        shader.SetFloat("fwd_str", fwdStr);
        shader.SetFloat("bck_str", fwdStr);
        shader.SetFloat("lft_str", fwdStr);
        shader.SetFloat("rgh_str", fwdStr);
        shader.SetFloat("strength", 1.0f / strength);
        shader.SetFloat("size", 1.0f / size);
        //shader.SetFloat("divx", xdiv);
        //shader.SetFloat("divy", ydiv);
        shader.SetFloat("fwd_x", (texSize / 256) * forward.x);
        shader.SetFloat("fwd_y", (texSize / 256) * forward.y);
        shader.SetFloat("bck_x", (texSize / 256) * back.x);
        shader.SetFloat("bck_y", (texSize / 256) * back.y);
        shader.SetFloat("lft_x", (texSize / 256) * left.x);
        shader.SetFloat("lft_y", (texSize / 256) * left.y);
        shader.SetFloat("rgh_x", (texSize / 256) * right.x);
        shader.SetFloat("rgh_y", (texSize / 256) * right.y);
        shader.SetTexture(kernelHandle, "res", tex);
        shader.SetTexture(kernelHandle, "buf", buf);
        shader.Dispatch(kernelHandle, texSize / 4, texSize / 4, 1);
        Graphics.Blit(tex, buf);
    }


    void ClearShader()
    {
        int kernelHandle = shader.FindKernel("Clear");

        
        shader.SetTexture(kernelHandle, "res", tex);
        shader.SetTexture(kernelHandle, "buf", buf);
        shader.Dispatch(kernelHandle, texSize / 4, texSize / 4, 1);

        Graphics.Blit(tex, buf);
        
    }
    public Vector3[] MapMesh(Vector3[] verts, int size)
    {
        ComputeBuffer calcMesh = new ComputeBuffer(verts.Length, 3*4);
        calcMesh.SetData(verts);
        int kernelHandle = roomShader.FindKernel("RecalculateVerts");
        roomShader.SetInt("texSize", 1024);
        roomShader.SetInt("meshSize", 256);
        roomShader.SetTexture(kernelHandle, "heightMap",buf);
        roomShader.SetBuffer(kernelHandle, "mesh", calcMesh);
        roomShader.Dispatch(kernelHandle, 256*256/16, 1, 1);
        calcMesh.GetData(verts);
        calcMesh.Dispose();
        return verts;

    }
}
