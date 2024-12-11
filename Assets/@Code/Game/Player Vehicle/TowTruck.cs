using UnityEngine;
using System.Collections.Generic;

public class TowTruck : MonoBehaviour {
    public static TowTruck current;
    // [SerializeField] private Transform carcon;
    private CrimeManager cm;
    [SerializeField] private ChunksManager chunkMan;

    [SerializeField] private Transform officePoint;
    private BoundaryManager bm;
    private Rigidbody rb;

    //PRICES
    [SerializeField] private int towPrice;
    [SerializeField] private int towToOfficePrice;
    [SerializeField] private int towToNearestGasStationPrice;

    [SerializeField] private List<Transform> gasStations;

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
        rb = GetComponent<Rigidbody>();
        cm = CrimeManager.current;
        // carcon = GetComponent<Transform>();
    }

    // private void Update() {
        
    // }

    public void PoliceTow() {
        //position
        Vector3 newPos = transform.position;
        newPos.y += 2;
        transform.position = newPos;

        //rotation
        transform.rotation = Quaternion.identity;

        rb.isKinematic = true;
        rb.isKinematic = false;

        //fade
        Fader.current.Yawn(0.1f, "Paying fines...", 1f);

        //audio
        AudioManager.current.PlayUI(1);

        //chunks
        chunkMan.ChunkCheck();
    }

    public void Tow() {
        if(!bm.CanPay(towPrice)) {
            NotificationManager.current.NewNotifColor("NOT ENOUGH DEPOSIT!", "There is not enough money in your deposit to tow!", 2);
            return;
        }

        //position
        Vector3 newPos = transform.position;
        newPos.y += 2;
        transform.position = newPos;

        //rotation
        transform.rotation = Quaternion.identity;

        rb.isKinematic = true;
        rb.isKinematic = false;

        //fade
        Fader.current.Yawn(0.1f, "Towing jeepney...", 1f);

        //audio
        AudioManager.current.PlayUI(1);
    }

    public void TowToOffice() {
        if(cm.isPlayerWanted) {
            NotificationManager.current.NewNotifColor("WANTED!", "Cannot tow to Billy's Office while you are wanted!", 3);
            return;
        } else if(!bm.CanPay(towToOfficePrice)) {
            NotificationManager.current.NewNotifColor("NOT ENOUGH DEPOSIT!", "There is not enough money in your deposit to tow!", 2);
            return;
        }

        transform.position = officePoint.position;
        transform.rotation = officePoint.rotation;

        rb.isKinematic = true;
        rb.isKinematic = false;

        //fade
        Fader.current.Yawn(0.1f, "Towing jeepney to Billy's Office...", 1f);

        //audio
        AudioManager.current.PlayUI(1);

        //chunks
        chunkMan.ChunkCheck();
    }

    public void TowToNearestGasStation() {
        if(cm.isPlayerWanted) {
            NotificationManager.current.NewNotifColor("WANTED!", "Cannot tow to nearest gas station while you are wanted!", 3);
            return;
        } 
        float dist = 10000;
        Transform nearestGasStation = null;

        foreach(Transform gs in gasStations) {
            float gsDist = Vector3.Distance(transform.position, gs.position);

            if(gsDist < dist) {
                dist = gsDist;
                nearestGasStation = gs;
            }
        }

        if(nearestGasStation == null) return;

        
        if(!bm.CanPay(towToNearestGasStationPrice)) {
            NotificationManager.current.NewNotifColor("NOT ENOUGH DEPOSIT!", "There is not enough money in your deposit to tow!", 2);
            return;
        }

        transform.position = nearestGasStation.position;
        transform.rotation = nearestGasStation.rotation;

        rb.isKinematic = true;
        rb.isKinematic = false;


        //fade
        Fader.current.Yawn(0.1f, "Towing jeepney to nearest gas station...", 1f);

        //audio
        AudioManager.current.PlayUI(1);

        //chunks
        chunkMan.ChunkCheck();
    }
}