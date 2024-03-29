﻿using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class AppSettings
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string Value { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string SettingGroup { get; set; } = string.Empty;
    }
    static public class AppSettingsAccess
    {
        static public int TwitterKey { get; set; } = 0;
        static public int TwitterSecret { get; set; } = 1;
        static public int AllowImages { get; set; } = 2;
        static public int FacebookAccessToken { get; set; } = 3;
        static public int FacebookAppId { get; set; } = 4;
        static public int TwilioAuthToken { get; set; } = 5;
        static public int TwilioSID { get; set; } = 6;
    }
}
