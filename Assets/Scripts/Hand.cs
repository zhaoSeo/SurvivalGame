﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName; //맨손 구분
    public float range; //공격 범위
    public int damage; //공격력
    public float workSpeed; //작업속도
    public float attackDelay; //딜레이
    public float attackDelayA; //공격 활성화 시점
    public float attackDelayB; //공격 비활성화 시점

    public Animator anime;
    //public BoxCollider boxCollider; 인지오차
}
