namespace Guardian.Networking
{
    class PhotonApplication
    {
        public string Name;
        public string Id;

        public PhotonApplication(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }

        public class Fenglee : PhotonApplication
        {
            public Fenglee() : base("Fenglee", "f1f6195c-df4a-40f9-bae5-4744c32901ef") { }
        }

        public class AoTTG2 : PhotonApplication
        {
            public AoTTG2() : base("AoTTG/2", "5578b046-8264-438c-99c5-fb15c71b6744") { }
        }
    }
}
