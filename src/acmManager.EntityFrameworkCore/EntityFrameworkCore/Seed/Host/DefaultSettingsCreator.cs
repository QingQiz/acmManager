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
        }

        private bool SettingExists(string name, int? tenantId)
        {
            return _context.Settings.IgnoreQueryFilters()
                .Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null);
        }
        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (SettingExists(name, tenantId))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }

        private static string SearchFile(string path, string fn)
        {
            var q = new Queue<string>(); 

            q.Enqueue(path);

            while (q.Any())
            {
                var nowPath = q.Dequeue();

                try
                {
                    var fs = Directory.GetFiles(nowPath);
                    if (fs.Any(x => x == Path.Combine(nowPath, fn)))
                    {
                        return Path.Combine(nowPath, fn);
                    }

                    foreach (var p in Directory.GetDirectories(nowPath))
                    {
                        q.Enqueue(p);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
            throw new FileNotFoundException($"Can not find {fn} in {path}");
        }
    }
}
