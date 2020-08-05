namespace Assets.Scripts.Interaction.Vacuum
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class RailVacuumable : MonoBehaviour, ISuckable, IBlowable
    {
        public Vector3 fromPos, toPos;
        public float drag = 2.0f, mass = 0.7f;
        public bool constrainLocalX, constrainLocalZ;
        public int railLayer = 13;

        private Rigidbody _rb;

        void Start()
        {
            Initiate();
        }

        private void Initiate()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.drag = drag;
            _rb.mass = mass;

            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = railLayer;

            InstantiateBrakes();
        }

        public void Suck(Vector3 suckSource, float suckForce)
        {
            Vector3 dirToFrom = fromPos - transform.position;
            Vector3 dirToTo = toPos - transform.position;
            Vector3 dirToVacuumSource = suckSource - transform.position;

            float fromScore = Vector3.Angle(dirToFrom, dirToVacuumSource);
            float toScore = Vector3.Angle(dirToTo, dirToVacuumSource);

            if (fromScore == toScore)
                return;

            if (fromScore < toScore)
                _rb.AddForce((fromPos - transform.position).normalized * suckForce * Time.smoothDeltaTime);
            else
                _rb.AddForce((toPos - transform.position).normalized * suckForce * Time.smoothDeltaTime);
        }

        public void Blow(Vector3 blowSource, float blowForce)
        {
            Vector3 dirToFrom = fromPos - transform.position;
            Vector3 dirToTo = toPos - transform.position;
            Vector3 dirToVacuumSource = blowSource - transform.position;

            float fromScore = Vector3.Angle(dirToFrom, dirToVacuumSource);
            float toScore = Vector3.Angle(dirToTo, dirToVacuumSource);

            if (fromScore == toScore)
                return;

            if (fromScore > toScore)
                _rb.AddForce((fromPos - transform.position).normalized * blowForce * Time.smoothDeltaTime);
            else
                _rb.AddForce((toPos - transform.position).normalized * blowForce * Time.smoothDeltaTime);
        }

        private void FixedUpdate()
        {
            LocalConstraining();

        }

        private void LocalConstraining()
        {
            if (!constrainLocalX && !constrainLocalZ)
                return;

            Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);

            if (constrainLocalX)
                localVelocity.x = 0;

            if (constrainLocalZ)
                localVelocity.z = 0;

            _rb.velocity = transform.TransformDirection(localVelocity);
        }

        private void InstantiateBrakes()
        {
            GameObject parentObj;

            if (!GameObject.Find("RailSystem"))
            {
                parentObj = new GameObject("RailSystem");
            }
            else
            {
                parentObj = GameObject.Find("RailSystem");
            }

            Instantiate(Resources.Load("Prefabs/Brake"), fromPos, Quaternion.identity, parentObj.transform);
            Instantiate(Resources.Load("Prefabs/Brake"), toPos, Quaternion.identity, parentObj.transform);
        }

        [ExecuteInEditMode]
        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.75F);
            Gizmos.DrawWireSphere(fromPos, 0.1f);
            Gizmos.DrawWireSphere(toPos, 0.1f);
            Gizmos.DrawLine(fromPos, toPos);
        }
    }
}