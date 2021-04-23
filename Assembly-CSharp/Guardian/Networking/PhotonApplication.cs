namespace Guardian.Networking
{
    class PhotonApplication
    {
        public static PhotonApplication Fenglee = new PhotonApplication("Fenglee", "f1f6195c-df4a-40f9-bae5-4744c32901ef");
        public static PhotonApplication AoTTG2 = new PhotonApplication("AoTTG-2", "5578b046-8264-438c-99c5-fb15c71b6744");
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
