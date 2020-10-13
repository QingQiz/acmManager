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
            if (!SettingExists(AppSettingNames.CrawlerPath, tenantId))
            {
                AddSettingIfNotExists(AppSettingNames.CrawlerPath, new Func<string>(() =>
                {
                    var crawlerPathProb = Directory.GetCurrentDirectory();

                    try
                    {
                        return SearchFile(crawlerPathProb, "crawler.py");
                    }
                    catch (FileNotFoundException)
                    {
                        crawlerPathProb = Directory.GetParent(crawlerPathProb).ToString();
                        return SearchFile(crawlerPathProb, "crawler.py");
                    }
                })(), tenantId);
            }
            
            // Python path
            if (!SettingExists(AppSettingNames.PythonPath, tenantId))
            {
                AddSettingIfNotExists(AppSettingNames.PythonPath, new Func<string>(() =>
                {
                    var pythonPathProb = new List<string>()
                    {
                        @"C:\Users",
                        @"C:\Program Files (x86)",
                        @"C:\Program Files",
                    };
                    foreach (var prob in pythonPathProb)
                    {
                        try
                        {
                            return SearchFile(prob, "python.exe");
                        }
                        catch (FileNotFoundException)
                        {
                        }
                    }
                    return "python";
                })(), tenantId);
            }
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
