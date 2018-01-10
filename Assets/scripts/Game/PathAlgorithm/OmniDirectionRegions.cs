using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Algorithm that expands the play area in all directions evenly by one chunk every time it is run.
/// </summary>
[PathAlgorithm(AlgorithmType.RECT)]
public class OmniDirectionRegions : IRectAlgorithm
{
    private int DEFAULT_REGION_SIZE = 32;
    
    private List<Rect> outerRegionList;
    private Vector2Int origin;
    private int size;

    public int Size { get { return size; } }

    public Rect innerArea, outerArea;

    /// <summary>
    /// Read Only collection interface to the outer-region collection.
    /// </summary>
    public ReadOnlyCollection<Rect> OuterRegions
    {
        get
        {
            return outerRegionList.AsReadOnly();
        }
    }

    /// <summary>
    /// Empty constructor. Runs with default area size arguments (32 units assumed as chunk size) and (16, 16) origin.
    /// </summary>
    public OmniDirectionRegions()
    {
        Reset();
        Run();
    }

    /// <summary>
    /// Create a new OmniDirectionRegion with given chunk size and origin.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="origin"></param>
    public OmniDirectionRegions(int size, Vector2Int origin)
    {
        DEFAULT_REGION_SIZE = size;
        this.origin = origin;
        Reset();
        Run();
    }

    /// <summary>
    /// Check if passed location is located inside the inner region.
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public bool IsInInnerRegion(Vector2Int loc)
    {
        return innerArea.Contains(loc);
    }

    /// <summary>
    /// Check if the passed location is located inside the outer region.
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public bool IsInOuterRegion(Vector2Int loc)
    {
        return !innerArea.Contains(loc) && outerArea.Contains(loc);
    }

    /// <summary>
    /// Check if the passed location is outside the outer and inner regions.
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public bool IsOutsideRegion(Vector2Int loc)
    {
        return !outerArea.Contains(loc);
    }

    /// <summary>
    /// Outer region becomes new inner region.
    /// Create new outer area that is equal to the current size (size is set in the Run() method).
    /// </summary>
    private void updateInnerOuterRegions()
    {
        innerArea = outerArea;
        outerArea = new Rect(origin - new Vector2Int((int)(size / 2), (int)(size / 2)), new Vector2Int(size, size));
    }

    /// <summary>
    /// Reset the algorithm to the default.
    /// </summary>
    public void Reset()
    {
        size = DEFAULT_REGION_SIZE;
        outerArea = new Rect(origin - new Vector2Int((int)(size / 2), (int)(size / 2)), new Vector2Int(size, size));
    }

    /// <summary>
    /// Increment size.
    /// Update the inner and outer regions.
    /// Clear the outer region list.
    /// Set the outer region list rects.
    /// </summary>
    public void Run()
    {
        size += DEFAULT_REGION_SIZE * 2;
        updateInnerOuterRegions();
        outerRegionList = new List<Rect>();

        // Top region
        outerRegionList.Add(new Rect(
            new Vector2Int((int)(outerArea.xMin), (int)(outerArea.yMax - DEFAULT_REGION_SIZE)),
            new Vector2Int((int)(outerArea.width) - DEFAULT_REGION_SIZE, DEFAULT_REGION_SIZE)
            ));

        // Right region
        outerRegionList.Add(new Rect(
            new Vector2Int((int)(outerArea.xMax) - DEFAULT_REGION_SIZE, (int)(outerArea.yMin) + DEFAULT_REGION_SIZE),
            new Vector2Int(DEFAULT_REGION_SIZE, (int)(outerArea.height) - DEFAULT_REGION_SIZE)
            ));

        // Bottom region
        outerRegionList.Add(new Rect(
            new Vector2Int((int)(outerArea.xMin) + DEFAULT_REGION_SIZE, (int)(outerArea.yMin)),
            new Vector2Int((int)(outerArea.width) - DEFAULT_REGION_SIZE, DEFAULT_REGION_SIZE)
            ));

        // Left region
        outerRegionList.Add(new Rect(
            new Vector2Int((int)(outerArea.xMin), (int)(outerArea.yMin)),
            new Vector2Int(DEFAULT_REGION_SIZE, (int)(outerArea.height) - DEFAULT_REGION_SIZE)
            ));
    }

    /// <summary>
    /// Set the origin.
    /// </summary>
    /// <param name="origin"></param>
    public void SetOrigin(Vector2Int origin)
    {
        this.origin = origin;
    }
}
