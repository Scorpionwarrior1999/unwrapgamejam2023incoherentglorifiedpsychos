using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RayFire
{
    [SelectionBase]
    [DisallowMultipleComponent]
    [AddComponentMenu ("RayFire/Rayfire Rigid Root")]
    [HelpURL ("http://rayfirestudios.com/unity-online-help/unity-rigid-root-component/")]
    public class RayfireRigidRoot : MonoBehaviour
    {
        public enum InitType
        {
            ByMethod = 0,
            AtStart  = 1
        }
        
        [Space (2)]
        public InitType initialization = InitType.AtStart;
        
        [Header ("  Simulation")]
        [Space (3)]
        
        public SimType simulationType = SimType.Dynamic;
        [Space (2)]
        public RFPhysic     physics    = new RFPhysic();
        [Space (2)]
        public RFActivation activation = new RFActivation();
        
        
        
        //public RFCollapse collapse;
        public RFFade       fading     = new RFFade();
        public RFReset      reset      = new RFReset();
        
        // Hidden
        public Transform     tm;
        public List<RFShard> inactiveShards;
        
        
        
        [HideInInspector] public List<RFRigid> rigids;
        
        [HideInInspector] public List<RFShard> shards;
        [HideInInspector] public RFCluster     cluster;
        
        // Particles
        [HideInInspector] public List<RayfireDebris> debrisList;
        [HideInInspector] public List<RayfireDust>   dustsList;
        
        /// /////////////////////////////////////////////////////////
        /// Common
        /// /////////////////////////////////////////////////////////
        
        void OnTransformChildrenChanged()
        {
            //Debug.Log ("changed");
        }
        
        // Awake
        void Awake()
        {
            if (initialization == InitType.AtStart)
            {
                AwakeMethods();
            }
            
            // TODO init shards initPos at init even if setup 
        }
        
        /// /////////////////////////////////////////////////////////
        /// Setup
        /// /////////////////////////////////////////////////////////
        
        // Awake ops
        void AwakeMethods()
        {
            // Create RayFire manager if not created
            RayfireMan.RayFireManInit();
            
            // Set components
            SetComponents();
            
            // Set shards components
            SetShards();
            
            // Set components for mesh / skinned mesh / clusters
            SetPhysics();
            
            // Setup list for activation
            SetInactive ();
            
            // Set Particle Components: Initialize, collect
            SetParticles();
            
            // Start all necessary coroutines
            StartAllCoroutines();
        }

        // Define basic components
        void SetComponents()
        {
            tm = GetComponent<Transform>();
        }
        
        // Set shards components
        void SetShards()
        { 
            // Get children
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < tm.childCount; i++)
                children.Add (tm.GetChild (i));

            // Set new cluster
            int id = 0;
            cluster = new RFCluster();
            for (int i = 0; i < children.Count; i++)
            {
                // Set id
                id++;
                
                // Check if already has rigid
                RayfireRigid rigid = children[i].gameObject.GetComponent<RayfireRigid>();
                
                // has own rigid
                if (rigid != null)
                {
                    // Init
                    rigid.Initialize();
                    
                    // Mesh
                    if (rigid.objectType == ObjectType.Mesh)
                    {
                        RFShard shard = new RFShard (children[i].transform, id);
                        shard.rigid = rigid;
                        cluster.shards.Add (shard);
                    }
                    
                    // Mesh Root
                    if (rigid.objectType == ObjectType.MeshRoot)
                    {
                        if (rigid.fragments.Count > 0)
                        {
                            for (int j = 0; j < rigid.fragments.Count; j++)
                            {
                                id += j;
                                RFShard shard = new RFShard (rigid.fragments[j].transform, id); // TODO Set if considering all shard ids
                                shard.rigid = rigid.fragments[j];
                                cluster.shards.Add (shard);
                            }
                        }
                    }
                    
                    // Connected Cluster TODO 
                    if (rigid.objectType == ObjectType.ConnectedCluster || rigid.objectType == ObjectType.NestedCluster)
                    {
                        RFShard shard = new RFShard (children[i].transform, id);
                        shard.rigid = rigid;
                        cluster.shards.Add (shard);
                    }
                }

                // Has no own rigid
                if (rigid == null)
                {
                    // Mesh
                    if (children[i].childCount == 0)
                    {
                        RFShard shard = new RFShard (children[i].transform, id);
                        shard.mf  = children[i].transform.GetComponent<MeshFilter>();
                        
                        // Has mesh
                        if (shard.mf != null && shard.mf.sharedMesh != null)
                        {
                            shard.rb  = children[i].transform.GetComponent<Rigidbody>();
                            shard.col = children[i].transform.GetComponent<Collider>();
                            cluster.shards.Add (shard);
                        }
                    }

                    // Mesh Root TODO
                    else if (children[i].childCount > 0)
                    {
                        if (IsNestedCluster (children[i]) == true)
                        {
                             // Nested
                        }
                        else
                        {
                            // Connected
                        }
                    }
                }
            }
        }

        // Define components
        void SetPhysics()
        {
            // Set density.
            float density     = RayfireMan.inst.materialPresets.Density (physics.materialType);
            float drag        = RayfireMan.inst.materialPresets.Drag (physics.materialType);
            float dragAngular = RayfireMan.inst.materialPresets.AngularDrag (physics.materialType);
            
            // Add Collider and Rigid body if has no Rigid component
            for (int i = 0; i < cluster.shards.Count; i++)
            {
                // Has no own rigid component: add collider and rigidbody
                if (cluster.shards[i].rigid == null)
                {
                    // Set mesh collier
                    if (cluster.shards[i].col == null && cluster.shards[i].mf != null)
                    {
                        MeshCollider col = cluster.shards[i].tm.gameObject.AddComponent<MeshCollider>();
                        col.sharedMesh        = cluster.shards[i].mf.sharedMesh;
                        col.convex            = true;
                        cluster.shards[i].col = col;
                    }

                    // Set Rigid body
                    if (cluster.shards[i].rb == null)
                        cluster.shards[i].rb = cluster.shards[i].tm.gameObject.AddComponent<Rigidbody>();
                    
                    // MeshCollider physic material preset. Set new or take from parent 
                    RFPhysic.SetColliderMaterial (this, cluster.shards[i]);
                    
                    // Set simulation
                    RFPhysic.SetSimulationType (cluster.shards[i].rb, simulationType, physics.useGravity);
                    
                    // Set density. After collider defined
                    RFPhysic.SetDensity (this, cluster.shards[i], density);

                    // Set drag properties
                    RFPhysic.SetDrag (cluster.shards[i], drag, dragAngular);
                }
            }
            
            // Set debris collider material
            RFPhysic.SetParticleColliderMaterial (debrisList);
            
            // Set material solidity and destructible
            physics.solidity     = physics.Solidity;
            physics.destructible = physics.Destructible;
        }
        
        // Setup inactive shards
        void SetInactive()
        {
            if (simulationType == SimType.Inactive || simulationType == SimType.Kinematic)
            {
                inactiveShards = new List<RFShard>();
                for (int i = 0; i < cluster.shards.Count; i++)
                    inactiveShards.Add (cluster.shards[i]);
            }
        }
        
        // Set Particle Components: Initialize, collect
        void SetParticles()
        {
            // Get all Debris and initialize
            if (HasDebris == false)
            {
                RayfireDebris[] debrisArray = GetComponents<RayfireDebris>();
                if (debrisArray.Length > 0)
                {
                    for (int i = 0; i < debrisArray.Length; i++)
                        debrisArray[i].Initialize();

                    debrisList = new List<RayfireDebris>();
                    for (int i = 0; i < debrisArray.Length; i++)
                        if (debrisArray[i].initialized == true)
                        {
                            debrisList.Add (debrisArray[i]);
                        }
                }
            }

            // Get all Dust and initialize
            if (HasDust == false)
            {
                RayfireDust[] dustArray = GetComponents<RayfireDust>();
                if (dustArray.Length > 0)
                {
                    for (int i = 0; i < dustArray.Length; i++)
                        dustArray[i].Initialize();

                    dustsList = new List<RayfireDust>();
                    for (int i = 0; i < dustArray.Length; i++)
                        if (dustArray[i].initialized == true)
                        {
                            dustsList.Add (dustArray[i]);
                        }
                }
            }
        }

        // Start all coroutines
        void StartAllCoroutines()
        {
            // Stop if static
            if (simulationType == SimType.Static)
                return;
            
            // Inactive
            if (gameObject.activeSelf == false)
                return;
            
            // Prevent physics cors
            if (physics.exclude == true)
                return;
            
            // Init inactive every frame update coroutine
            if (simulationType == SimType.Inactive || simulationType == SimType.Kinematic)
                StartCoroutine (InactiveCor());
        }
        
        // Prepare shards. Set bounds, set neibs
        static void SetShardsByRigids(RFCluster cluster, List<RayfireRigid> rigidList, ConnectivityType connectivity)
        {
            for (int i = 0; i < rigidList.Count; i++)
            {
                // Get mesh filter
                MeshFilter mf = rigidList[i].GetComponent<MeshFilter>();

                // Child has no mesh
                if (mf == null)
                    continue;

                // Create new shard
                RFShard shard = new RFShard(rigidList[i].transform, i);
                shard.cluster = cluster;
                shard.rigid   = rigidList[i];
                shard.uny     = rigidList[i].activation.unyielding;
                shard.col     = rigidList[i].physics.meshCollider;

                // Set faces data for connectivity
                if (connectivity == ConnectivityType.ByMesh)
                    RFTriangle.SetTriangles(shard, mf);

                // Collect shard
                cluster.shards.Add(shard);
            }
        }
        
        /// /////////////////////////////////////////////////////////
        /// Inactive
        /// /////////////////////////////////////////////////////////
        
        // Activation by velocity and offset
        IEnumerator InactiveCor ()
        {
            while (inactiveShards.Count > 0)
            {
                // Velocity activation
                if (activation.byVelocity > 0)
                {
                    for (int i = inactiveShards.Count - 1; i >= 0; i--)
                    {
                        if (inactiveShards[i].tm.hasChanged == true)
                            if (inactiveShards[i].rb.velocity.magnitude > activation.byVelocity)
                            {
                                Activate (inactiveShards[i]);
                                inactiveShards.RemoveAt (i);
                            }
                    }

                    // Stop 
                    if (inactiveShards.Count == 0)
                        yield break;
                }

                // Offset activation
                if (activation.byOffset > 0)
                {
                    for (int i = inactiveShards.Count - 1; i >= 0; i--)
                    {
                        if (inactiveShards[i].tm.hasChanged == true)
                            if (Vector3.Distance (inactiveShards[i].tm.position, inactiveShards[i].pos) > activation.byOffset)
                            {
                                Activate (inactiveShards[i]);
                                inactiveShards.RemoveAt (i);
                            }
                    }

                    // Stop 
                    if (inactiveShards.Count == 0)
                        yield break;
                }
                
                // Stop velocity
                for (int i = inactiveShards.Count - 1; i >= 0; i--)
                {
                    if (inactiveShards[i].tm.hasChanged == true)
                    {
                        inactiveShards[i].rb.velocity        = Vector3.zero;
                        inactiveShards[i].rb.angularVelocity = Vector3.zero;
                        inactiveShards[i].tm.hasChanged      = false;
                    }
                }
                
                // TODO Connectivity check if shards was activated: check only neibs of activated?
                // CheckConnectivity();
                
                
                
                yield return null;
            }
        }

        // Activate
        void Activate(RFShard shard)
        {
            // Set props
            if (shard.rb.isKinematic == true)
                shard.rb.isKinematic = false;
            shard.rb.useGravity  = true;
            
            // TODO Fade on activation
            if (fading.onActivation == true)
            {
                RFFade.Fade (this, shard);
            }
            
            // TODO Init particles on activation
            // RFParticles.InitActivationParticles(scr);


            // Add initial rotation if still TODO put in ui
            float val = 1.0f;
            if (shard.rb.angularVelocity == Vector3.zero)
                shard.rb.angularVelocity = new Vector3 (
                    Random.Range (-val, val), Random.Range (-val, val), Random.Range (-val, val));
        }
        
        /// /////////////////////////////////////////////////////////
        /// Static
        /// /////////////////////////////////////////////////////////
        
        // Check if root is nested cluster
        static bool IsNestedCluster (Transform trans)
        {
            for (int c = 0; c < trans.childCount; c++)
                if (trans.GetChild (c).childCount > 0)
                    return true;
            return false;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Getters
        /// /////////////////////////////////////////////////////////
        
        bool HasDebris { get { return debrisList != null && debrisList.Count > 0; } }
        bool HasDust { get { return dustsList != null && dustsList.Count > 0; } }
    }
    
    
    
    public class RFRigid
    {
        public enum RFRigidObjType
        {
            Mesh = 0,
            Conn = 1,
            Nest = 2
        }
        
    
        public RFRigidObjType tp;
        public Transform      tm;
        public Rigidbody      rb;
        public Collider       cl;
        
        // Init tm
        public Vector3 initScl;
        public Vector3 initPos;
        public Quaternion initRot;


        public float size;

    }
    
}
