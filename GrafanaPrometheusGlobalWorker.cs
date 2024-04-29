using System.Reflection;
using ScyScaff.Core.Models.Parser;
using ScyScaff.Core.Models.Plugins;
using ScyScaff.Docker.Enums;
using ScyScaff.Docker.Models.Builder;
using ScyScaff.Docker.Models.Plugins;

namespace ScyScaffPlugin.GrafanaPrometheusGlobalWorker;

public class GrafanaPrometheusGlobalWorker : IGlobalWorkerTemplatePlugin, IDockerCompatible
{
    public string Name => "grafana-prometheus";

    public Dictionary<string, string[]> SupportedFlags { get; } = new();

    public IEnumerable<DockerComposeService> GetComposeServices(string projectName, IScaffolderEntity? entity, string serviceName, int serviceIndex)
    {
        List<DockerComposeService> dockerComposeServices = new();

        DockerComposeService prometheusService = new DockerComposeService
        {
            Type = DockerComposeServiceType.GlobalWorker,
            Image = "prom/prometheus",
            ContainerName = "prometheus",
            Ports = new Dictionary<int, int?>
            {
                { 9090, 9090 }
            },
            Volumes = new Dictionary<string, string>
            {
                { $"./{projectName}.Global/Prometheus", "/etc/prometheus" },
                { "prometheus-data", "/prometheus" }
            }
        };

        DockerComposeService grafanaService = new DockerComposeService
        {
            Type = DockerComposeServiceType.GlobalWorker,
            Image = "grafana/grafana",
            ContainerName = "grafana",
            Ports = new Dictionary<int, int?>
            {
                { 3000, 3000 }
            }
        };
        
        dockerComposeServices.Add(prometheusService);
        dockerComposeServices.Add(grafanaService);
        
        return dockerComposeServices;
    }
}