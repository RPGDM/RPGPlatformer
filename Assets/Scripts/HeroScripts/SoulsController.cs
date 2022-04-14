using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsController : MonoBehaviour
{
    public void Replenishment(int attachment)
    {
        GetComponent<Player>().Balance += attachment;
    }
    public void WriteOff(int price)
    {
        if (price <= GetComponent<Player>().Balance)
        {
            GetComponent<Player>().Balance -= price;
        }
    }
}
