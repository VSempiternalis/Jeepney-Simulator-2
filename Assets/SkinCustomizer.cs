using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkinCustomizer : MonoBehaviour {
    private BoundaryManager bm;

    [SerializeField] private List<Skin> skins;
    private Skin currentSkin;
    // [SerializeField] private List<Material> bumperSkins;
    // [SerializeField] private List<Material> frameSkins;
    // [SerializeField] private List<Material> hoodSkins;
    // [SerializeField] private List<Material> mainBodySkins;
    // [SerializeField] private List<Material> roofSkins;
    // [SerializeField] private List<int> skinPrices;
    [SerializeField] private List<bool> skinsOwned;

    //DISPLAY
    // [SerializeField] private List<Material> displayImages;
    [SerializeField] private MeshRenderer displayImage;
    [SerializeField] private GameObject buttonOff;
    [SerializeField] private TMP_Text buttonOffText;
    [SerializeField] private GameObject buttonOn;
    [SerializeField] private TMP_Text buttonOnText;
    // [SerializeField] private List<GameObject> buttonsOff;
    // [SerializeField] private List<GameObject> buttonsOn;

    [SerializeField] private MeshRenderer bumper;
    [SerializeField] private MeshRenderer frame;
    // [SerializeField] private MeshRenderer hood;
    [SerializeField] private MeshRenderer mainBody;
    [SerializeField] private MeshRenderer roof;

    [Serializable] public struct Skin {
        public string skinName;
        public int price;
        public bool isOwned;
        public Material displayImage;

        public Material bumperSkin;
        public Material frameSkin;
        public Material hoodSkin;
        public Material mainBodySkin;
        public Material roofSkin;
    }

    private void Start() {
        bm = BoundaryManager.current;

        //populate skins owned list
        // skinsOwned.Count = 12;
        // for(int i = 0; i < 18; i++){ //NOTE: INCREASE WHEN ADDING NEW SKINS!
        //     skinsOwned.Add(false);
        // }

        // SetDisplay(0);
    }

    private void Update() {
        
    }

    public void TryBuyCurrentSkin() {
        print("BUYING SKIN: " + currentSkin.skinName);

        // check if can afford
        if(bm.deposit >= currentSkin.price) {
            bm.AddToDeposit(-currentSkin.price);
            // currentSkin.isOwned = true;
            // skins[skins.IndexOf(currentSkin)].isOwned = true;
            skinsOwned[skins.IndexOf(currentSkin)] = true;
            SetSkinToCurrent();
            SetDisplay(skins.IndexOf(currentSkin));
        } else {
            NotificationManager.current.NewNotif("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.");
        }
    }

    public void SetDisplay(int index) {
        currentSkin = skins[index];

        //set image
        displayImage.material = currentSkin.displayImage;

        //show right button
        if(skinsOwned[index]) {
            buttonOff.SetActive(false);
            buttonOn.SetActive(true);
            buttonOnText.text = currentSkin.skinName + " (OWNED)";
        } else {
            buttonOn.SetActive(false);
            buttonOff.SetActive(true);
            buttonOffText.text = currentSkin.skinName + " - P" + currentSkin.price;
        }
    }

    public void SetSkinToCurrent() {
        AudioManager.current.PlayUI(17);

        //BUMPER
        Material[] bumperMats = bumper.materials;
        bumperMats[0] = currentSkin.bumperSkin;
        bumper.materials = bumperMats;

        //FRAME
        Material[] frameMats = frame.materials;
        frameMats[0] = currentSkin.frameSkin;
        frame.materials = frameMats;

        //HOOD AND BODY
        Material[] mainBodyMats = mainBody.materials;
        mainBodyMats[0] = currentSkin.hoodSkin;
        mainBodyMats[2] = currentSkin.mainBodySkin;
        mainBody.materials = mainBodyMats;

        //ROOF
        Material[] roofMats = roof.materials;
        roofMats[1] = currentSkin.roofSkin;
        roof.materials = roofMats;
    }

    //SAVING
    public string GetSkinsOwned() {
        string returnString = "";

        foreach(bool isOwned in skinsOwned) {
            returnString += isOwned? "1":"0";
        }

        print("Saving skins owned: " + returnString);
        return returnString;
    }

    //LOADING
    public void LoadSavedSkins(string skinsString) {
        print("Loading skins owned: " + skinsString);
        // List<bool> 
        // skinsOwned = ;

        foreach(char c in skinsString) {
            bool isOwned = (c == '1');
            skinsOwned.Add(isOwned);
        }

        SetDisplay(0);
    }

    public void LoadDefaultSkins() {
        // populate skins owned list
        for(int i = 0; i < 18; i++){ //NOTE: INCREASE WHEN ADDING NEW SKINS!
            skinsOwned.Add(false);
        }

        SetDisplay(0);
    }
}
