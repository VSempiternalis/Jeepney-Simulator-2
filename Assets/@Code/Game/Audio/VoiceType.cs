using UnityEngine;
using System.Collections.Generic;

public class VoiceType : MonoBehaviour {
    // [Header("SUBS MANAGER VALUES")]
    // public int voiceType; //the unique id of this speaker/voice
    public string voiceName;

    [Space(5)]
    [Header("MAIN")]
    public List<AudioClip> payAudios; //0
    public List<string> paySubsORIG;
    public List<string> paySubsENG; //0

    [Space(5)]
    public List<AudioClip> changeAudios;
    public List<string> changeSubsORIG;
    public List<string> changeSubsENG;

    [Space(5)]
    public List<AudioClip> stopAudios;
    public List<string> stopSubsORIG;
    public List<string> stopSubsENG;

    [Space(5)]
    [Header("OPTIONAL")]
    public List<AudioClip> dropAudios; //3
    public List<string> dropSubsORIG;
    public List<string> dropSubsENG; //3

    [Space(5)]
    public List<AudioClip> thanksAudios;
    public List<string> thanksSubsORIG;
    public List<string> thanksSubsENG;

    [Space(5)]
    public List<AudioClip> deathAudios;
    public List<string> deathSubsORIG;
    public List<string> deathSubsENG;

    [Space(5)]
    public List<AudioClip> chatterAudios; //6
    public List<string> chatterSubsORIG;
    public List<string> chatterSubsENG; //6

    [Space(5)]
    public List<AudioClip> gasStationAudios;
    public List<string> gasSubsORIG;
    public List<string> gasSubsENG;

    [Space(5)]
    public List<AudioClip> hitAudios;
    public List<string> hitSubsORIG;
    public List<string> hitSubsENG;

    [Space(5)]
    public List<AudioClip> policeAudios; //9
    public List<string> policeSubsORIG;
    public List<string> policeSubsENG; //9
}
