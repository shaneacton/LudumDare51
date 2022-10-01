using UnityEngine;
struct MovementData
{
    public Vector3 position;
    public Quaternion rotation;
    public bool shoot;


    public MovementData(Vector3 pos, Quaternion rot, bool shoot)
    {
        this.position = pos;
        this.rotation = rot;
        this.shoot = shoot;
    }
}