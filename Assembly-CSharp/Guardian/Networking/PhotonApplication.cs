namespace Guardian.Networking
{
    class PhotonApplication
    {
        public static PhotonApplication AoTTG2 = new PhotonApplication("AoTTG-2", string.Empty);
        public static PhotonApplication Custom = new PhotonApplication("Custom", string.Empty);
        public static PhotonApplication Guardian = new PhotonApplication("Guardian", "b92ae2ae-b815-4f37-806a-58b4f58573ff");

        public readonly string Name;
        public string Id;

        public PhotonApplication(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
