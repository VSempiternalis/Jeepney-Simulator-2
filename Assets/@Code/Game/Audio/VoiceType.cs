using UnityEngine;
using System.Collections.Generic;

public class VoiceType : MonoBehaviour {
    [Header("MAIN")]
    public List<AudioClip> payAudios;
    public List<AudioClip> changeAudios;
    public List<AudioClip> stopAudios;

    [Header("OPTIONAL")]
    public List<AudioClip> dropAudios;
    public List<AudioClip> thanksAudios;
    public List<AudioClip> deathAudios;
    public List<AudioClip> chatterAudios;
    public List<AudioClip> gasStationAudios;
    public List<AudioClip> hitAudios;
    public List<AudioClip> policeAudios;
}
