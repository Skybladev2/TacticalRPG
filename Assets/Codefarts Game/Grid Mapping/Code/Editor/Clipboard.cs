// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.Utilities
{
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class Clipboard
    {
        private string text;

        public static void SetText(string value)
        {
#if UNITY_EDITOR
            EditorGUIUtility.systemCopyBuffer = value;
#else
            this.text = value;
#endif
        }

        public static string GetText()
        {
#if UNITY_EDITOR
            return EditorGUIUtility.systemCopyBuffer;
#else
            return this.text;
#endif
        }
    }
}
