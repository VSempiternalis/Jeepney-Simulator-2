using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DestinationsUIManager : MonoBehaviour {
    public static DestinationsUIManager current;
    [SerializeField] private Transform destinationsUI;
    [SerializeField] private GameObject destinationUIPF;
    private Dictionary<string, int> destPassDict = new Dictionary<string, int>();
    private AudioHandler playerAudio;

    private void Awake() {
        current = this;
    }

    private void Start() {
        //[+] playerAudio = PlayerInput.current.GetComponent<AudioHandler>();
    }

    public void AddDestination(string destName) {
        //If dest already in list
        if(destPassDict.ContainsKey(destName)) {
            //Add to passenger count
            destPassDict[destName] ++;

            //Update UI
            foreach(Transform dest in destinationsUI) {
                if(dest.name == destName) {
                    dest.GetChild(0).GetComponent<TMP_Text>().text = destName + " (" + destPassDict[destName] + ")";

                    // Animate the text scaling using LeanTween
                    LeanTween.scale(dest.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.5f)
                        .setEaseOutExpo()
                        .setOnComplete(() => {
                            LeanTween.scale(dest.gameObject, new Vector3(1f, 1f, 1f), 0.5f)
                            .setEaseOutExpo();
                        });

                    break;
                }
            }
        } else {
            destPassDict.Add(destName, 1);

            //UI
            GameObject dest = Instantiate(destinationUIPF, destinationsUI);
            Transform destTrans = dest.transform;
            destTrans.position = new Vector3(300f, 0f, 0f);
            destTrans.GetChild(0).GetComponent<TMP_Text>().text = destName + " (1)";
            dest.name = destName;
            dest.SetActive(true);

            // Animate the text scaling using LeanTween
            destTrans.localScale = new Vector3(0f, 0f, 0f);
            LeanTween.scale(dest.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.2f)
                .setEaseOutExpo()
                .setOnComplete(() => {
                    LeanTween.scale(dest.gameObject, new Vector3(1f, 1f, 1f), 0.2f)
                    .setEaseOutExpo();
                });
        }

        //Audio
        // playerAudio.Play(6);
    }

    public void RemoveDestination(string destName) {
        if(!destPassDict.ContainsKey(destName)) return;

        // Remove dest
        if(destPassDict.ContainsKey(destName) && destPassDict[destName] <= 1) {
            foreach(Transform destChild in destinationsUI) {
                if(destChild.name == destName) {
                    // Animate the text scaling using LeanTween
                    LeanTween.scale(destChild.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.2f)
                        .setEaseOutExpo()
                        .setOnComplete(() => {
                            LeanTween.scale(destChild.gameObject, new Vector3(0f, 0f, 0f), 0.2f)
                            .setEaseOutExpo()
                            .setOnComplete(() => {
                                Destroy(destChild.gameObject);
                                destPassDict.Remove(destName);
                            });
                        });
                    break;
                }
            }
        } else {
            foreach(Transform destChild in destinationsUI) {
                if(destChild.name == destName) {
                    destPassDict[destName] --;

                    // Animate the text scaling using LeanTween
                    LeanTween.scale(destChild.gameObject, new Vector3(1f, 1f, 1f), 0.2f)
                        .setEaseOutElastic();

                    destChild.GetChild(0).GetComponent<TMP_Text>().text = destName + " (" + destPassDict[destName] + ")";
                    break;
                }
            }
        }

        //Audio
        // playerAudio.Play(7);
    }

    public void Clear() {
        destPassDict.Clear();

        foreach(Transform destChild in destinationsUI) {
            Destroy(destChild.gameObject);
        }
    }
}
