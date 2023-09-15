using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player")]
public class PlayerData : ScriptableObject
{
    public int vidas;           // vidas actuales
    public int nivelMax;        // niveles superados
    public int raciones;        // raciones actuales
    public int racionesMax;     // maximo de raciones posibles a guardar
    public int poderActivo;     // poder actual seleccionado
    public float vida;          // vida actual
    public float vidaMax;       // vida al 100%
    public float racion;        // puntos de vida que recupera una racion
    public float ataqueLigero;  // puntos de da�o del ataque ligero
    public float ataquePesado;  // puntos de da�o del ataque pesado
    public float poder;         // puntos de da�o del poder seleccionado 
}
