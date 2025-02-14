// Copyright (C) 2021 jmh
// SPDX-License-Identifier: GPL-3.0-only

using System.Threading.Tasks;

namespace AuthenticatorPro.Shared.Data.Backup.Converter
{
    public abstract class BackupConverter
    {
        public enum BackupPasswordPolicy
        {
            Never, Always, Maybe
        }
        
        protected readonly IIconResolver _iconResolver;

        protected BackupConverter(IIconResolver iconResolver)
        {
            _iconResolver = iconResolver; 
        }
        
        public abstract BackupPasswordPolicy PasswordPolicy { get; }
        public abstract Task<Backup> Convert(byte[] data, string password = null);
    }
}