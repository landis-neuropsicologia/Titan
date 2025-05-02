namespace Titan.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly string key = "PhoneNumber_";
    private static readonly Regex InternationalPhoneRegex = new(@"^\+(?:[0-9] ?){6,14}[0-9]$", RegexOptions.Compiled);
    private static readonly Regex LocalPhoneRegex = new(@"^(?:[0-9] ?){8,14}[0-9]$", RegexOptions.Compiled);

    #region Properties

    public string Value { get; }

    public string CountryCode { get; }

    public string NationalNumber { get; }

    public string NationalNumberClear { get; }

    public bool IsInternational { get; }

    #endregion

    #region C'tor

    private PhoneNumber(string value, string countryCode, string nationalNumber, bool isInternational)
    {
        Value = value;
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
        NationalNumberClear = nationalNumber.Replace(" ", string.Empty);
        IsInternational = isInternational;
    }

    #endregion

    #region Factory

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException($"{GetResourceString($"{key}Empty", CultureInfo.CurrentCulture)}", nameof(phoneNumber));

        // Remove all non-digit characters except the leading +
        string cleaned = phoneNumber.Trim();
        bool isInternational = cleaned.StartsWith("+");

        // Validate the format
        if (isInternational)
        {
            if (!InternationalPhoneRegex.IsMatch(cleaned))
                throw new ArgumentException($"{GetResourceString($"{key}InternationalNumberInvalid", CultureInfo.CurrentCulture)}", nameof(phoneNumber));
            
            string countryCode = string.Empty;
            string nationalNumber = string.Empty;

            if (phoneNumber.Length == 12)
            {
                countryCode = Regex.Match(cleaned, @"^\+([0-9]{1})").Groups[1].Value;
                nationalNumber = Regex.Replace(phoneNumber.Substring(countryCode.Length + 1).Trim(), @"(\d{3})(\d{3})(\d{4})", "$1 $2 $3"); 
            }
            else if (phoneNumber.Length == 13)
            {
                countryCode = Regex.Match(cleaned, @"^\+([0-9]{1,3})").Groups[1].Value;
                nationalNumber = Regex.Replace(phoneNumber.Substring(countryCode.Length + 1).Trim(), @"(\d{3})(\d{3})(\d{3})", "$1 $2 $3"); 

                if (nationalNumber.StartsWith("0"))
                {
                    countryCode = Regex.Match(cleaned, @"^\+([0-9]{1,2})").Groups[1].Value;
                    nationalNumber = Regex.Replace(phoneNumber.Substring(countryCode.Length + 1).Trim(), @"(\d{2})(\d{4})(\d{4})", "$1 $2 $3");
                }
            }
            else if (phoneNumber.Length == 14)
            {
                countryCode = Regex.Match(cleaned, @"^\+([0-9]{1,2})").Groups[1].Value;
                nationalNumber = Regex.Replace(phoneNumber.Substring(countryCode.Length + 1).Trim(), @"(\d{2})(\d{5})(\d{4})", "$1 $2 $3");
            }
            else if (phoneNumber.Length == 16)
            {
                countryCode = Regex.Match(cleaned, @"^\+([0-9]{1,3})").Groups[1].Value;
                nationalNumber = Regex.Replace(phoneNumber.Substring(countryCode.Length + 1).Trim(), @"(\d{2})(\d{4})(\d{4})", "$1 $2 $3");
            }
            else
            {
                // Extract country code (assuming it's the first 1-3 digits after the +)
                countryCode = Regex.Match(cleaned, @"^\+([0-9]{1,3})").Groups[1].Value;
                nationalNumber = phoneNumber.Substring(countryCode.Length + 1).Trim();
            }

            countryCode = $"+{countryCode}";

            return new PhoneNumber(cleaned, countryCode, nationalNumber, true);
        }
        else
        {
            // For local numbers, we don't know the country code
            if (!LocalPhoneRegex.IsMatch(cleaned))
                throw new ArgumentException($"{GetResourceString($"{key}NationalNumberInvalid", CultureInfo.CurrentCulture)}", nameof(phoneNumber));

            if (phoneNumber.Length == 11)
            {
                cleaned = Regex.Replace(phoneNumber.Trim(), @"(\d{2})(\d{5})(\d{4})", "$1 $2 $3");
            }
            else
            {
                cleaned = Regex.Replace(phoneNumber.Trim(), @"(\d{2})(\d{4})(\d{4})", "$1 $2 $3");
            }
                
            return new PhoneNumber(cleaned, string.Empty, cleaned, false);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Formats the phone number in E.164 format if it's international
    /// </summary>
    public string ToE164Format()
    {
        if (!IsInternational)
            throw new InvalidOperationException(GetResourceString($"{key}ToE164Format", CultureInfo.CurrentCulture));

        return $"{CountryCode}{NationalNumberClear}";
    }

    /// <summary>
    /// Formats the phone number with standard spacing
    /// </summary>
    public string ToFormattedString()
    {
        if (IsInternational)
        {
            // Group digits with spaces for readability
            return $"{ CountryCode } { NationalNumber }";
        }

        return FormatNationalNumber(NationalNumber);
    }

    private string FormatNationalNumber(string number)
    {
        number = number.Replace(" ", string.Empty);

        // Simple formatting: groups of 2-3 digits with spaces
        if (number.Length <= 4)
            return number;

        if (number.Length <= 7)
            return string.Concat(number.AsSpan(0, number.Length - 4), " ", number.AsSpan(number.Length - 4));

        // For longer numbers, create more groups
        string result = number;

        // Insert space between area code and subscriber number
        if (number.Length >= 10)
        {
            return NationalNumber;
        }

        // Insert space in the middle of subscriber number for readability
        int midPoint = result.Length - 4;

        return string.Concat(result.AsSpan(0, midPoint), " ", result.AsSpan(midPoint));
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (PhoneNumber)obj;

        // Compare the normalized form (no spaces, with country code if available)
        string normalizedThis = IsInternational ? ToE164Format() : NationalNumber;
        string normalizedOther = other.IsInternational ? other.ToE164Format() : other.NationalNumber;

        return string.Equals(normalizedThis, normalizedOther, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        // Use the same normalized form for hash code
        string normalized = IsInternational ? ToE164Format() : NationalNumber;
        return normalized.GetHashCode();
    }

    #endregion
}