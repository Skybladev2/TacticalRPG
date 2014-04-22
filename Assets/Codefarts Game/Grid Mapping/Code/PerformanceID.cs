/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
#if PERFORMANCE
namespace Codefarts.GridMapping
{
    /// <summary>
    /// Provides various keys as global constant values for use with the settings system.
    /// </summary>
    public enum PerformanceID
    {
        DrawFillRectangle,
        DrawRectangle,
        PositionPrefabOnMapExtensionMethod,
        PositionPrefabOnMap_AutoCenter,
        PositionPrefabOnMap_ScalePrefab,
        FillRectangleCallbacks,
        Erase,
        Draw,
        EraseFillRectangle,
        EraseRectangle,
        RecalculateHighlightPosition,
        GetGridPosition,
        FillRectangleExtensionMethod,
        FillRectangleExtensionMethod_Instantiation,
        ScalePrefab,                    
        TryParsePrefabName,             
        PositionPrefabOnMap_Rotate,     
        FillRectangleExtensionMethod_Callback,

        PositionPrefabOnMap_SetPosition,

        PositionPrefabOnMap_SetParent,

        RectangleCallbacks
    }
} 
#endif