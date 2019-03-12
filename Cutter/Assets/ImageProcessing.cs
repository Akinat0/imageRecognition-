using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageProcessing : MonoBehaviour {

    public Camera camera;
    public Material m;

    public int previewWidth;
    public int previewHeight;

    public string pathToSource = "/Users/amir/Desktop/ImageRecognition/Images/Cutted Images/3d.jpg";
    private string firstPart = "/Users/amir/Desktop/ImageRecognition/Images/Moscow4k/Pieces/";
    private string lastPart = ".jpg";

   

    // Use this for initialization
    void Start () {
        //GeneratePreview("/Users/amir/Desktop/Cutter/3d_res.jpg", pathToSource);
        GenerateSeparatedImages(20.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateSeparatedImages(float Step){

        for (float i = 0; i < 360.0f; i+=Step){
            camera.transform.Rotate(new Vector3(0, Step, 0));
            for(float j = 0; j < 360.0f; j += Step){
                camera.transform.Rotate(new Vector3(Step, 0, 0));
                GeneratePreview(pathToSource, firstPart + "_" + i + "_" + j + lastPart);
            }
        }
    }

    public void GeneratePreview( string readPath, string writePath)
    {
        RenderTexture rt;
        //Load Texture From
        Texture2D lt = new Texture2D(2, 2);
        lt.LoadImage(File.ReadAllBytes(readPath));

        //Set Material
       // m.mainTexture = lt;

        //Render
        rt = new RenderTexture(previewWidth, previewHeight, 24);
        camera.targetTexture = rt;
        camera.Render();

        //RenderTexture bt = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D virtualPhoto = new Texture2D(previewWidth, previewHeight, TextureFormat.RGB24, false);


        //CopyTo texture2d
        virtualPhoto.ReadPixels(new Rect(0, 0, previewWidth, previewHeight), 0, 0);
        RenderTexture.active = null;
        //RenderTexture.active = bt;

        camera.targetTexture = null;

        //Write Texture
        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();


        File.WriteAllBytes(writePath, bytes);

        //Make Memory Free
        DestroyImmediate(lt, true);
        DestroyImmediate(virtualPhoto, true);
        DestroyImmediate(rt, true);

    }

}
