namespace SchoolMgmtAPI.Models.DbModel
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; }           // store hash, NOT raw token
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByTokenHash { get; set; } // hash of the new token if rotated
        public int UserId { get; set; }                 // reference to user (adjust type to your user key)
        public bool IsActive => Revoked == null && DateTime.UtcNow < Expires;
    }
}
