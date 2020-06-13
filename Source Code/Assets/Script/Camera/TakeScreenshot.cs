using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class TakeScreenshot : MonoBehaviour
{
    public string prefix = "Screenshot";
    public enum method
    {
        captureScreenshotPng, ReadPixelsPng,
        ReadPixelsJpg
    };

    public method captMethod = method.captureScreenshotPng;
    public int captureScreenshotScale = 1;
    [Range(0, 100)]
    public int jpgQuality = 75;
    private Texture2D texture;
    private int sw;
    private int sh;
    private Rect sRect;
    string date;

    void Start()
    {
        sw = Screen.width;
        sh = Screen.height;
        sRect = new Rect(0, 0, sw, sh);
    }

    public void TakeShot()
    {
        date = System.DateTime.Now.ToString("_d-MMM-yyyy-HH-mm-ss-f");
        if (captMethod == method.captureScreenshotPng)
        {
            ScreenCapture.CaptureScreenshot(prefix + date + ".png", captureScreenshotScale);
        }
        else
        {
            StartCoroutine(ReadPixels());
        }
    }

    IEnumerator ReadPixels()
    {
        yield return new WaitForEndOfFrame();
        byte[] bytes;
        texture = new Texture2D(sw, sh, TextureFormat.RGB24, false);
        texture.ReadPixels(sRect, 0, 0);
        texture.Apply();
        if (captMethod == method.ReadPixelsJpg)
        {
            bytes = texture.EncodeToJPG(jpgQuality);
            WriteBytesToFile(bytes, ".jpg");
        }
        else if (captMethod == method.ReadPixelsPng)
        {
            bytes = texture.EncodeToPNG();
            WriteBytesToFile(bytes, ".png");
        }
    }

    private void WriteBytesToFile(byte[] bytes, string format)
    {
        Destroy(texture);
        File.WriteAllBytes(Application.dataPath + "/../" + prefix + date
        + format, bytes);
    }
}
