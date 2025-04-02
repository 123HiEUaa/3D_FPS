using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("HMFPS/Weapon")]

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    [Header("Shooting")]
    // shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    // Chế độ Burst
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    //Cường độ(Spread)
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;


    [Header("Bullet")]
    //Bullet
    public GameObject bulletPrefabs;
    public Transform bulletSpawn; // Vị trí viên đạn được tạo
    public float bulletVelocity = 30; // Vận tốc của viên đạn
    public float bulletPrefabsLifeTime = 3f; //Thời gian tồn tại của viên đạn

    public GameObject muzzleEffect ;

    internal Animator animator ;

    [Header("Loading")]
    //Loading
    public float reloadTime;
    public int manazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    bool isADS;

    public enum WeaponModel
    {
        Pistol1911,
        M107
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }



    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = manazineSize;

        spreadIntensity = hipSpreadIntensity;
    }



    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {

            foreach (Transform child in transform )
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }

            if (isActiveWeapon)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    EnterADS();
                }
                if(Input.GetMouseButtonUp(1)) 
                {
                    ExitADS();
                }
            }



            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                //Tiếng Băng đạn trống
                SoundManager.Instance.emtyMangazineSound1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                // Giữ chuột trái
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                currentShootingMode == ShootingMode.Burst)
            {
                //Nhấp chuột trái
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < manazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>0)
            {
                Reload();
            }

            // Nếu muốn tự động nạp đạn khi băng đàn hết
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload();
            }



            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

            
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    
    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.midlleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    private void ExitADS() 
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.midlleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            //RECOIL_ADS
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }

 

        //SoundManager.Instance.shootingChannel.Play();

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDiretionAndSpread().normalized;

        //Khởi tạo viên đạn
        GameObject bullet = Instantiate(bulletPrefabs, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;
        //
        bullet.transform.forward = shootingDirection;
        // Băn viên đạn
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Phá hủy viên đạn sau 1 khoảng thời gian
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabsLifeTime));

        // Kiểm tra xem bắn xong chưa
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
            
        }

        //Chế độ Burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {

        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
        
    }

    private void ReloadCompleted()
    {
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > manazineSize)
        {
            bulletsLeft = manazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading =false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDiretionAndSpread()
    {
        //Băn từ giữa màn hình để kiểm tra đang ở đâu
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //Chạm vào một vật thể nào đó
            targetPoint = hit.point;
        }
        else
        {
            //Bắn lên ko trung
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the shooting direction and spread
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
