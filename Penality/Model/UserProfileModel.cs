using System;

namespace MotiSectorAPI.Model
{
    public class UserProfileModel
    {
        public string FullName { get; set; }
        public string[] Roles { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }
        public int PasswordExpiryRemainingDays { get; set; }
        public int MinPasswordLength { get; set; }
        public int MinNumbersLength { get; set; }
        public int MinUpperCaseLength { get; set; }
        public int MaxPasswordLength { get; set; }
        public int MinSpecialCharactersLength { get; set; }
        public string MobileNo { get; set; }

        public string IMEINumber { get; set; }      

        public int LocationCode { get; set; }
        public string LocationName { get; set; }
        //public bool ForcePasswordChange { get; set; }
    }
}