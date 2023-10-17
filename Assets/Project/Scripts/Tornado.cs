using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public bool attack;
    private float velocidad = 5f;
    private Transform objetivo;

    void Start()
    {
        objetivo = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 nuevaPosicion = transform.position;

        nuevaPosicion.x = objetivo.position.x;
        nuevaPosicion.z = objetivo.position.z;

        transform.position = Vector3.MoveTowards(transform.position, nuevaPosicion, velocidad * Time.deltaTime);
    }
}
