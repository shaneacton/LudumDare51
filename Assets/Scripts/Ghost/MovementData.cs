using UnityEngine;

enum AttackType { Nothing, Shoot, Lazer }
struct MovementData
{
    public Vector3 position;
    public Quaternion rotation;
    public AttackType attackType;


    public MovementData(Vector3 pos, Quaternion rot, AttackType shoot)
    {
        this.position = pos;
        this.rotation = rot;
        this.attackType = shoot;
    }
}