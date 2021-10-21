using System;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public class ClaimsInvalidException : Exception
    {
        public const string CLAIM_NOT_FOUND = "Claim not found.";
        public const string CLAIM_UNPROCESSABLE = "Claim unprocessable.";

        public ClaimsInvalidException(string claimName, string description)
        : base(GetMessage(claimName, description))
        {
            ClaimName = claimName;
            Description = description;
        }

        public string ClaimName { get; }
        public string Description { get; }

        public override string Message => GetMessage(ClaimName, Description);

        protected static string GetMessage(string claimName, string description) =>
            $"{claimName}: ${description}.";
    }
}
