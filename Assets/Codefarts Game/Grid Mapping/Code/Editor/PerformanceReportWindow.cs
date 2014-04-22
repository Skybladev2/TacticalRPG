/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
#if PERFORMANCE
namespace Codefarts.GridMapping.Editor
{
    using System;

    using UnityEditor;
    using UnityEngine;
    using System.Linq;


    /// <summary>
    /// Provides a performance reporting window.
    /// </summary>
    public class PerformanceReportWindow : EditorWindow
    {
        /// <summary>
        /// Holds the scroll value for the performance metrics.
        /// </summary>
        private Vector2 metricsScroll;

        /// <summary>
        /// Holds the performance data report information.
        /// </summary>
        private string performanceData;

        /// <summary>
        /// Holds the selected index for the selected performance metric.
        /// </summary>
        private int selectedIndex;

        /// <summary>
        /// Used to display names for each performance metric.
        /// </summary>
        private readonly string[] names = new[] { "Name", "Total", "Average", "Count" };

        /// <summary>
        /// Used to determine how the performance metrics are organized.
        /// </summary>
        private int sortIndex;   

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void OnGUI()
        {
            GUILayout.BeginVertical();

            // draw header
            this.DrawHeader();

            GUILayout.BeginHorizontal();
            this.DrawPerformanceMetrics();
            this.DrawPerformanceResults();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the tool bar header.
        /// </summary>
        private void DrawHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Reset"))
            {
                var perf = PerformanceTesting<PerformanceID>.Instance;
                perf.ResetAll(true);
            }
            this.sortIndex = EditorGUILayout.Popup(this.sortIndex, this.names);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw the <see cref="performanceData"/> results.
        /// </summary>
        private void DrawPerformanceResults()
        {
            GUILayout.TextArea(this.performanceData);
        }

        /// <summary>
        /// Draws the performance metrics table.
        /// </summary>
        private void DrawPerformanceMetrics()
        {
            var perf = PerformanceTesting<PerformanceID>.Instance;

            this.metricsScroll = GUILayout.BeginScrollView(this.metricsScroll, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, GUILayout.MaxWidth(300));
            var values = (PerformanceID[])Enum.GetValues(typeof(PerformanceID));
            switch (this.sortIndex)
            {
                case 0:
                    values = values.OrderBy(x => Enum.GetName(typeof(PerformanceID), x)).ToArray();
                    break;

                case 1:
                    values = values.OrderByDescending(x => perf.TotalTicks(x)).ToArray();
                    break;

                case 2:
                    values = values.OrderByDescending(x => perf.AverageTicks(x)).ToArray();
                    break;
                case 3:
                    values = values.OrderByDescending(x => perf.GetStartCount(x)).ToArray();
                    break;
            }

            Controls.ControlGrid.DrawGenericGrid<PerformanceID>((items, index, style, options) =>
              {
                  var value = items[index];
                  var selected = GUILayout.Toggle(this.selectedIndex == index, Enum.GetName(typeof(PerformanceID), value), style);
                  if (selected)
                  {
                      this.selectedIndex = index;
                      this.performanceData = String.Format("{0}\r\nTotal: {1}ms ({4} ticks)\r\nAverage: {2}ms ({5} ticks)\r\nCount: {3}\r\n", value, perf.TotalMilliseconds(value), perf.AverageMilliseconds(value), perf.GetStartCount(value), perf.TotalTicks(value), perf.AverageTicks(value));
                  }
                  return value;
              }, values, 1, GUI.skin.button);


            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Shows the <see cref="PerformanceReportWindow"/>.
        /// </summary>
        [MenuItem("Window/Codefarts/Performance/Results")]
        public static void ShowWindow()
        {
            var window = GetWindow<PerformanceReportWindow>("Performance Results");
            window.Show();
        }
    }
}    
#endif