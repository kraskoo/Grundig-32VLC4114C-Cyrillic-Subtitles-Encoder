namespace Grundig_32VLC4114C_Cyrillic_Subtitles_Encoder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class Encodings
    {
        private const string Cyrillic = "cyrillic";
        private static List<EncodingInfo> allEncodings;
        private static List<EncodingInfo> encodings;

        public static List<EncodingInfo> GetAllEncodings()
        {
            return allEncodings ?? (allEncodings = Encoding.GetEncodings().ToList());
        }

        public static List<EncodingInfo> GetCyrillicEncodings()
        {
            return encodings ?? (encodings = Encoding.GetEncodings().Where(
                                         e => e.DisplayName.ToLower().Contains(Cyrillic)
                                              || e.Name.ToLower().Contains(Cyrillic)).Concat(GetInfos(Encoding.UTF8))
                                     .ToList());
        }

        private static IEnumerable<EncodingInfo> GetInfos(params Encoding[] encodingParams)
        {
            foreach (var encoding in encodingParams)
                yield return GetInnerType<EncodingInfo>(encoding.CodePage, encoding.BodyName, encoding.EncodingName);
        }

        private static T GetInnerType<T>(params object[] ctorObjects)
        {
            var typeOf = typeof(T);
            var constructors = typeOf.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var ctor = constructors.FirstOrDefault(c => c.GetParameters().Length == ctorObjects.Length);
            return (T)ctor.Invoke(ctorObjects);
        }
    }
}