namespace MicroserviceTemplate.WebApi.Models
{
    public class SystemVersionResult
    {
        public SystemVersionResult()
        {
            Dependencies = new Dictionary<string, string>();
        }

        public string ApplicationVersion { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }
    }
}
