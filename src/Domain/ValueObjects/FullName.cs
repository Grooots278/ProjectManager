using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string? MiddleName { get; }

        public FullName(string firstName, string lastName, string? middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public static FullName Create(string firstName, string lastName, string? middleName = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required", nameof(lastName));

            return new FullName(firstName.Trim(), lastName.Trim(), middleName?.Trim());
        }

        public string GetFullName() => MiddleName != null
            ? $"{LastName} {FirstName} {MiddleName}"
            : $"{LastName} {FirstName}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return MiddleName ?? string.Empty;
        }
    }
}
