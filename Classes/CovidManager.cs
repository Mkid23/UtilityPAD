using System.Collections;
using UnityEngine;

namespace TvvPancke.Classes
{     
    /// DO NOT DO ANYTHING TO THIS CLASS

    public class CovidManager : MonoBehaviour
    {
        public static CovidManager instance;

        private void Awake() =>
            instance = this;
        
        public static Coroutine RunCoroutine(IEnumerator enumerator) =>
            instance.StartCoroutine(enumerator);

        public static void EndCoroutine(Coroutine enumerator) =>
            instance.StopCoroutine(enumerator);
    }
}
