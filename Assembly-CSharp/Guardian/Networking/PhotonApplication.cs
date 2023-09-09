namespace Guardian.Networking
{
    class PhotonApplication
    {
        public static PhotonApplication AoTTG2 = new PhotonApplication("AoTTG-2", string.Empty, 0);
        public static PhotonApplication Guardian = new PhotonApplication("Guardian", "b92ae2ae-b815-4f37-806a-58b4f58573ff", 1);
        public static PhotonApplication Custom = new PhotonApplication("Custom", string.Empty, 2);
        public static PhotonApplication AoTSkins = new PhotonApplication("AoT Skins", "b3d7a26b-9163-40ed-8253-41a6ee764195", 3);
        public static PhotonApplication RankedCommunity = new PhotonApplication("Ranked Community", "5bef7456-a8c0-41cb-976a-b4c71a3d33fa", 4);
        public static PhotonApplication Fenglee2016 = new PhotonApplication("Fenglee [2016 BETA]", "3a6f1e7b-f270-44ce-9017-345c5aa8e246", 5);
        public static PhotonApplication ProjectVoltage = new PhotonApplication("Project Voltage", "55ab2405-6c80-4bf9-aff8-3e53f64c36cf", 6);
        public static PhotonApplication AoTTG2Legacy = new PhotonApplication("AoTTG-2 [DEAD]", "5578b046-8264-438c-99c5-fb15c71b6744", 7);
        public static PhotonApplication Fenglee = new PhotonApplication("Fenglee [DEAD]", "f1f6195c-df4a-40f9-bae5-4744c32901ef", 8);
        public static PhotonApplication Temporary = new PhotonApplication("TEMPORARY", "e3341a7b-1cb9-4b6c-90e4-39b796e52876", 9);

        public readonly string Name;
        public string Id;

        private readonly int Index;

        public PhotonApplication(string name, string id, int index)
        {
            this.Name = name;
            this.Id = id;
            this.Index = index;
        }

        public static PhotonApplication GetNext(PhotonApplication current)
        {
            return current.Index switch
            {
                0 => Guardian,
                1 => Custom,
                2 => AoTSkins,
                3 => RankedCommunity,
                4 => Fenglee2016,
                5 => ProjectVoltage,
                6 => AoTTG2Legacy,
                7 => Fenglee,
                8 => Temporary,
                9 => AoTTG2,
                _ => Custom
            };
        }
    }
}
