using UnityEngine;
using System.Collections;

public interface ICutscene
{
    IEnumerator StartCutscene();
    IEnumerator EndCutscene();
}
