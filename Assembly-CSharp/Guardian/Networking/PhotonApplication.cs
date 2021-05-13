namespace Guardian.Networking
{
    class PhotonApplication
    {
        public static PhotonApplication Custom = new PhotonApplication("Custom", string.Empty);

        public string Name;
        public string Id;

        public PhotonApplication(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
