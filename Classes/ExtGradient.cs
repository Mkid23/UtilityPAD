using System;
using UnityEngine;

namespace TvvPancke.Classes
{
    public class ExtGradient
    {
        public GradientColorKey[] colors = new GradientColorKey[]
        {
                
                new GradientColorKey(
                  new Color32(25, 25, 25, 255),0f 
                )
        };

        public bool isRainbow = false;
        public bool copyRigColors = false;

    }
}
