using UnityEngine;

namespace Common
{
  public static class Helpers {
    /// <summary>
    /// Convert a value greater than the max of a set to a value within range.
    /// [0..3], input 4, result is 0
    /// </summary>
    /// <param name="input">int to wrap</param>
    /// <param name="min">minimum of set</param>
    /// <param name="max">maximum of set</param>
    /// <returns>Wrapped int within range</returns>
    public static int Wrap(int input, int min, int max) {
      if (input < min ) {
        return max - (min - input) % (max - min);
      } else {
        return min + (input - min) % (max - min);
      }
    }
    
    /// <summary>
    /// Unity does not provide a friendly method to convert a Hex -> Color. So here we are. 
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static Color32 HexStringToColor(string hexString)
    {
      var hex = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
      var r = (byte)((hex >> 16) & 0xFF);
      var g = (byte)((hex >> 8) & 0xFF);
      var b = (byte)((hex) & 0xFF);
      return new Color32(r, g, b, 255);
    }
  }
}
