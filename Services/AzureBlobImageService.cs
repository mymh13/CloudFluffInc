using CloudFluffInc.Configurations;
using Microsoft.Extensions.Options;
using CloudFluffInc.Storage;
using Microsoft.Extensions.Logging;

namespace CloudFluffInc.Services;

public class AzureBlobImageService : IImageService
{
    private readonly string _blobContainerUrl;
    private readonly ILogger<AzureBlobImageService> _logger;

    public AzureBlobImageService(IOptions<AzureBlobOptions> options, ILogger<AzureBlobImageService> logger)
    {
        _blobContainerUrl = options.Value.ContainerUrl;
        _logger = logger;
    }

    public string GetImageUrl(string imageName)
    {
        var url = $"{_blobContainerUrl}/{imageName}";
        _logger.LogInformation($"Generated Azure Blob URL: {url}");
        return url;
    }
}
