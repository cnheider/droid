namespace droid.Runtime.GameObjects.NeodroidCamera.Synthesis.Experimental {
  public class StringColorEncoding {
    
    //TODO: FINISH
    public static UnityEngine.Color EncodeStrAsColor(string string_to_hash) {
      var hasher = System.Security.Cryptography.MD5.Create();
      var hash = hasher.ComputeHash(buffer : System.Text.Encoding.UTF8.GetBytes(s : string_to_hash));
      var hashed_integer = System.BitConverter.ToInt32(value : hash, 0);
      
      //Then to convert an integer into a random color use this.In this example 256 hues will be selected from the color wheel:
      var color = UnityEngine.Color.HSVToRGB(H : (System.Math.Abs(value : hashed_integer) % 256) / 256.0f, 1.0f, 0.75f);
      
      return color;
    }

    public void a(int hashed_integer) {
      //This same technique can be applied to other things as well.For example we can use the hashed value to index into a lookup table of names.Other approaches could apply a specific texture to a material or instantiate a specific prefab:
      var potential_names = new string[] {"Wsada", "Ygsaqd", "Uwtqsad"};
      var name = potential_names[System.Math.Abs(value : hashed_integer) % potential_names.Length];
    }
  }
}