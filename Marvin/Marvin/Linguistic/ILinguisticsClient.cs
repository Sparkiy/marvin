using System.Threading.Tasks;

namespace Marvin.Linguistic
{
    /// <summary>
    /// Linguistics client interface.
    /// </summary>
    public interface ILinguisticsClient
    {
        /// <summary>
        /// List analyzers asynchronously.
        /// </summary>
        /// <returns>An array of supported analyzers.</returns>
        Task<Analyzer[]> ListAnalyzersAsync();

        /// <summary>
        /// Analyze text asynchronously.
        /// </summary>
        /// <param name="request">Analyze text request.</param>
        /// <returns>An array of analyze text result.</returns>
        Task<AnalyzeTextResult[]> AnalyzeTextAsync(AnalyzeTextRequest request);
    }
}