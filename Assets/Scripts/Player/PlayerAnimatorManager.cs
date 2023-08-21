using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MMS.PLAYER.Animation
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator animator;
        [SerializeField] private float directionDampTime = .25f;
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Forward", v);
            animator.SetFloat("Turn", h, directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}

