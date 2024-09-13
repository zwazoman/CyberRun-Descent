using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace SimpleVFXs
{
    [ExecuteAlways]

    public class Demo_Shield : MonoBehaviour
    {
        [SerializeField] shieldVisual _shieldVisualScript;
        [SerializeField][Range(0, MaxHP)] float _HP;
        const float MaxHP = 80f;

        float _TimeSinceLastHit;
        bool _isShieldActive = false;

        public void Take20Damage()
        {
            _HP -= 20; //takes 20 damage
            _TimeSinceLastHit = Time.time; //resets regeneration countdown

            _shieldVisualScript.SetHealthValue(_HP / MaxHP, true); //applies new HPs to the visuals and plays the hit animation
            if (_HP <= 0) BreakShield(); //breaks the shield if the HP falls below 0
        }

        public void EnableShield()
        {
            if (_isShieldActive) return;
            _isShieldActive = true;

            //spawns the shield's vfx and fills up HP
            _HP = MaxHP;
            _shieldVisualScript.ActivateShield();
            _shieldVisualScript.SetHealthValue(_HP / MaxHP);
        }

        public void DisableShield()
        {
            if (!_isShieldActive) return;
            _isShieldActive = false;

            //disables the shield's vfx
            _shieldVisualScript.DeactivateShield();
        }

        public void BreakShield()
        {
            if (!_isShieldActive) return;

            _isShieldActive = false;
            _shieldVisualScript.PlayBreakAnimation();
        }

        private void Update()
        {
            if (!_isShieldActive) return;

            //HP regeneration when the shield hasn't been hit for 1.2 seconds
            if (Time.time - _TimeSinceLastHit > 1.2f)
            {
                _HP = Mathf.Min(_HP + 50 * Time.deltaTime,MaxHP);
                _shieldVisualScript.SetHealthValue(_HP / MaxHP);
            }
        }

        private void OnValidate()
        {
            if(_shieldVisualScript == null) _shieldVisualScript = GetComponent<shieldVisual>();

            //applies HP to the shield visuals when using the slider
            _shieldVisualScript.SetHealthValue(_HP / MaxHP, false);
        }
    }

}
