//-----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    /// <summary>
    /// Count for each column how many of the coordinates are in that column.
    /// </summary>
    /// <param name="coordinates">A list of coordinates</param>
    /// <param name="coordinates">The number of columns</param>
    /// <returns>Number of coordinates by column</returns>
    public static int[] CountInColumns(IEnumerable<Vector2Int> coordinates, int columns)
    {
        int[] countInColumns = new int[columns];
        foreach (var r in coordinates)
        {
            countInColumns[r.x]++;
        }

        return countInColumns;
    }
}
