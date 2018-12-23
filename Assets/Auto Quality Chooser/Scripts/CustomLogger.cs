using UnityEngine;

namespace AutoQualityChooser
{

    public class CustomLogger : MonoBehaviour
    {
        [System.Diagnostics.Conditional("AutoQualityLogger")]
        public static void Log(string message, object myClass)
        {
            Debug.Log(string.Format("#{0}# {1}", GetNamespacelessName(myClass), message));
        }
        private static string GetNamespacelessName(object obj)
        {
            string fullName = obj.GetType().ToString();
            string[] s = fullName.Split('.');
            return s[s.Length - 1];
        }
    }
}
