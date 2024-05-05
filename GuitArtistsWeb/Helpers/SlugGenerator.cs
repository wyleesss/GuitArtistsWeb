using System.Text;

namespace GuitArtistsWeb.Helpers
{
    public class SlugGenerator
    {
        public static string Generate(string str)
        {
            Dictionary<char, string> ukrainianToEnglish = new Dictionary<char, string>
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "h"}, {'ґ', "g"},
            {'д', "d"}, {'е', "e"}, {'є', "ye"}, {'ж', "zh"}, {'з', "z"},
            {'и', "y"}, {'і', "i"}, {'ї', "yi"}, {'й', "y"}, {'к', "k"},
            {'л', "l"}, {'м', "m"}, {'н', "n"}, {'о', "o"}, {'п', "p"},
            {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"}, {'ф', "f"},
            {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shch"},
            {'ь', ""}, {'ю', "yu"}, {'я', "ya"}
        };

            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                // Перевіряємо, чи є українська літера
                if (ukrainianToEnglish.ContainsKey(char.ToLower(c)))
                {
                    string replacement = ukrainianToEnglish[char.ToLower(c)];
                    sb.Append(replacement);
                }
                else if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
                else if (c == ' ')
                {
                    // Замінюємо пробіли на дефіси
                    sb.Append('-');
                }
            }

            // Перетворюємо на нижній регістр
            string slug = sb.ToString().ToLower();
            return slug;
        }
    }
}
