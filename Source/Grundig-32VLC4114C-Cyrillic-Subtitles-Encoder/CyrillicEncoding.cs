namespace Grundig_32VLC4114C_Cyrillic_Subtitles_Encoder
{
    using System.Text;

    public class CyrillicEncoding : Encoding
    {
        private readonly Encoding currentEncoding;

        public CyrillicEncoding()
        {
            this.currentEncoding = Default;
        }

        public override string BodyName => this.currentEncoding.BodyName;

        public override string HeaderName => UTF8.HeaderName;

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return this.currentEncoding.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return this.currentEncoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return this.currentEncoding.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return this.currentEncoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
        {
            return this.currentEncoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return this.currentEncoding.GetMaxCharCount(byteCount);
        }
    }
}