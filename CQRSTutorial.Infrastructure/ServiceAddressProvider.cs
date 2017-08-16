using System;

namespace CQRSTutorial.Infrastructure
{
    public class ServiceAddressProvider : IServiceAddressProvider
    {
        public string GetServiceAddressFor<TRequest>()
        {
            return GetServiceAddressFor(typeof(TRequest).Name);
        }

        private string GetServiceAddressFor(string typeName)
        {
            return ReplaceCapitalisedWordsWithUnderscoreSeparatedWords(typeName);
        }

        private string ReplaceCapitalisedWordsWithUnderscoreSeparatedWords(string typeName)
        {
            string returnValue = null;
            var isFirstCharacter = true;
            foreach (var @char in typeName)
            {
                var replacementCharacters = GetReplacementCharacters(@char, isFirstCharacter);
                returnValue += replacementCharacters;
                isFirstCharacter = false;
            }

            if (returnValue != null && returnValue.StartsWith("i_"))
            {
                returnValue = returnValue.Substring(2);
            }

            return returnValue;
        }

        private static string GetReplacementCharacters(char characterToReplace, bool characterToReplaceIsTheFirstCharacter)
        {
            string replacementCharacters;
            if (characterToReplaceIsTheFirstCharacter)
            {
                replacementCharacters = Convert.ToString(char.ToLower(characterToReplace));
            }
            else if (char.IsUpper(characterToReplace))
            {
                replacementCharacters = $"_{char.ToLower(characterToReplace)}";
            }
            else
            {
                replacementCharacters = Convert.ToString(characterToReplace);
            }
            return replacementCharacters;
        }
    }
}