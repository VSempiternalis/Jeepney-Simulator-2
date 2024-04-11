using UnityEngine;

public class TowTruck : MonoBehaviour {
    public static TowTruck current;
    // [SerializeField] private Transform carcon;

    [SerializeField] private Transform officePoint;
    private BoundaryManager bm;

    //PRICES
    [SerializeField] private int towPrice;
    [SerializeField] private int towToOfficePrice;

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
        // carcon = GetComponent<Transform>();
    }

    // private void Update() {
        
    // }

    public void Tow() {
        if(!bm.CanPay(towPrice)) {
            NotificationManager.current.NewNotif("NOT ENOUGH DEPOSIT!", "There is not enough money in your deposit to tow!");
            return;
        }

        //position
        Vector3 newPos = transform.position;
        newPos.y += 2;
        transform.position = newPos;

        //rotation
        transform.rotation = Quaternion.identity;

        //fade
        Fader.current.Yawn(0.1f, "Towing jeepney...", 1f);

        //audio
        AudioManager.current.PlayUI(1);
    }

    public void TowToOffice() {
        if(!bm.CanPay(towToOfficePrice)) {
            NotificationManager.current.NewNotif("NOT ENOUGH DEPOSIT!", "There is not enough money in your deposit to tow!");
            return;
        }

        transform.position = officePoint.position;
        transform.rotation = officePoint.rotation;

        //fade
        Fader.current.Yawn(0.1f, "Towing jeepney to Billy's Office...", 1f);

        //audio
        AudioManager.current.PlayUI(1);
    }
}