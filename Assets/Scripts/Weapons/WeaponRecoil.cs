using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    #region Public fields
    public float duration;
    public Vector2[] recoilPattern;
    #endregion

    #region Private fields
    private float horizontalRecoil;
    private float verticalRecoil;

    private float time;
    private int index = 0;
    private MoveCamera playerCamera;
    #endregion

    private void Start()
    {
        if (GetComponent<Weapon>().ownerIsPlayer)
        {
            playerCamera = GetComponentInParent<MoveCamera>();
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
        if(time > 0)
        {
            playerCamera.m_VerticalAngle -= (verticalRecoil/10) * Time.deltaTime / duration;
            playerCamera.m_HorizontalAngle -= (horizontalRecoil/10) * Time.deltaTime / duration;
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
