using Microsoft.AspNetCore.Mvc;
using MicroserviceTemplate.WebApi.Models;

namespace MicroserviceTemplate.WebApi.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class SystemController : ApiControllerBase
    {
        [HttpGet("versions")]
        public async Task<SystemVersionResult> GetVersions()
        {
            SystemVersionResult versionResult = new SystemVersionResult();
            versionResult.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            foreach (var assemblyName in System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                versionResult.Dependencies.TryAdd(assemblyName.Name, assemblyName.Version.ToString());
            }

            return versionResult;
        }

        [HttpGet("ping")]
        public async Task<string> GetPing()
        {
            return "pong";
        }

    }
}