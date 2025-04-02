using UnityEngine;
[AddComponentMenu("HMFPS/AmmoBox")]
public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 125;
    public AmmoType ammoType;

    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo
    }
}
