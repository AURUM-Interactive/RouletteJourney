using UnityEngine;
    public class ProjUtilities : MonoBehaviour
    {
        // convert seconds to fixed timestep interval
        public static float SecondsToFixed(float seconds)
        {
            return seconds * 0.02f;
        }
    }


