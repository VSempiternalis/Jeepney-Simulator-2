using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CrimeManager : MonoBehaviour {
    public static CrimeManager current;
    private PlayerDriveInput pdi;
    private NotificationManager nm;
    private SpawnArea sa;

    [SerializeField] private List<PoliceCar> policeCars;
    [SerializeField] private int copCarRange;
    private int carsChasing;

    [SerializeField] private bool isPlayerWanted;
    private int wantedLevel;
    private int fines;

    //UI
    [SerializeField] private GameObject finesUI;
    [SerializeField] private GameObject breakingNews;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;

    //Audio
    [SerializeField] private AudioSource audioSource;

    private void Awake() {
        current = this;
    }

    private void Start() {
        pdi = PlayerDriveInput.current;
        nm = NotificationManager.current;
        sa = SpawnArea.current;
    }

    private void Update() {
        
    }

    private void SetIsPlayerWanted(bool newVal) {
        print("SET IS PLAYER WANTED: " + (newVal? "TRUE":"FALSE"));
        isPlayerWanted = newVal;

        //stop spawning vics
        sa.isSpawningVehicles = !isPlayerWanted;

        //UI
        finesUI.GetComponent<TMP_Text>().text = "FINES: P" + fines;
        finesUI.SetActive(isPlayerWanted);
        breakingNews.SetActive(isPlayerWanted);

        //UPDATE STARS
        if(wantedLevel == 3) {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        } else if(wantedLevel == 2) {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(false);
        } else if(wantedLevel == 1) {
            star1.SetActive(true);
            star2.SetActive(false);
            star3.SetActive(false);
        } else {
            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
        }

        //Audio
        if(isPlayerWanted && !audioSource.isPlaying) audioSource.Play();
        else audioSource.Stop();
    }

    public void NewViolation(int violation) {
        print("VIOLATION: " + violation);
        /*
        VIOLATIONS
        1 - Red light violation
        2 - Vehicle collision
        3 - Vehicular manslaughter
        */

        bool isPoliceAlert = false; //true if police car sees player commit crime

        //CHECK FOR COP CARS
        foreach(PoliceCar pc in policeCars) {
            if(pc.gameObject.activeSelf && Vector3.Distance(pc.transform.position, pdi.transform.position) <= copCarRange) {
                isPoliceAlert = true;

                //Update wanted level
                if(violation == 2) wantedLevel += 2;
                else wantedLevel ++;
                if(wantedLevel > 3) wantedLevel = 3;

                //update fines
                if(violation == 1) fines += 100;
                if(violation == 2) fines += 200;
                if(violation == 3) fines += 300;

                SetIsPlayerWanted(true);

                //set this police car to chase
                if(carsChasing < wantedLevel) {
                    pc.SetIsChasing(true);
                    carsChasing ++;
                }

                break;
            }
        }

        //NOTIFICATIONS
        if(isPoliceAlert) {
            if(violation == 1) nm.NewNotifColor("RED LIGHT VIOLATION", "A nearby police car just saw you running a red light!", 3);
            else if(violation == 2) nm.NewNotifColor("VEHICLE COLLISION", "A nearby police car just saw you colliding with a car!", 3);
            else if(violation == 3) nm.NewNotifColor("VEHICULAR MANSLAUGHTER", "A nearby police car just saw you crashing into a pedestrian!", 3);
        } else {
            if(violation == 1) nm.NewNotifColor("RED LIGHT VIOLATION", "You just ran a red light!\n\nLuckily for you, no cop car was around to see it...", 2);
            else if(violation == 2) nm.NewNotifColor("VEHICLE COLLISION", "You have collided with a car!\n\nLuckily for you, no cop car was around to see it...", 2);
            else if(violation == 3) nm.NewNotifColor("VEHICULAR MANSLAUGHTER", "You have crashed into a pedestrian!\n\nLuckily for you, no cop car was around to see it...", 2);
        }
    }
}
