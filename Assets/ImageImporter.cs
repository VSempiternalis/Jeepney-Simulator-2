using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class ImageImporter : MonoBehaviour {
    string decalsDirectory;

    // [SerializeField] private Transform image1;
    // [SerializeField] private Transform image2;
    // [SerializeField] private Transform image3;

    [SerializeField] private MeshRenderer image1;
    [SerializeField] private MeshRenderer image2;
    [SerializeField] private MeshRenderer image3;

    [SerializeField] private List<MeshRenderer> images;

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

        //main menu check
        if(images == null) return;

        // Load all images in the folder
        string[] imagePaths = Directory.GetFiles(decalsDirectory); // Adjust file extension as needed

        int index = 0;
        foreach(MeshRenderer image in images) {
            // print("Cycling image: " + index);
            if(index >= imagePaths.Length) {
                image.gameObject.SetActive(false);
            } else {
                image.gameObject.SetActive(true);
                string imagePath = imagePaths[index];

                byte[] fileData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(1, 1); // Create a new Texture2D
                texture.LoadImage(fileData); // Load the image data into the texture

                // print("Imported image: " + imagePath);

                image.GetComponent<MeshRenderer>().material.mainTexture = texture;
            }

            index ++;
        }

        // int index = 0;
        // foreach(string imagePath in imagePaths) {
        //     byte[] fileData = File.ReadAllBytes(imagePath);
        //     Texture2D texture = new Texture2D(1, 1); // Create a new Texture2D
        //     texture.LoadImage(fileData); // Load the image data into the texture

        //     // Create a GameObject to display the texture (example)
        //     // GameObject imageObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //     // Destroy(imageObject.GetComponent<MeshCollider>());
        //     // imageObject.GetComponent<MeshRenderer>().material.mainTexture = texture; // Apply the texture to the GameObject's material

        //     Debug.Log("Imported image: " + imagePath);

        //     //place image
        //     if(index == 0) {
        //         image1.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //     } else if(index == 1) {
        //         image2.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //     } else if(index == 2) {
        //         image3.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //     }

        //     index ++;
        // }
    }

    private void Update() {
        
    }
}
