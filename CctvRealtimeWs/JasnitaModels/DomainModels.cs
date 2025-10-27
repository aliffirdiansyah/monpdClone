using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CctvRealtimeWs.JasnitaModels
{
    public class DomainModels
    {
        // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
        public class Domain1
        {
            [JsonPropertyName("domain")]
            public Domain1 Domain { get; set; }

            [JsonPropertyName("domainManagedFields")]
            public List<DomainManagedField> DomainManagedFields { get; set; }

            [JsonPropertyName("isSynced")]
            public bool IsSynced { get; set; }

            [JsonPropertyName("permission")]
            public Permission Permission { get; set; }
        }

        public class Domain2
        {
            [JsonPropertyName("backupsMaxCount")]
            public int BackupsMaxCount { get; set; }

            [JsonPropertyName("cloudAuthority")]
            public string CloudAuthority { get; set; }

            [JsonPropertyName("cloudConnKey")]
            public string CloudConnKey { get; set; }

            [JsonPropertyName("connServerName")]
            public string ConnServerName { get; set; }

            [JsonPropertyName("createTime")]
            public DateTime CreateTime { get; set; }

            [JsonPropertyName("domainId")]
            public int DomainId { get; set; }

            [JsonPropertyName("emailLimit")]
            public int EmailLimit { get; set; }

            [JsonPropertyName("eventLimit")]
            public int EventLimit { get; set; }

            [JsonPropertyName("failoverURL")]
            public string FailoverURL { get; set; }

            [JsonPropertyName("isService")]
            public bool IsService { get; set; }

            [JsonPropertyName("isVisible")]
            public bool IsVisible { get; set; }

            [JsonPropertyName("mmSize")]
            public int MmSize { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("publicURL")]
            public string PublicURL { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("userId")]
            public int UserId { get; set; }

            [JsonPropertyName("webClientURL")]
            public string WebClientURL { get; set; }

            [JsonPropertyName("webConfiguratorURL")]
            public string WebConfiguratorURL { get; set; }
        }

        public class DomainManagedField
        {
            [JsonPropertyName("enumValues")]
            public List<string> EnumValues { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("required")]
            public bool Required { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }
        }

        public class Permission
        {
            [JsonPropertyName("CanRequestRestartArpClient")]
            public bool CanRequestRestartArpClient { get; set; }

            [JsonPropertyName("CanViewArpClientUpdateStatus")]
            public bool CanViewArpClientUpdateStatus { get; set; }

            [JsonPropertyName("canBeDeleted")]
            public bool CanBeDeleted { get; set; }

            [JsonPropertyName("canDeleteContainers")]
            public bool CanDeleteContainers { get; set; }

            [JsonPropertyName("canDeleteMediaFiles")]
            public bool CanDeleteMediaFiles { get; set; }

            [JsonPropertyName("canDeleteSites")]
            public bool CanDeleteSites { get; set; }

            [JsonPropertyName("canEditContainers")]
            public bool CanEditContainers { get; set; }

            [JsonPropertyName("canEditSites")]
            public bool CanEditSites { get; set; }

            [JsonPropertyName("canEncryptContainers")]
            public bool CanEncryptContainers { get; set; }

            [JsonPropertyName("canManageRoles")]
            public bool CanManageRoles { get; set; }

            [JsonPropertyName("canRename")]
            public bool CanRename { get; set; }

            [JsonPropertyName("canSupervisor")]
            public bool CanSupervisor { get; set; }

            [JsonPropertyName("canUseManualExport")]
            public bool CanUseManualExport { get; set; }

            [JsonPropertyName("canUseMediaFiles")]
            public bool CanUseMediaFiles { get; set; }

            [JsonPropertyName("canViewContainers")]
            public bool CanViewContainers { get; set; }

            [JsonPropertyName("canViewSites")]
            public bool CanViewSites { get; set; }

            [JsonPropertyName("canViewWebClient")]
            public bool CanViewWebClient { get; set; }

            [JsonPropertyName("canWebConfigurator")]
            public bool CanWebConfigurator { get; set; }
        }

        public class DomainModelsResponse
        {
            [JsonPropertyName("countInPage")]
            public int CountInPage { get; set; }

            [JsonPropertyName("domains")]
            public List<Domain1> Domains { get; set; }

            [JsonPropertyName("totalCount")]
            public int TotalCount { get; set; }
        }


    }
}
