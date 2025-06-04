using System;
using System.Security.Cryptography;

/// <summary>
/// clsRandomStr 的摘要描述
/// </summary>
public class clsRandomStr
{
    private static int DEFAULT_MIN_STRING_LENGTH = 6;
    private static int DEFAULT_MAX_STRING_LENGTH = 12;
    private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static string PASSWORD_CHARS_NUMERIC = "23456789";

    /// <summary>
    /// 產生亂數
    /// </summary>
    /// <returns>
    /// 產生亂數
    /// </returns>
    /// <remarks>
    /// </remarks>
    public static string Generate()
    {
        return Generate(DEFAULT_MIN_STRING_LENGTH,
                        DEFAULT_MAX_STRING_LENGTH);
    }

    /// <summary>
    /// 產生亂數
    /// </summary>
    /// <param name="minLength">
    /// 最短字數
    /// </param>
    /// <param name="maxLength">
    /// 最長字數
    /// </param>
    /// <returns>
    /// 產生亂數
    /// </returns>
    /// <remarks>
    /// </remarks>
    public static string Generate(int minLength,
                                  int maxLength)
    {
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;
        char[][] charGroups = new char[][] 
        {
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray()
        };
        int[] charsLeftInGroup = new int[charGroups.Length];
        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;
        int[] leftGroupsOrder = new int[charGroups.Length];
        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;
        byte[] randomBytes = new byte[4];
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomBytes);
        int seed = (randomBytes[0] & 0x7f) << 24 |
                    randomBytes[1] << 16 |
                    randomBytes[2] << 8 |
                    randomBytes[3];
        Random random = new Random(seed);
        char[] password = null;
        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];
        int nextCharIdx;// Index of the next character to be added to password.
        int nextGroupIdx;// Index of the next character group to be processed.
        int nextLeftGroupsOrderIdx;// Index which will be used to track not processed character groups.
        int lastCharIdx;// Index of the last non-processed character in a group.
        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;// Index of the last non-processed group.
        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                                                     lastLeftGroupsOrderIdx);
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);
            password[i] = charGroups[nextGroupIdx][nextCharIdx];
            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] =
                                          charGroups[nextGroupIdx].Length;
            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] =
                                charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
                }
                charsLeftInGroup[nextGroupIdx]--;
            }
            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }
                lastLeftGroupsOrderIdx--;
            }
        }
        return new string(password);
    }
}