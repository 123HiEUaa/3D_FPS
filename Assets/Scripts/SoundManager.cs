using UnityEngine;
using static Weapon;
[AddComponentMenu("HMFPS/SoundManager")]

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingChannel;

    public AudioSource reloadingSoundM107;
    public AudioSource reloadingSound1911;

    public AudioSource emtyMangazineSound1911;

    public AudioClip M107Shot;
    public AudioClip PisTol1911shot;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerDie;
    public AudioClip playerHurt;

    public AudioClip gameOverMusic;

    public AudioSource backgroundSound;
    public AudioClip backgroundmusic;


    private void Start()
    {
        backgroundSound.PlayOneShot(backgroundmusic);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol1911:
                shootingChannel.PlayOneShot(PisTol1911shot);
                break;

            case WeaponModel.M107:
                shootingChannel.PlayOneShot(M107Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {

        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSoundM107.Play();
                break;

            case WeaponModel.M107:
                reloadingSoundM107.Play();
                break;
        }
    }
}
