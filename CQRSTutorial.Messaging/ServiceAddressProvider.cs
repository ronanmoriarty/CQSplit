using System;

namespace CQRSTutorial.Messaging
{
    public class ServiceAddressProvider : IServiceAddressProvider
    {
        public string GetServiceAddressFor(Type messageType)
        {
            return GetServiceAddressFor(messageType.Name);
        }

        public string GetServiceAddressFor(Type consumerType, string stringToReplace, string stringToReplaceWith)
        {
            return GetServiceAddressFor(consumerType.Name).Replace(stringToReplace, stringToReplaceWith);
        }

        private string GetServiceAddressFor(string messageTypeName)
        {
            return ReplaceCapitalisedWordsWithUnderscoreSeparatedWords(messageTypeName);
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