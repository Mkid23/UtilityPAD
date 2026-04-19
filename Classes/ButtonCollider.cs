using Photon.Pun;
using UnityEngine;
using static TvvPancke.Menu.Main;
using static TvvPancke.Settings;

namespace TvvPancke.Classes
{
    internal class Button : MonoBehaviour
    {
        public string relatedText;

        public static float buttonCooldown = 0f;



        public void OnTriggerEnter(Collider collider)
        {
            if (Time.time > buttonCooldown && collider == buttonCollider && menu != null)
            {
                buttonCooldown = Time.time + 0.2f;
                GorillaTagger.Instance.StartVibration(rightHanded, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                GorillaTagger.Instance.StartCoroutine(PlaySFX(buttonSfxUrl));
                Toggle(this.relatedText);
            }
        }
    }
}
