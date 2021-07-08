using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace AppDownloadPortal_Backend.Services
{
    public class S3Service
    {
        public async Task<string> ReadAllTextAsync(string bucket, string key)
        {
            var client = new AmazonS3Client();

            // Create a GetObject request
            var request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            };

            // Issue request and remember to dispose of the response
            using var response = await client.GetObjectAsync(request);
            using var reader = new StreamReader(response.ResponseStream);
            var contents = await reader.ReadToEndAsync();
            return contents;
        }

        public async Task WriteAllTextAsync(string bucket, string key, string contents)
        {
            var client = new AmazonS3Client();

            // Create a GetObject request
            var request = new PutObjectRequest()
            {
                BucketName = bucket,
                Key = key,
                ContentBody = contents,
                CannedACL = S3CannedACL.PublicRead
            };

            await client.PutObjectAsync(request);
        }
    }
}