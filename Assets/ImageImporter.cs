using UnityEngine;
using System.IO;
using TMPro;

public class ImageImporter : MonoBehaviour {
    string decalsDirectory;

    [SerializeField] private Transform image1;
    [SerializeField] private Transform image2;
    [SerializeField] private Transform image3;

    [SerializeField] private TMP_Text directoryText;
    [SerializeField] private Link openDirectoryButton;

    private void Start() {
        decalsDirectory = Path.Combine(Application.persistentDataPath, "Decals");

        if(!Directory.Exists(decalsDirectory)) {
            Directory.CreateDirectory(decalsDirectory);
        }

        //Update UI
        directoryText.text = decalsDirectory;
        openDirectoryButton.url = decalsDirectory;

        // Load all images in the folder
        string[] imagePaths = Directory.GetFiles(decalsDirectory); // Adjust file extension as needed

        int index = 0;
        foreach(string imagePath in imagePaths) {
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(1, 1); // Create a new Texture2D
            texture.LoadImage(fileData); // Load the image data into the texture

            // Create a GameObject to display the texture (example)
            GameObject imageObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(imageObject.GetComponent<MeshCollider>());
            imageObject.GetComponent<Renderer>().material.mainTexture = texture; // Apply the texture to the GameObject's material

            Debug.Log("Imported image: " + imagePath);

            //place image
            if(index == 0) {
                imageObject.transform.SetParent(image1);
                imageObject.transform.localScale = new Vector3(1.25f, 1.5f, 1f);
                imageObject.transform.localPosition = new Vector3(0, 0, 0);
                imageObject.transform.localRotation = Quaternion.identity;
            } else if(index == 1) {
                imageObject.transform.SetParent(image2);
                imageObject.transform.localScale = new Vector3(3.2f, 0.9f, 1f);
                imageObject.transform.localPosition = new Vector3(0, 0, 0);
                imageObject.transform.localRotation = Quaternion.identity;
            } else if(index == 2) {
                imageObject.transform.SetParent(image3);
                imageObject.transform.localScale = new Vector3(3.2f, 0.9f, 1f);
                imageObject.transform.localPosition = new Vector3(0, 0, 0);
                imageObject.transform.localRotation = Quaternion.identity;
            }

            index ++;
        }
    }

    private void Update() {
        
    }
}
