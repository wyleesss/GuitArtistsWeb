using System.Security.Claims;

namespace GuitArtistsWeb.Helpers
{
    public static class ClaimUpdateHelper
    {
        public static ClaimsPrincipal GetNewIdentity(ClaimsIdentity? identity, string claimType, string newClaimValue)
        {
            var newIdentity = new ClaimsIdentity(identity);
            var oldClaim = newIdentity.FindFirst(claimType);

            newIdentity.RemoveClaim(oldClaim);
            newIdentity.AddClaim(new Claim(claimType, newClaimValue));

            ClaimsPrincipal newPrincipal = new ClaimsPrincipal(newIdentity);

            return newPrincipal;
        }
    }
}
