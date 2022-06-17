using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golems{
    public interface IGolemBehaviour
    {
        public bool isPlayerInAttackRange {get;}
        public bool isPlayerNoticeable { get;}
        public bool isAlive {get;}
        public bool hasAttacked {get;}

        public float timeBetweenAttacks {get;}
        public float noticeRange {get;}
        public float attackRange {get;}

        public LayerMask whatIsPlayer {get; }
        public LayerMask whatIsGround {get; }

        public Transform player {get;}


    }
}
