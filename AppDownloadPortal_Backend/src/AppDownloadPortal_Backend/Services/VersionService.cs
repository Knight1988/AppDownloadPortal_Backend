using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppDownloadPortal_Backend.Model;
using Newtonsoft.Json;

namespace AppDownloadPortal_Backend.Services
{
    public class VersionService
    {
        private readonly S3Service _s3Service;
        private const string BucketName = "system-micromarket-app-download-portal";
        private const string AppBucketName = "system-micromarket-data";

        public VersionService(S3Service s3Service)
        {
            _s3Service = s3Service;
        }

        public async Task<List<AppVersion>> GetVersionAsync(string env)
        {
            var contents =
                await _s3Service.ReadAllTextAsync(BucketName, $"data/version.{env}.json");
            var versions = JsonConvert.DeserializeObject<AppVersion[]>(contents)
                .OrderByDescending(p => p.Version)
                .ToList();

            foreach (var version in versions)
            {
                version.Environment ??= env;
                version.DisplayText ??= version.VersionString;
            }
            
            return versions;
        }
        
        private async Task<List<AppVersion>> GetVersionOriginAsync(string env)
        {
            var contents =
                await _s3Service.ReadAllTextAsync(BucketName, $"data/version.{env}.json");
            var versions = JsonConvert.DeserializeObject<AppVersion[]>(contents)
                .OrderByDescending(p => p.Version)
                .ToList();
            
            return versions;
        }

        public async Task UpdateVersionAsync(AppVersion appVersion)
        {
            var versions = await GetVersionOriginAsync(appVersion.Environment);
            var editVersion = versions.FirstOrDefault(p => p.VersionString == appVersion.VersionString);
            if (editVersion == null) editVersion = appVersion;

            editVersion.DisplayText = appVersion.DisplayText;
            var contents = JsonConvert.SerializeObject(versions, Formatting.Indented);
            await _s3Service.WriteAllTextAsync(BucketName, $"data/version.{appVersion.Environment}.json", contents);
        }

        public async Task SetDefaultAsync(AppVersion appVersion)
        {
            var versions = await GetVersionOriginAsync(appVersion.Environment);
            foreach (var version in versions)
            {
                version.IsDefault = version.VersionString == appVersion.VersionString;
            }
            
            var contents = JsonConvert.SerializeObject(versions, Formatting.Indented);
            await _s3Service.WriteAllTextAsync(BucketName, $"data/version.{appVersion.Environment}.json", contents);
            
            // get app installer
            var versionFileName = $"Taburettoreji.{appVersion.Environment}.{appVersion.VersionString}.appinstaller";
            var defaultFileName = $"Taburettoreji.{appVersion.Environment}.appinstaller";
            var appInstallerContent =
                await _s3Service.ReadAllTextAsync(AppBucketName, $"Child-store/app/version/{appVersion.Environment}/{versionFileName}");

            appInstallerContent = appInstallerContent.Replace(versionFileName, defaultFileName);
            await _s3Service.WriteAllTextAsync(AppBucketName, $"Child-store/app/version/{appVersion.Environment}/{defaultFileName}", appInstallerContent);
        }
    }
}