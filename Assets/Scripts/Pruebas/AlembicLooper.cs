using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class AlembicLooper : MonoBehaviour
{
    private Component alembicPlayer;
    private float duration = 1.5f;
    private PropertyInfo currentTimeProp;

    void Start()
    {
        // Busca el componente AlembicStreamPlayer por nombre
        alembicPlayer = GetComponent("AlembicStreamPlayer");

        if (alembicPlayer != null)
        {
            var type = alembicPlayer.GetType();
            currentTimeProp = type.GetProperty("CurrentTime");
        }
        else
        {
            Debug.LogError("AlembicStreamPlayer no encontrado.");
        }
    }

    void Update()
    {
        if (alembicPlayer != null && currentTimeProp != null)
        {
            float time = Time.time % duration;
            currentTimeProp.SetValue(alembicPlayer, time);
        }
    }
}
