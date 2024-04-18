using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkinCustomizer : MonoBehaviour {
    private BoundaryManager bm;

    [SerializeField] private List<Skin> skins;
    [SerializeField] private List<SteeringWheelSkin> swSkins;
    [SerializeField] private List<SeatsSkin> seatsSkins;
    private Skin currentSkin;
    private SteeringWheelSkin currentSWSkin;
    private SeatsSkin currentSeatsSkin;
    [SerializeField] private List<bool> skinsOwned;
    [SerializeField] private List<bool> swSkinsOwned;
    [SerializeField] private List<bool> seatSkinsOwned;

    //DISPLAY
    [Space(10)]
    [Header("SKINS")]
    [SerializeField] private MeshRenderer displayImage;
    [SerializeField] private GameObject buttonOff;
    [SerializeField] private TMP_Text buttonOffText;
    [SerializeField] private GameObject buttonOn;
    [SerializeField] private TMP_Text nameText;

    [SerializeField] private MeshRenderer bumper;
    [SerializeField] private MeshRenderer frame;
    [SerializeField] private MeshRenderer mainBody;
    [SerializeField] private MeshRenderer roof;

    [Space(10)]
    [Header("STEERING WHEEL")]
    [SerializeField] private MeshRenderer swDisplayImage;
    [SerializeField] private GameObject swButtonOff;
    [SerializeField] private TMP_Text swButtonOffText;
    [SerializeField] private GameObject swButtonOn;
    [SerializeField] private TMP_Text swNameText;

    [SerializeField] private MeshRenderer swMesh;

    [Space(10)]
    [Header("SEATS")]
    [SerializeField] private MeshRenderer seatDisplayImage;
    [SerializeField] private GameObject seatButtonOff;
    [SerializeField] private TMP_Text seatButtonOffText;
    [SerializeField] private GameObject seatButtonOn;
    [SerializeField] private TMP_Text seatNameText;

    [SerializeField] private MeshRenderer frontMesh1;
    [SerializeField] private MeshRenderer frontMesh2;
    [SerializeField] private List<MeshRenderer> rearMeshes;

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

    [Serializable] public struct SteeringWheelSkin {
        public string skinName;
        public int price;
        public bool isOwned;
        public Material displayImage;

        public Material skin;
    }

    [Serializable] public struct SeatsSkin {
        public string skinName;
        public int price;
        public bool isOwned;
        public Material displayImage;

        public Material frontSkin;
        public Material rearSkin;
    }

    private void Start() {
        bm = BoundaryManager.current;
    }

    #region TRYBUY =======================================================================================

    public void TryBuyCurrentSkin() {
        print("BUYING SKIN: " + currentSkin.skinName);

        // check if can afford
        if(bm.deposit >= currentSkin.price) {
            bm.AddToDeposit(-currentSkin.price);
            skinsOwned[skins.IndexOf(currentSkin)] = true;
            SetSkinToCurrent();
            SetDisplay(skins.IndexOf(currentSkin));
        } else {
            NotificationManager.current.NewNotif("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.");
        }
    }

    public void TryBuyCurrentSWSkin() {
        print("BUYING SKIN: " + currentSWSkin.skinName);

        // check if can afford
        if(bm.deposit >= currentSWSkin.price) {
            bm.AddToDeposit(-currentSWSkin.price);
            swSkinsOwned[swSkins.IndexOf(currentSWSkin)] = true;
            SetSWSkinToCurrent();
            SetSWDisplay(swSkins.IndexOf(currentSWSkin));
        } else {
            NotificationManager.current.NewNotif("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.");
        }
    }

    public void TryBuyCurrentSeatSkin() {
        print("BUYING SKIN: " + currentSeatsSkin.skinName);

        // check if can afford
        if(bm.deposit >= currentSeatsSkin.price) {
            bm.AddToDeposit(-currentSeatsSkin.price);
            seatSkinsOwned[seatsSkins.IndexOf(currentSeatsSkin)] = true;
            SetSeatSkinToCurrent();
            SetSeatDisplay(seatsSkins.IndexOf(currentSeatsSkin));
        } else {
            NotificationManager.current.NewNotif("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.");
        }
    }

    #endregion TRYBUY
    #region SET DISPLAY ============================================================================

    public void SetDisplay(int index) {
        print("set display: " + skinsOwned.Count);
        currentSkin = skins[index];

        //set image
        displayImage.material = currentSkin.displayImage;
        nameText.text = currentSkin.skinName;

        //show right button
        if(skinsOwned[index]) {
            buttonOff.SetActive(false);
            buttonOn.SetActive(true);
        } else {
            buttonOn.SetActive(false);
            buttonOff.SetActive(true);
            buttonOffText.text = "BUY - P" + currentSkin.price;
        }
    }

    public void SetSWDisplay(int index) {
        print("set sw display: " + swSkinsOwned.Count);
        currentSWSkin = swSkins[index];

        //set image
        swDisplayImage.material = currentSWSkin.displayImage;
        swNameText.text = currentSWSkin.skinName;

        //show right button
        if(swSkinsOwned[index]) {
            swButtonOff.SetActive(false);
            swButtonOn.SetActive(true);
        } else {
            swButtonOn.SetActive(false);
            swButtonOff.SetActive(true);
            swButtonOffText.text = "BUY - P" + currentSWSkin.price;
        }
    }

    public void SetSeatDisplay(int index) {
        currentSeatsSkin = seatsSkins[index];

        //set image
        seatDisplayImage.material = currentSeatsSkin.displayImage;
        seatNameText.text = currentSeatsSkin.skinName;

        //show right button
        if(seatSkinsOwned[index]) {
            seatButtonOff.SetActive(false);
            seatButtonOn.SetActive(true);
        } else {
            seatButtonOn.SetActive(false);
            seatButtonOff.SetActive(true);
            seatButtonOffText.text = "BUY - P" + currentSeatsSkin.price;
        }
    }

    #endregion SET DISPLAY
    #region SET SKIN TO CURRENT ============================================================================

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

    public void SetSWSkinToCurrent() {
        AudioManager.current.PlayUI(15);

        //STEERING WHEEL
        Material[] swMats = swMesh.materials;
        swMats[0] = currentSWSkin.skin;
        swMesh.materials = swMats;
    }

    public void SetSeatSkinToCurrent() {
        AudioManager.current.PlayUI(15);

        //FRONT 1
        Material[] frontMats = frontMesh1.materials;
        frontMats[1] = currentSeatsSkin.frontSkin;
        frontMesh1.materials = frontMats;
        frontMesh2.materials = frontMats;

        //REAR
        Material[] rearMats = rearMeshes[0].materials;
        rearMats[1] = currentSeatsSkin.rearSkin;
        foreach(MeshRenderer mesh in rearMeshes) {
            mesh.materials = rearMats;
        }
    }

    #endregion SET SKIN TO CURRENT
    #region SAVELOAD ============================================================================

    //SAVING
    public string GetSkinsOwned() {
        string returnString = "";

        foreach(bool isOwned in skinsOwned) {
            returnString += isOwned? "1":"0";
        }

        print("Saving skins owned: " + returnString);
        return returnString;
    }

    public string GetSWSkinsOwned() {
        string returnString = "";

        foreach(bool isOwned in swSkinsOwned) {
            returnString += isOwned? "1":"0";
        }

        print("Saving sw skins owned: " + returnString);
        return returnString;
    }

    public string GetSeatSkinsOwned() {
        string returnString = "";

        foreach(bool isOwned in seatSkinsOwned) {
            returnString += isOwned? "1":"0";
        }

        print("Saving seat skins owned: " + returnString);
        return returnString;
    }

    //SAVE CURRENT SKIN
    public int GetCurrentSkin() {
        return skins.IndexOf(currentSkin);
    }
    public int GetCurrentSWSkin() {
        return swSkins.IndexOf(currentSWSkin);
    }
    public int GetCurrentSeatSkin() {
        return seatsSkins.IndexOf(currentSeatsSkin);
    }

    //LOADING
    public void LoadSavedSkins(string skinsString) {
        print("Loading skins owned: " + skinsString);

        foreach(char c in skinsString) {
            bool isOwned = (c == '1');
            skinsOwned.Add(isOwned);
        }

        // SetDisplay(0);
    }

    public void LoadSavedSWSkins(string skinsString) {
        print("Loading sw skins owned: " + skinsString);

        foreach(char c in skinsString) {
            bool isOwned = (c == '1');
            swSkinsOwned.Add(isOwned);
        }

        // SetSWDisplay(0);
    }

    public void LoadSavedSeatSkins(string skinsString) {
        print("Loading seat skins owned: " + skinsString);

        foreach(char c in skinsString) {
            bool isOwned = (c == '1');
            seatSkinsOwned.Add(isOwned);
        }

        // SetSeatDisplay(0);
    }

    //LOAD CURRENT SKINS
    public void LoadSkin(int index) {
        SetDisplay(index);
        SetSkinToCurrent();
    }
    public void LoadSWSkin(int index) {
        SetSWDisplay(index);
        SetSWSkinToCurrent();
    }
    public void LoadSeatSkin(int index) {
        SetSeatDisplay(index);
        SetSeatSkinToCurrent();
    }

    //DEFAULT
    public void LoadDefaultSkins() {
        // populate skins owned list
        for(int i = 0; i < skins.Count; i++) {
            skinsOwned.Add(false);
        }
        for(int i = 0; i < swSkins.Count; i++) {
            swSkinsOwned.Add(false);
        }
        for(int i = 0; i < seatsSkins.Count; i++) {
            seatSkinsOwned.Add(false);
        }

        SetDisplay(3);
        SetSWDisplay(2);
        SetSeatDisplay(0);

        SetSkinToCurrent();
        SetSWSkinToCurrent();
        SetSeatSkinToCurrent();
    }

    #endregion SAVELOAD
}
