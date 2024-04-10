using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayList", menuName = "Music Player/PlayList")]
public class PlayListSO : ScriptableObject
{
    public List<Sound> sounds;
}
