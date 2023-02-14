using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RayFire
{
    [AddComponentMenu ("RayFire/Rayfire Unyielding")]
    [HelpURL ("http://rayfirestudios.com/unity-online-help/unity-unyielding-component/")]
    public class RayfireUnyielding : MonoBehaviour
    {
        public enum RFUnyType
        {
            AtStart = 0,
            ByMethod  = 3
        }
        
        [Header ("  Properties")]
        [Space (3)]
        
        public RFUnyType initialize = RFUnyType.ByMethod;
        [Space (2)]
        
        [Header ("  Gizmo")]
        [Space (3)]
        
        [Tooltip ("Unyielding gizmo center.")]
        public Vector3 centerPosition;
        [Space (2)]
        
        [Tooltip ("Unyielding gizmo size.")]
        public Vector3 size = new Vector3(1f,1f,1f);
       
        [HideInInspector] public List<RayfireRigid> rigidList;

        // Hidden
        [HideInInspector] public bool    showGizmo = true;
        [HideInInspector] public bool    showCenter;
        [HideInInspector] public int     id;
        
        /// /////////////////////////////////////////////////////////
        /// Common
        /// /////////////////////////////////////////////////////////
        
        // Start
        void Start()
        {
            if (initialize == RFUnyType.AtStart)
            {
                Initialize();
            }
        }
        
        // Set uny state for mesh root rigids. Used by Mesh Root. Can be used for cluster shards
        public void SetUnyByOverlap(RayfireRigid rigid)
        {
            if (enabled == false)
                return;
           
            if (initialize == RFUnyType.AtStart)
                return;
            
            // Get target mask TODO check fragments layer
            // int mask = 1 << scr.gameObject.layer;
            
            // Get box overlap colliders
            Collider[] colliders = Physics.OverlapBox (transform.TransformPoint (centerPosition), Extents, transform.rotation, 1 << rigid.gameObject.layer);
            
            // Check with mesh object
            if (rigid.objectType == ObjectType.Mesh)
            {
                if (rigid.physics.meshCollider != null)
                    if (colliders.Contains (rigid.physics.meshCollider) == true)
                        RFActivation.SetUnyState (rigid, id);
            }

            // Check with mesh root object
            else if (rigid.objectType == ObjectType.MeshRoot)
            {
                for (int i = 0; i < rigid.fragments.Count; i++)
                    if (rigid.fragments[i].physics.meshCollider != null)
                        if (colliders.Contains (rigid.fragments[i].physics.meshCollider) == true)
                            RFActivation.SetUnyState (rigid.fragments[i], id);
            }

            // Check with connected cluster
            else if (rigid.objectType == ObjectType.ConnectedCluster)
            {
                for (int i = 0; i < rigid.physics.clusterColliders.Count; i++)
                    if (rigid.physics.clusterColliders[i] != null)
                        if (colliders.Contains (rigid.physics.clusterColliders[i]) == true)
                            rigid.clusterDemolition.cluster.shards[i].uny = true;
            }
        }
        
        // Set uny state
        void Initialize ()
        {
            if (enabled == false)
                return;

            // Register in manager
            Register();
            
            // Get target mask TODO check fragments layer
            // int mask = 1 << scr.gameObject.layer;
            
            // Get box overlap colliders
            Collider[] colliders = Physics.OverlapBox (transform.TransformPoint (centerPosition), Extents, transform.rotation);

            // Set state for overlapped rigids
            SetUnyByColliders (colliders);
        }
        
        // Set uny state. Could be used only at start after all rigid object got colliders
        void SetUnyByColliders (Collider[] colliders)
        {
            // Get rigids
            if (rigidList == null)
                rigidList = new List<RayfireRigid>();
            else
                rigidList.Clear();
            
            // Collect TODO get shard's cluster rigid
            for (int i = 0; i < colliders.Length; i++)
            {
                RayfireRigid rigid = colliders[i].GetComponent<RayfireRigid>();
                if (rigid != null)
                    if (rigidList.Contains (rigid) == false)
                        rigidList.Add (rigid);
            }
            
            // Set this uny state
            SetUnyRigids (this, rigidList);
        }

        // Activate inactive\kinematic shards/fragments
        public void Activate()
        {
            
            
        }
        
        /// /////////////////////////////////////////////////////////
        /// Static
        /// /////////////////////////////////////////////////////////
        
        // Set this uny state
        static void SetUnyRigids (RayfireUnyielding uny, List<RayfireRigid> rigids)
        {
            if (rigids.Count > 0)
                for (int i = 0; i < rigids.Count; i++)
                    RFActivation.SetUnyState (rigids[i], uny.id);
        }
        
        /// /////////////////////////////////////////////////////////
        /// Manager register
        /// /////////////////////////////////////////////////////////
        
        // Register in manager
        void Register()
        {
            RFUny uny = new RFUny();
            uny.id       = GetUnyId();
            uny.scr      = this;
            uny.size     = Extents;

            uny.center   = transform.TransformPoint (centerPosition);
            uny.rotation = transform.rotation;

            // Add in all uny list
            RayfireMan.inst.unyList.Add (uny);

            // Save uny id to this id
            id = uny.id;
        }
        
        // Get uniq id
        static int GetUnyId()
        {
            return RayfireMan.inst.unyList.Count + 1;
        }
        
        // Get final extents
        public Vector3 Extents
        {
            get
            {
                Vector3 ext = size / 2f;
                ext.x *= transform.localScale.x;
                ext.y *= transform.localScale.y;
                ext.z *= transform.localScale.z;
                return ext;
            }
        }
    }

    [Serializable]
    public class RFUny
    {
        public int               id;
        public RayfireUnyielding scr;
        
        public Vector3    size;
        public Vector3    center;
        public Quaternion rotation;
    }
}