namespace Codefarts.GridMapping.Editor.Controls
{
    using System;

    using UnityEngine;

    public class ControlGrid
    {
        /// <summary>
        /// Draw a grid of check boxes similar to SelectionGrid.
        /// </summary>
        /// <param name="checkedValues">Specifies the checked values of the check boxes.</param>
        /// <param name="text">The content for each individual check box.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <param name="style">The style to be applied to each check box.</param>
        /// <param name="options">Specifies layout options to be applied to each check box.</param>
        /// <returns>Returns the checked state for each check box.</returns>
        /// <remarks><p>Check boxes are drawn top to bottom, left to right.</p>  </remarks>
        /// <exception cref="IndexOutOfRangeException">Can occur if the size of the array is too small.</exception>
        public static bool[] DrawCheckBoxGrid(bool[] checkedValues, string[] text, int columns, GUIStyle style, params GUILayoutOption[] options)
        {
            // convert string content into gui content
            var content = new GUIContent[text.Length];
            for (var i = 0; i < content.Length; i++)
            {
                content[i] = new GUIContent(text[i]);
            }
            return DrawCheckBoxGrid(checkedValues, content, columns, style, options);
        }

        /// <summary>
        /// Draw a grid of check boxes similar to SelectionGrid.
        /// </summary>
        /// <param name="checkedValues">Specifies the checked values of the check boxes.</param>
        /// <param name="textures">The content for each individual check box.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <param name="style">The style to be applied to each check box.</param>
        /// <param name="options">Specifies layout options to be applied to each check box.</param>
        /// <returns>Returns the checked state for each check box.</returns>
        /// <remarks><p>Check boxes are drawn top to bottom, left to right.</p>  </remarks>
        /// <exception cref="IndexOutOfRangeException">Can occur if the size of the array is too small.</exception>
        public static bool[] DrawCheckBoxGrid(bool[] checkedValues, Texture2D[] textures, int columns, GUIStyle style, params GUILayoutOption[] options)
        {
            // convert texture content into gui content
            var content = new GUIContent[textures.Length];
            for (var i = 0; i < content.Length; i++)
            {
                content[i] = new GUIContent(string.Empty, textures[i]);
            }
            return DrawCheckBoxGrid(checkedValues, content, columns, style, options);
        }

        /// <summary>
        /// Draw a grid of check boxes similar to SelectionGrid.
        /// </summary>
        /// <param name="checkedValues">Specifies the checked values of the check boxes.</param>
        /// <param name="content">The content for each individual check box.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <param name="style">The style to be applied to each check box.</param>
        /// <param name="options">Specifies layout options to be applied to each check box.</param>
        /// <returns>Returns the checked state for each check box.</returns>
        /// <remarks><p>Check boxes are drawn top to bottom, left to right.</p>  </remarks>
        /// <exception cref="IndexOutOfRangeException">Can occur if the size of the array is too small.</exception>
        public static bool[] DrawCheckBoxGrid(bool[] checkedValues, GUIContent[] content, int columns, GUIStyle style, params GUILayoutOption[] options)
        {
            return DrawGenericGrid((e, i, s, o) => GUILayout.Toggle(e[i], content[i], style, options), checkedValues, columns, style, options);
        }

        /// <summary>
        /// Draw a grid of controls using a draw callback similar to SelectionGrid.
        /// </summary>
        /// <param name="drawCallback">Specifies a draw callback that is responsible for performing the actual drawing.</param>
        /// <param name="values">Specifies the values of the controls.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <param name="style">The style to be applied to each control.</param>
        /// <param name="options">Specifies layout options to be applied to each control.</param>
        /// <returns>Returns the value for each control.</returns>
        /// <remarks><p>Controls are drawn top to bottom, left to right.</p>  </remarks>
        /// <exception cref="IndexOutOfRangeException">Can occur if the size of the array is too small.</exception>
        /// <exception cref="ArgumentNullException">If the drawCallback is null.</exception>
        public static T[] DrawGenericGrid<T>(Func<T[], int, GUIStyle, GUILayoutOption[], T> drawCallback, T[] values, int columns, GUIStyle style, params GUILayoutOption[] options)
        {
            if (drawCallback == null)
            {
                throw new ArgumentNullException("drawCallback");
            }

            GUILayout.BeginVertical();
            var rowIndex = 0;
            var columnIndex = 0;
            var index = (rowIndex * columns) + columnIndex;

            var results = new T[values.Length];
            GUILayout.BeginHorizontal();
            while (index < values.Length)
            {
                // draw control
                results[index] = drawCallback(values, index, style, options);

                // move to next column
                columnIndex++;

                // if passed max columns move down to next row and set to first column
                if (columnIndex > columns - 1)
                {
                    columnIndex = 0;
                    rowIndex++;

                    // remember to start a new horizontal layout
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                // re-calculate the index
                index = (rowIndex * columns) + columnIndex;
            }
          
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
           
            return results;
        }
    }
}
