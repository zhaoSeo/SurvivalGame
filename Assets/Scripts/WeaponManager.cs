using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{

    public static bool isChangeWeapon = false;
    public static Transform currentWeapon;
    public static Animator currentWeaponAnime;

    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private float changeWeaponDealyTime;

    [SerializeField]
    private float changeWeaponEndDelayTime;

    [SerializeField]
    private Gun[] guns;

    [SerializeField]
    private Hand[] hands;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;

    void Start()
    {
        for(int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for(int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            } else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnime.SetTrigger("Weapon_Out");
        Debug.Log("1");
        yield return new WaitForSeconds(changeWeaponDealyTime);
        Debug.Log("2");
        CancelPreWeaponAction();
        WeaponChange(_type, _name);
        Debug.Log("3");
        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        Debug.Log("4");
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                break;
            case "HAND":
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        Debug.Log("4.5");
        if (_type == "GUN")
        {
            Debug.Log("5");
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            Debug.Log("5.5");
            theHandController.HandChange(handDictionary[_name]);
        }
            
    }

}
