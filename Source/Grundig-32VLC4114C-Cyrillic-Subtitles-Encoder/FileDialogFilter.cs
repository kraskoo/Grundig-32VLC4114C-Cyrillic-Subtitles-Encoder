namespace Grundig_32VLC4114C_Cyrillic_Subtitles_Encoder
{
    public class FileDialogFilter
    {
        public FileDialogFilter(string name, string extension)
        {
            this.Name = name;
            this.Extension = extension;
        }

        public string Extension { get; }

        public string Name { get; }

        public override string ToString()
        {
            return $"{this.Name} (*{this.Extension})|*{this.Extension}";
        }
    }
}