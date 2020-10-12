using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using acmManager.Configuration;

namespace acmManager.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly acmManagerDbContext _context;

        public DefaultSettingsCreator(acmManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (acmManagerConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "sofeeys@outlook.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "sofeeys@outlook.com mailer", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-Hans", tenantId);
            
            // Crawler
            var crawlerPathProb = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            var crawlerPath = GetFiles(crawlerPathProb, "crawler.py");
            AddSettingIfNotExists(AppSettingNames.CrawlerPath, crawlerPath, tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }

        private static string GetFiles(string path, string pattern)
        {
            var directories = new string[] { };

            try
            {
                var res = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
                if (res.Length != 0) return res[0];

                directories = Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException) { }

            foreach (var directory in directories)
            {
                try
                {
                    return GetFiles(directory, pattern);
                }
                catch (UnauthorizedAccessException) { }
                catch (FileNotFoundException) { }
            }
            throw new FileNotFoundException($"can not find {pattern} in {path}");
        }
    }
}
