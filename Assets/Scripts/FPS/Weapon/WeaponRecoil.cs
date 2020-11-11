using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponRecoil : MonoBehaviour
{
    #region Public fields
    public float duration;
    public Vector2[] recoilPattern;
    #endregion

    #region Private fields
    private MoveCamera mainCamera;
    private float horizontalRecoil;
    private float verticalRecoil;

    private bool ownerIsPlayer;

    private float time = 0;
    private int index = 0;
    #endregion

    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    private void Start()
    {
        ownerIsPlayer = weapon.weaponManager.OwnerIsPlayer;
        if (ownerIsPlayer)
        {
            mainCamera = GetComponentInParent<MoveCamera>();
        }
        else
        {
            enabled = false;
        }  
    }

    public void Reset()
    {
        index = 0;
    }

    private void Update()
    {
        if(ownerIsPlayer && time > 0)
        {
            mainCamera.VerticalAngle -= (verticalRecoil/10) * Time.deltaTime / duration;
            mainCamera.HorizontalAngle -= (horizontalRecoil/10) * Time.deltaTime / duration;
            time -= Time.deltaTime;
        }
    }

    public void GenerateRecoil()
    {
        time = duration;

        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = (index + 1) % recoilPattern.Length;
    }
}
