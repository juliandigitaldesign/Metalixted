using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyLagCompensation : MonoBehaviourPun, IPunObservable
{

    private Rigidbody _rigidbody;

    private Vector3 _netPosition;
    private Quaternion _netRotation;
    private Vector3 _previusPos;

    public bool teleportIfFar;
    public float teleportIfFarDistance;

    [Header("Lerping [Experimental]")]
    public float smoothPos = 5.0f;
    public float smoothRot = 5.0f;

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
            _netPosition = (Vector3)stream.ReceiveNext();
            _netRotation = (Quaternion)stream.ReceiveNext();
            _rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPosition += (_rigidbody.velocity * lag);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine) return;

        _rigidbody.position = Vector3.Lerp(_rigidbody.position, _netPosition, smoothPos * Time.fixedDeltaTime);
        _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, _netRotation, smoothPos * Time.fixedDeltaTime);

        if (Vector3.Distance(_rigidbody.position, _netPosition) > teleportIfFarDistance) 
        {
            _rigidbody.position = _netPosition;
        }
    }
}
