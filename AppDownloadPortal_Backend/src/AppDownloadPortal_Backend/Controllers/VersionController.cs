using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppDownloadPortal_Backend.Model;
using AppDownloadPortal_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppDownloadPortal_Backend.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly VersionService _versionService;

        public VersionController(VersionService versionService)
        {
            _versionService = versionService;
        }
        
        // GET api/version/dev
        [HttpGet("{env}")]
        public async Task<List<AppVersion>> Get(string env)
        {
            return await _versionService.GetVersionAsync(env);
        }

        // POST api/version
        [HttpPost]
        public async Task Post([FromBody] AppVersion appVersion)
        {
            await _versionService.UpdateVersionAsync(appVersion);
        }

        // PUT api/version
        [HttpPut]
        public async Task Put([FromBody] AppVersion appVersion)
        {
            await _versionService.UpdateVersionAsync(appVersion);
        }

        // DELETE api/env/version
        [HttpDelete("{env}/{version}")]
        public async Task Delete(string env, string version)
        {
            await _versionService.DeleteVersionAsync(env, version);
        }

        // PUT api/version/default
        [HttpPut("default")]
        public async Task SetDefault([FromBody] AppVersion appVersion)
        {
            await _versionService.SetDefaultAsync(appVersion);
        }
    }
}