using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;

    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anime.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //공격 활성화 시작
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;
        
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        
        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                //충돌
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    private bool CheckObject()
    {
        //transform.TransformDirection(Vector3.forward)
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
            return true;
        return false;
    }

    public void HandChange(Hand _hand)
    {
        Debug.Log("6.5");
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnime = currentHand.anime;

        currentHand.transform.localPosition = Vector3.zero; //주의
        currentHand.gameObject.SetActive(true);
        Debug.Log("7.5");
    }
}
