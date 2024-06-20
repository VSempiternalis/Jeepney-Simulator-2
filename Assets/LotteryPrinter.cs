using UnityEngine;
using TMPro;

public class LotteryPrinter : MonoBehaviour {
    private int tensDigit;
    private int secondsDigit;
    [SerializeField] private TMP_Text tensDigitText;
    [SerializeField] private TMP_Text secondsDigitText;

    [SerializeField] private GameObject ticketPF;
    [SerializeField] private StorageHandler ticketStorage;
    [SerializeField] private int printCost;

    private LotteryManager lm;
    private TimeManager tm;

    private void Start() {
        lm = LotteryManager.current;
        tm = TimeManager.current;
    }

    private void Update() {

    }

    public void ModifyTensDigit(int mod) {
        if(mod > 0 && tensDigit == 9) {
            tensDigit = 0;
        } else if(mod < 0 && tensDigit == 0) {
            tensDigit = 9;
        } else tensDigit += mod;

        UpdateTexts();
            
        AudioManager.current.PlayUI(1);
    }

    public void ModifySecondsDigit(int mod) {
        if(mod > 0 && secondsDigit == 9) {
            secondsDigit = 0;
        } else if(mod < 0 && secondsDigit == 0) {
            secondsDigit = 9;
        } else secondsDigit += mod;

        UpdateTexts();
            
        AudioManager.current.PlayUI(1);
    }

    public void PrintTicket() {
        //if too late
        if(tm.shiftTimeLeft < 540) {
            NotificationManager.current.NewNotifColor("NO CHEATING!", "You cannot print a ticket until the next shift!", 2);
            return;
        }

        if(BoundaryManager.current.CanPay(printCost)) {
            GameObject newTicket = Instantiate(ticketPF, transform.position, Quaternion.identity);

            newTicket.SetActive(true);
            newTicket.GetComponent<LotteryTicket>().SetNumber(tensDigit, secondsDigit);
            newTicket.name = "Luto Ticket: " + tensDigit + "" + secondsDigit;

            ticketStorage.AddItemRandom(newTicket);
            lm.tickets.Add(newTicket);

            NotificationManager.current.NewNotif("TICKET BOUGHT", "Ticket cost: P10\n\nRemaining deposit: " + BoundaryManager.current.deposit);
            AudioManager.current.PlayUI(2);
        } else {
            NotificationManager.current.NewNotifColor("INSUFFICIENT FUNDS", "You do not have enough money in the deposit to afford a ticket!", 2);
            AudioManager.current.PlayUI(7);
        }
    }

    private void UpdateTexts() {
        tensDigitText.text = tensDigit.ToString();
        secondsDigitText.text = secondsDigit.ToString();
    }
}
