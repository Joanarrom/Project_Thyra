using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderHandler : MonoBehaviour
{
  public ThirdPersonController player;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Golpe a: " + other.name);
        player.OnWeaponHit(other);
    }
}
