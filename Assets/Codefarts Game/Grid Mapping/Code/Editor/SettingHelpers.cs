/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor
{
    using System;

    using Codefarts.CoreProjectCode.Settings;

    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Provides helper methods for drawing controls that are bound to settings.
    /// </summary>
    /// <remarks>These methods are designed to simplify drawing controls for changing settings.</remarks>
    public static class SettingHelpers
    {
        /// <summary>
        /// Draws a integer field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsIntField(string key, string text, int defaultValue, Action changed)
        {
            DrawSettingsIntField(key, text, defaultValue, int.MinValue, int.MaxValue, changed);
        }

        /// <summary>
        /// Draws a integer field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsIntField(string key, string text, int defaultValue)
        {
            DrawSettingsIntField(key, text, defaultValue, int.MinValue, int.MaxValue, null);
        }

        /// <summary>
        /// Draws a integer field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="min">
        /// The minimum value that the setting can have.
        /// </param>
        /// <param name="max">
        /// The maximum value that the setting can have.
        /// </param>    
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsIntField(string key, string text, int defaultValue, int min, int max, Action changed)
        {
            var intValue = defaultValue;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                intValue = settings.GetSetting(key, defaultValue);
            }

            var value = EditorGUILayout.IntField(text, intValue);
            value = value < min ? min : value;
            value = value > max ? max : value;
            if (value != intValue)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a integer field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="min">
        /// The minimum value that the setting can have.
        /// </param>
        /// <param name="max">
        /// The maximum value that the setting can have.
        /// </param>    
        public static void DrawSettingsIntField(string key, string text, int defaultValue, int min, int max)
        {
            DrawSettingsIntField(key, text, defaultValue, min, max, null);
        }

        /// <summary>
        /// Draws a float field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsFloatField(string key, string text, float defaultValue, Action changed)
        {
            DrawSettingsFloatField(key, text, defaultValue, float.MinValue, float.MaxValue, changed);
        }

        /// <summary>
        /// Draws a float field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsFloatField(string key, string text, float defaultValue)
        {
            DrawSettingsFloatField(key, text, defaultValue, float.MinValue, float.MaxValue, null);
        }

        /// <summary>
        /// Draws a float field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="min">
        /// The minimum value that the setting can have.
        /// </param>
        /// <param name="max">
        /// The maximum value that the setting can have.
        /// </param>    
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsFloatField(string key, string text, float defaultValue, float min, float max, Action changed)
        {
            var floatValue = defaultValue;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                floatValue = settings.GetSetting(key, defaultValue);
            }

            var value = EditorGUILayout.FloatField(text, floatValue);
            value = value < min ? min : value;
            value = value > max ? max : value;
            if (Math.Abs(value - floatValue) > Mathf.Epsilon)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a boolean field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsBoolField(string key, string text, bool defaultValue)
        {
            DrawSettingsBoolField(key, text, defaultValue, null);
        }

        /// <summary>
        /// Draws a boolean field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsBoolField(string key, string text, bool defaultValue, Action changed)
        {
            var boolValue = defaultValue;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                boolValue = settings.GetSetting(key, defaultValue);
            }

            var value = EditorGUILayout.Toggle(text, boolValue);
            if (value != boolValue)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a integer field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="min">
        /// The minimum value that the setting can have.
        /// </param>
        /// <param name="max">
        /// The maximum value that the setting can have.
        /// </param>    
        public static void DrawSettingsFloatField(string key, string text, float defaultValue, float min, float max)
        {
            DrawSettingsFloatField(key, text, defaultValue, min, max, null);
        }

        /// <summary>
        /// Draws a  CheckBox control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultState">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsCheckBox(string key, string text, bool defaultState, Action changed)
        {
            var isChecked = defaultState;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                isChecked = settings.GetSetting(key, defaultState);
            }

            var value = GUILayout.Toggle(isChecked, text);
            if (value != isChecked)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a  CheckBox control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultState">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsCheckBox(string key, string text, bool defaultState)
        {
            DrawSettingsCheckBox(key, text, defaultState, null);
        }

        /// <summary>
        /// Draws a popup control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="label">The label that will be displayed next to the popup.</param>
        /// <param name="text">
        /// The array of strings that will be used as the available options.
        /// </param>
        /// <param name="defaultIndex">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>  
        /// <remarks>if <see cref="label"/> is null no label will be drawn.</remarks>  
        public static void DrawSettingsPopup(string key, string label, string[] text, int defaultIndex, Action changed)
        {
            var index = defaultIndex;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                index = settings.GetSetting(key, defaultIndex);
            }

            if (label != null)
            {
                GUILayout.Label(label);
            }

            var value = EditorGUILayout.Popup(index, text);
            if (value != index)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a popup control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The array of strings that will be used as the available options.
        /// </param>
        /// <param name="defaultIndex">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsPopup(string key, string[] text, int defaultIndex)
        {
            DrawSettingsPopup(key, null, text, defaultIndex, null);
        }

        /// <summary>
        /// Draws a popup control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="label">The label that will be displayed next to the popup.</param>
        /// <param name="text">
        /// The array of strings that will be used as the available options.
        /// </param>
        /// <param name="defaultIndex">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsPopup(string key, string label, string[] text, int defaultIndex)
        {
            DrawSettingsPopup(key, label, text, defaultIndex, null);
        }

        /// <summary>
        /// Draws a text field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsTextField(string key, string text, string defaultValue, Action changed)
        {
            var textValue = defaultValue;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                textValue = settings.GetSetting(key, defaultValue);
            }

            var value = EditorGUILayout.TextField(text, textValue);
            if (value != textValue)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a text field control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsTextField(string key, string text, string defaultValue)
        {
            DrawSettingsTextField(key, text, defaultValue, null);
        }

        /// <summary>
        /// Draws a text box control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsTextBox(string key, string defaultValue, Action changed)
        {
            var textValue = defaultValue;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                textValue = settings.GetSetting(key, defaultValue);
            }

            var value = EditorGUILayout.TextArea(textValue);
            if (value != textValue)
            {
                settings.SetValue(key, value);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a text box control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsTextBox(string key, string defaultValue)
        {
            DrawSettingsTextBox(key, defaultValue, null);
        }

        /// <summary>
        /// Draws a  color picker control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultState">
        /// The default value for the setting.
        /// </param>
        /// <param name="changed">A callback that will be invoked if the state of the control changes.</param>    
        public static void DrawSettingsColorPicker(string key, string text, Color defaultState, Action changed)
        {
            var color = defaultState;
            var settings = SettingsManager.Instance;

            if (settings.HasValue(key))
            {
                color = settings.GetSetting(key, defaultState);
            }

            var selectedColor = EditorGUILayout.ColorField(text, color);
            if (selectedColor != color)
            {
                settings.SetValue(key, selectedColor);
                if (changed != null)
                {
                    changed();
                }
            }
        }

        /// <summary>
        /// Draws a  color picker control for settings.
        /// </summary>
        /// <param name="key">
        /// Specifies key within settings.
        /// </param>
        /// <param name="text">
        /// The text that will be used as the controls label.
        /// </param>
        /// <param name="defaultState">
        /// The default value for the setting.
        /// </param>
        public static void DrawSettingsColorPicker(string key, string text, Color defaultState)
        {
            DrawSettingsColorPicker(key, text, defaultState, null);
        }
    }
}
