using System;
using UnityEngine;
public class PlayerAttackController : MonoBehaviour
{
    // Add Attack Cooldown
    private int attackCooldown = 3;
    private bool _isAttacking = false;
    public void Start()
    {
        
    }

    public void Q(Vector3 src, Vector3 aim)
    {
        Projectile q = new Projectile(src);
        q.launch(aim);
    }
    
    public void W()
    {
        
    }

    public void E()
    {
        
    }

    public void R()
    {
        
    }

    public bool isAttacking()
    {
        return _isAttacking;
    }
    
}