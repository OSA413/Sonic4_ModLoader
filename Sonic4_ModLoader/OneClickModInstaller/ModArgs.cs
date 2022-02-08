namespace OneClickModInstaller {
    public class ModArgs {
        public readonly string Path;
        public readonly string Type;
        public readonly int Id;

        public ModArgs(string path, string type=null, int id=0) {
            Path = path;
            Type = type;
            Id = id;
        }
    }
}