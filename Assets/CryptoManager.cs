using TMPro;
using UnityEngine;

public class CryptoManager : MonoBehaviour {
    public static CryptoManager current;
    private TimeManager tm;
    private BoundaryManager bm;

    [Header("Line Renderers")]
    [SerializeField] private LineRenderer BBClineR;
    [SerializeField] private LineRenderer SHTlineR;
    [SerializeField] private LineRenderer CUMlineR;

    [Header("Owned")]
    private int ownedBBC;
    private int ownedSHT;
    private int ownedCUM;
    
    [Header("Values")]
    private int priceBBC;
    private int priceSHT;
    private int priceCUM;
    
    [Header("Fluctuation")]
    [SerializeField] private int fluctuationBBC;
    [SerializeField] private int fluctuationSHT;
    [SerializeField] private int fluctuationCUM;
    
    [Header("SCREEN")]
    [SerializeField] private TMP_Text BBCOwnedText;
    [SerializeField] private TMP_Text SHTOwnedText;
    [SerializeField] private TMP_Text CUMOwnedText;
    [SerializeField] private TMP_Text BBCValueText;
    [SerializeField] private TMP_Text SHTValueText;
    [SerializeField] private TMP_Text CUMValueText;
    [SerializeField] private TMP_Text BBCTotalText;
    [SerializeField] private TMP_Text SHTTotalText;
    [SerializeField] private TMP_Text CUMTotalText;

    private void Awake() {
        current = this;
    }

    private void Start() {
        tm = TimeManager.current;
        bm = BoundaryManager.current;
        tm.onHourUpdateEvent += NewTick;

        priceBBC = Random.Range(1, 101);
        priceSHT = 100;
        priceCUM = Random.Range(1, 101);

        UpdateGraph();
        UpdateScreen();

        NewTick(1, 1);
        NewTick(1, 1);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F1)) NewTick(1, 1);
    }

    private void NewTick(int hours, int days) {
        UpdatePrices();
        UpdateGraph();
        UpdateScreen();
    }

    private void UpdatePrices() {
        priceBBC += Random.Range(-fluctuationBBC, fluctuationBBC+1);
        priceSHT += Random.Range(-fluctuationSHT, fluctuationSHT+1);
        priceCUM += Random.Range(-fluctuationCUM, fluctuationCUM+1);

        if(priceBBC < 1) priceBBC = 1;
        else if(priceBBC > 100) priceBBC = 100;

        if(priceSHT < 1) priceSHT = 1;
        else if(priceSHT > 100) priceSHT = 100;
        if(ownedSHT > 0) priceSHT = 1; //rugpull

        if(priceCUM < 1) priceCUM = 1;
        else if(priceCUM > 100) priceCUM = 100;
    }

    private void UpdateGraph() {
        UpdateLines(BBClineR, priceBBC);
        UpdateLines(SHTlineR, priceSHT);
        UpdateLines(CUMlineR, priceCUM);
    }

    private void UpdateLines(LineRenderer lr, int newValue) {
        lr.SetPosition(10, new Vector3(500, lr.GetPosition(9).y, 0));
        lr.SetPosition(9, new Vector3(600, lr.GetPosition(8).y, 0));
        lr.SetPosition(8, new Vector3(700, lr.GetPosition(7).y, 0));
        lr.SetPosition(7, new Vector3(800, lr.GetPosition(6).y, 0));
        lr.SetPosition(6, new Vector3(900, lr.GetPosition(5).y, 0));
        lr.SetPosition(5, new Vector3(1000, lr.GetPosition(4).y, 0));
        lr.SetPosition(4, new Vector3(1100, lr.GetPosition(3).y, 0));
        lr.SetPosition(3, new Vector3(1200, lr.GetPosition(2).y, 0));
        lr.SetPosition(2, new Vector3(1300, lr.GetPosition(1).y, 0));
        lr.SetPosition(1, new Vector3(1400, lr.GetPosition(0).y, 0));
        lr.SetPosition(0, new Vector3(1500, (newValue*10)+80, 0));    
    }

    private void UpdateScreen() {
        BBCOwnedText.text = "OWNED: " + ownedBBC;
        SHTOwnedText.text = "OWNED: " + ownedSHT;
        CUMOwnedText.text = "OWNED: " + ownedCUM;
        BBCValueText.text = "PRICE: P" + priceBBC;
        SHTValueText.text = "PRICE: P" + priceSHT;
        CUMValueText.text = "PRICE: P" + priceCUM;
        BBCTotalText.text = "TOTAL: P" + (priceBBC * ownedBBC);
        SHTTotalText.text = "TOTAL: P" + (priceSHT * ownedSHT);
        CUMTotalText.text = "TOTAL: P" + (priceCUM * ownedCUM);
    }

    public void Buy(string coin) {
        if(coin == "BBC" && ownedBBC < 100 && bm.CanPay(priceBBC)) ownedBBC ++;
        else if(coin == "SHT" && ownedSHT < 100 && bm.CanPay(priceSHT)) ownedSHT ++;
        else if(coin == "CUM" && ownedCUM < 100 && bm.CanPay(priceCUM)) ownedCUM ++;
        
        UpdateScreen();
    }

    public void Buy10(string coin) {
        for(int i = 0; i < 10; i++){
            Buy(coin);
        }
    }

    public void Sell(string coin) {
        if(coin == "BBC" && ownedBBC > 0) {
            ownedBBC --;
            bm.AddToDeposit(priceBBC);
        } else if(coin == "SHT" && ownedSHT > 0) {
            ownedSHT --;
            bm.AddToDeposit(priceSHT);
        } else if(coin == "CUM" && ownedCUM > 0) {
            ownedCUM --;
            bm.AddToDeposit(priceCUM);
        }

        UpdateScreen();
    }

    public void Sell10(string coin) {
        for(int i = 0; i < 10; i++){
            Sell(coin);
        }
    }
}
