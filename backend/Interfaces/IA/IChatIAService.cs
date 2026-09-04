namespace OpenDoors.Api.Interfaces.IA
{
    public interface IChatIAService
    {
        public Task<string> ChatAsync(string systemPrompt, string userPrompt);
        public Task<T> ChatJsonAsync<T>(string systemPrompt, string userPrompt);
    }
}
