// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

namespace MilkyWayMarket.Code
{
    public class Creator
    {
        [JsonProperty("login")] public string login { get; set; }

        [JsonProperty("id")] public int? id { get; set; }

        [JsonProperty("node_id")] public string node_id { get; set; }

        [JsonProperty("avatar_url")] public string avatar_url { get; set; }

        [JsonProperty("gravatar_id")] public string gravatar_id { get; set; }

        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("html_url")] public string html_url { get; set; }

        [JsonProperty("followers_url")] public string followers_url { get; set; }

        [JsonProperty("following_url")] public string following_url { get; set; }

        [JsonProperty("gists_url")] public string gists_url { get; set; }

        [JsonProperty("starred_url")] public string starred_url { get; set; }

        [JsonProperty("subscriptions_url")] public string subscriptions_url { get; set; }

        [JsonProperty("organizations_url")] public string organizations_url { get; set; }

        [JsonProperty("repos_url")] public string repos_url { get; set; }

        [JsonProperty("events_url")] public string events_url { get; set; }

        [JsonProperty("received_events_url")] public string received_events_url { get; set; }

        [JsonProperty("type")] public string type { get; set; }

        [JsonProperty("site_admin")] public bool? site_admin { get; set; }
    }

    public class Deployments
    {
        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("id")] public int? id { get; set; }

        [JsonProperty("node_id")] public string node_id { get; set; }

        [JsonProperty("task")] public string task { get; set; }

        [JsonProperty("original_environment")] public string original_environment { get; set; }

        [JsonProperty("environment")] public string environment { get; set; }

        [JsonProperty("description")] public object description { get; set; }

        [JsonProperty("created_at")] public DateTime? created_at { get; set; }

        [JsonProperty("updated_at")] public DateTime? updated_at { get; set; }

        [JsonProperty("statuses_url")] public string statuses_url { get; set; }

        [JsonProperty("repository_url")] public string repository_url { get; set; }

        [JsonProperty("creator")] public Creator creator { get; set; }

        [JsonProperty("sha")] public string sha { get; set; }

        [JsonProperty("ref")] public string @ref { get; set; }

        [JsonProperty("payload")] public Payload payload { get; set; }

        [JsonProperty("transient_environment")]
        public bool? transient_environment { get; set; }

        [JsonProperty("production_environment")]
        public bool? production_environment { get; set; }

        [JsonProperty("performed_via_github_app")]
        public PerformedViaGithubApp performed_via_github_app { get; set; }
    }

    public class Owner
    {
        [JsonProperty("login")] public string login { get; set; }

        [JsonProperty("id")] public int? id { get; set; }

        [JsonProperty("node_id")] public string node_id { get; set; }

        [JsonProperty("avatar_url")] public string avatar_url { get; set; }

        [JsonProperty("gravatar_id")] public string gravatar_id { get; set; }

        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("html_url")] public string html_url { get; set; }

        [JsonProperty("followers_url")] public string followers_url { get; set; }

        [JsonProperty("following_url")] public string following_url { get; set; }

        [JsonProperty("gists_url")] public string gists_url { get; set; }

        [JsonProperty("starred_url")] public string starred_url { get; set; }

        [JsonProperty("subscriptions_url")] public string subscriptions_url { get; set; }

        [JsonProperty("organizations_url")] public string organizations_url { get; set; }

        [JsonProperty("repos_url")] public string repos_url { get; set; }

        [JsonProperty("events_url")] public string events_url { get; set; }

        [JsonProperty("received_events_url")] public string received_events_url { get; set; }

        [JsonProperty("type")] public string type { get; set; }

        [JsonProperty("site_admin")] public bool? site_admin { get; set; }
    }

    public class Payload
    {
    }

    public class PerformedViaGithubApp
    {
        [JsonProperty("id")] public int? id { get; set; }

        [JsonProperty("slug")] public string slug { get; set; }

        [JsonProperty("node_id")] public string node_id { get; set; }

        [JsonProperty("owner")] public Owner owner { get; set; }

        [JsonProperty("name")] public string name { get; set; }

        [JsonProperty("description")] public string description { get; set; }

        [JsonProperty("external_url")] public string external_url { get; set; }

        [JsonProperty("html_url")] public string html_url { get; set; }

        [JsonProperty("created_at")] public DateTime? created_at { get; set; }

        [JsonProperty("updated_at")] public DateTime? updated_at { get; set; }

        [JsonProperty("permissions")] public Permissions permissions { get; set; }

        [JsonProperty("events")] public List<object> events { get; set; }
    }

    public class Permissions
    {
        [JsonProperty("checks")] public string checks { get; set; }

        [JsonProperty("contents")] public string contents { get; set; }

        [JsonProperty("deployments")] public string deployments { get; set; }

        [JsonProperty("members")] public string members { get; set; }

        [JsonProperty("metadata")] public string metadata { get; set; }

        [JsonProperty("pages")] public string pages { get; set; }

        [JsonProperty("statuses")] public string statuses { get; set; }
    }

    public class Root
    {
        [JsonProperty("Deployments")] public List<Deployments> MyArray { get; set; }
    }

}