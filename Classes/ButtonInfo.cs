using System;
using UnityEngine;

namespace TvvPancke.Classes
{
    public class ButtonInfo /// DONT CHANGE ANYTHING IN THIS CLASS UNLESS YOU KNOW WHAT YOU'RE DOING
    {
        public string buttonText = "-";
        public string overlapText = null;
        public Action method = null;
        public Action enableMethod = null;
        public Action disableMethod = null;
        public bool enabled = false;
        public bool isTogglable = true;
        public string toolTip = "This button doesn't have a tooltip/tutorial.";
        public Color? assignedColor = null;
    }
}
