using UnityEngine;

public class TileScript : MonoBehaviour {
    public bool[] tunnels = new bool[8];
    // Markers for the entrances and exits for the tile
    /*     Map: Only cardinal directions and digonals
     *     0   1   2
     *     4       3
     *     5   6   7
     *      The connecting exits and entrances sum to 7 - e.g. 0 connects to 7, 1 connects to 6, 2 to 5, and 3 to 4
     */
    public bool[] getAllMarkers()
    {
        return tunnels;
    }
    public bool getNorthMarker()
    {
        return tunnels[1];
    }
    public bool getSouthMarker()
    {
        return tunnels[6];
    }
    public bool getEastMarker()
    {
        return tunnels[3];
    }
    public bool getWestMarker()
    {
        return tunnels[4];
    }
    public bool getNorthEastMarker()
    {
        return tunnels[2];
    }
    public bool getSouthEastMarker()
    {
        return tunnels[7];
    }
    public bool getNorthWestMarker()
    {
        return tunnels[0];
    }
    public bool getSouthWestMarker()
    {
        return tunnels[5];
    }
    public bool getMarkerIndex(int index)
    {
        return tunnels[index];
    }
}
