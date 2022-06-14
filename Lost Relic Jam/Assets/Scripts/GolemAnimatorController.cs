using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Golems{
    public class GolemAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator golemAnim;
        //[SerializeField] private AudioSource _source; don't really know the difference 
        //[SerializeField] private ParticleSystem _footStep; will be added once i know how to work with sounds
        [SerializeField] private AudioClip[] _footStep;

        private IGolemBehaviour _golem;

        void Awake() => _golem = GetComponentInParent<IGolemBehaviour>();

        void Update() {
            if(_golem == null) return;

            golemAnim.SetTrigger("Idle");
            if(_golem.isPlayerNoticeable) golemAnim.SetTrigger("Walk");
            if(_golem.isPlayerInAttackRange) golemAnim.SetTrigger("Hit2");
            if(!_golem.isAlive) golemAnim.SetTrigger("Die");
        }
    }

}
