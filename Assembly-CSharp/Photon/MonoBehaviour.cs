namespace Photon
{
    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        private PhotonView _photonView;
        public PhotonView photonView
        {
            get
            {
                if (_photonView == null)
                {
                    _photonView = PhotonView.Get(this);
                }
                return _photonView;
            }
            set
            {
                _photonView = value;
            }
        }
    }
}
