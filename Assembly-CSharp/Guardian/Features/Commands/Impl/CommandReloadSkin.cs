using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandReloadSkin : Command
    {
        public CommandReloadSkin() : base("reloadskins", new string[] { "rlskins", "rlskin" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Reloading skins...");

            FengGameManagerMKII.LinkHash = new ExitGames.Client.Photon.Hashtable[5]
            {
                new ExitGames.Client.Photon.Hashtable(),
                new ExitGames.Client.Photon.Hashtable(),
                new ExitGames.Client.Photon.Hashtable(),
                new ExitGames.Client.Photon.Hashtable(),
                new ExitGames.Client.Photon.Hashtable()
            };

            if (PhotonNetwork.isMasterClient)
            {
                foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
                {
                    if (titan.abnormalType == TitanClass.Punk)
                    {
                        titan.GetComponent<TITAN_SETUP>().SetPunkHair2();
                    }
                    else
                    {
                        titan.GetComponent<TITAN_SETUP>().SetHair2();
                    }

                    titan.LoadSkin();
                }

                FengGameManagerMKII.Instance.LoadSkin();
            }
            else if (GameHelper.IsPT(PhotonNetwork.player))
            {
                TITAN titan = GameHelper.GetPT(PhotonNetwork.player);

                if (titan.abnormalType == TitanClass.Punk)
                {
                    titan.GetComponent<TITAN_SETUP>().SetPunkHair2();
                }
                else
                {
                    titan.GetComponent<TITAN_SETUP>().SetHair2();
                }
                titan.LoadSkin();
            }

            if (!GameHelper.IsPT(PhotonNetwork.player))
            {
                GameHelper.GetHero(PhotonNetwork.player).LoadSkin();
            }

            irc.AddLine("Skins reloaded.");
        }
    }
}
