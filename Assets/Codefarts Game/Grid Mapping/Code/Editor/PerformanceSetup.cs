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
    using Codefarts.CoreProjectCode.Services;

    using UnityEditor;

    /// <summary>
    /// Provides performance inspection menu items that integrate into unity's menu.
    /// </summary>
    [InitializeOnLoad]
    public class PerformanceSetup
    {
        /// <summary>
        /// Initializes static members of the <see cref="PerformanceSetup"/> class.
        /// </summary>
        static PerformanceSetup()
        {
            EditorCallbackService.Instance.Register(() =>
                {
                    var perf = PerformanceTesting<PerformanceID>.Instance;
                    var performanceIds = (PerformanceID[])System.Enum.GetValues(typeof(PerformanceID));
                    // perf.Remove(performanceIds);
                    perf.Create(performanceIds);
                    //new[]{PerformanceID.DrawFillRectangle, PerformanceID.PositionPrefabOnMapExtensionMethod, PerformanceID.ScalePrefab,  });
                    // Debug.Log("here");
                }, int.MinValue);
        }

        /// <summary>
        /// Provides a menu to allow developer to report performance.                                              
        /// </summary>
        [MenuItem("Window/Codefarts/Performance/Report")]
        public static void ReportPerformance()
        {
            Helpers.ReportPerformanceTimes();
        }

        /// <summary>
        /// Provides a menu to allow developer to reset performance numbers.                                              
        /// </summary>
        [MenuItem("Window/Codefarts/Performance/Reset")]
        public static void ResetPerformance()
        {
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.ResetAll(true);
        }
    }
}
#endif