using UnityEngine;

public class OrbInfo
{
    public static OrbType DetermineColourType(char colourCode)
    {
        OrbType? type = null;

        // Convert character to upper
        colourCode = char.ToUpper(colourCode);

        switch (colourCode)
        {
            case 'B':
                type = OrbType.Blue;
                break;
            case 'G':
                type = OrbType.Green;
                break;
            case 'R':
                type = OrbType.Red;
                break;
            case 'Y':
                type = OrbType.Yellow;
                break;
        }
        return type.Value;
    }
}



public enum OrbType
{
    Blue,
    Green,
    Red,
    Yellow,
    None
}
