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
}
