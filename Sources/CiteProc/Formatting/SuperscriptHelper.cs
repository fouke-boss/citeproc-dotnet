using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CiteProc.Formatting
{
    internal class SuperscriptHelper
    {
        private static readonly Dictionary<char, string> SUPERSCRIPTS = null;
        private static readonly Regex SUPERSCRIPTS_REGEX = null;
        
        static SuperscriptHelper()
        {
            // create a replace list of superscript characters
            var superscripts = new Dictionary<char, string>();
            superscripts.Add('ª', "a");
            superscripts.Add('²', "2");
            superscripts.Add('³', "3");
            superscripts.Add('¹', "1");
            superscripts.Add('º', "o");
            superscripts.Add('ʰ', "h");
            superscripts.Add('ʱ', "ɦ");
            superscripts.Add('ʲ', "j");
            superscripts.Add('ʳ', "r");
            superscripts.Add('ʴ', "ɹ");
            superscripts.Add('ʵ', "ɻ");
            superscripts.Add('ʶ', "ʁ");
            superscripts.Add('ʷ', "w");
            superscripts.Add('ʸ', "y");
            superscripts.Add('ˠ', "ɣ");
            superscripts.Add('ˡ', "l");
            superscripts.Add('ˢ', "s");
            superscripts.Add('ˣ', "x");
            superscripts.Add('ˤ', "ʕ");
            superscripts.Add('ᴬ', "A");
            superscripts.Add('ᴭ', "Æ");
            superscripts.Add('ᴮ', "B");
            superscripts.Add('ᴰ', "D");
            superscripts.Add('ᴱ', "E");
            superscripts.Add('ᴲ', "Ǝ");
            superscripts.Add('ᴳ', "G");
            superscripts.Add('ᴴ', "H");
            superscripts.Add('ᴵ', "I");
            superscripts.Add('ᴶ', "J");
            superscripts.Add('ᴷ', "K");
            superscripts.Add('ᴸ', "L");
            superscripts.Add('ᴹ', "M");
            superscripts.Add('ᴺ', "N");
            superscripts.Add('ᴼ', "O");
            superscripts.Add('ᴽ', "Ȣ");
            superscripts.Add('ᴾ', "P");
            superscripts.Add('ᴿ', "R");
            superscripts.Add('ᵀ', "T");
            superscripts.Add('ᵁ', "U");
            superscripts.Add('ᵂ', "W");
            superscripts.Add('ᵃ', "a");
            superscripts.Add('ᵄ', "ɐ");
            superscripts.Add('ᵅ', "ɑ");
            superscripts.Add('ᵆ', "ᴂ");
            superscripts.Add('ᵇ', "b");
            superscripts.Add('ᵈ', "d");
            superscripts.Add('ᵉ', "e");
            superscripts.Add('ᵊ', "ə");
            superscripts.Add('ᵋ', "ɛ");
            superscripts.Add('ᵌ', "ɜ");
            superscripts.Add('ᵍ', "g");
            superscripts.Add('ᵏ', "k");
            superscripts.Add('ᵐ', "m");
            superscripts.Add('ᵑ', "ŋ");
            superscripts.Add('ᵒ', "o");
            superscripts.Add('ᵓ', "ɔ");
            superscripts.Add('ᵔ', "ᴖ");
            superscripts.Add('ᵕ', "ᴗ");
            superscripts.Add('ᵖ', "p");
            superscripts.Add('ᵗ', "t");
            superscripts.Add('ᵘ', "u");
            superscripts.Add('ᵙ', "ᴝ");
            superscripts.Add('ᵚ', "ɯ");
            superscripts.Add('ᵛ', "v");
            superscripts.Add('ᵜ', "ᴥ");
            superscripts.Add('ᵝ', "β");
            superscripts.Add('ᵞ', "γ");
            superscripts.Add('ᵟ', "δ");
            superscripts.Add('ᵠ', "φ");
            superscripts.Add('ᵡ', "χ");
            superscripts.Add('⁰', "0");
            superscripts.Add('ⁱ', "i");
            superscripts.Add('⁴', "4");
            superscripts.Add('⁵', "5");
            superscripts.Add('⁶', "6");
            superscripts.Add('⁷', "7");
            superscripts.Add('⁸', "8");
            superscripts.Add('⁹', "9");
            superscripts.Add('⁺', "+");
            superscripts.Add('⁻', "−");
            superscripts.Add('⁼', "=");
            superscripts.Add('⁽', "(");
            superscripts.Add('⁾', ")");
            superscripts.Add('ⁿ', "n");
            superscripts.Add('℠', "SM");
            superscripts.Add('™', "TM");
            superscripts.Add('㆒', "一");
            superscripts.Add('㆓', "二");
            superscripts.Add('㆔', "三");
            superscripts.Add('㆕', "四");
            superscripts.Add('㆖', "上");
            superscripts.Add('㆗', "中");
            superscripts.Add('㆘', "下");
            superscripts.Add('㆙', "甲");
            superscripts.Add('㆚', "乙");
            superscripts.Add('㆛', "丙");
            superscripts.Add('㆜', "丁");
            superscripts.Add('㆝', "天");
            superscripts.Add('㆞', "地");
            superscripts.Add('㆟', "人");
            superscripts.Add('ˀ', "ʔ");
            superscripts.Add('ˁ', "ʕ");
            superscripts.Add((char)1765, "و");
            superscripts.Add((char)1766, "ي");

            // done
            SUPERSCRIPTS = superscripts;
            SUPERSCRIPTS_REGEX = new Regex(string.Format("([{0}]+)", new string(SUPERSCRIPTS.Select(x => x.Key).ToArray())));
        }

        public static IEnumerable<string> SplitAndReplace(string text)
        {
            // find matches
            var matches = SUPERSCRIPTS_REGEX.Matches(text)
                .Cast<System.Text.RegularExpressions.Match>()
                .ToArray();

            // yield all
            int index = 0;
            foreach (var match in matches)
            {
                // output before
                yield return text.Substring(index, match.Index);

                // replace
                yield return string.Join("", match.Value.Select(c => SUPERSCRIPTS[c]));

                // next
                index = match.Index + match.Length;
            }

            // yield last
            yield return text.Substring(index);
        }
    }
}
