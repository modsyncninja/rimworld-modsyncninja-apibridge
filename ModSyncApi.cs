using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ModSyncNinjaApiBridge
{
    public class ModSyncApi
    {
        public const string DEFAULT_URL = "http://www.modsync.ninja/api2/";
        // allows changing URL for dev env from outsite.
        public string URL = DEFAULT_URL;

        /// <summary>
        /// Gets the mod version by mod id.
        /// Synchronous call!
        /// </summary>
        /// <param name="modId">Mod ID</param>
        /// <returns>GetModVersionResponse object</returns>
        public GetModVersionResponse GetModVersion(string modId)
        {
            try
            {
                WebRequest request = WebRequest.Create(URL + "GetModVersion?modId=" + modId);

                request.Method = "GET";
                request.Timeout = 20000;
                var response = request.GetResponse() as HttpWebResponse;

                using (Stream stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    var responseString = reader.ReadToEnd();
                    GetModVersionResponse result;
                    GetModVersionResponse.TryParse(responseString, out result);
                    return result;
                }
            }
            catch (Exception e)
            {
                return new GetModVersionResponse() { Payload = new ModVersionPayload(), Status = new ResponseStatus() { Error = (int)ModVersionApiErrors.UnknownError, Success = false } };
            }
        }

        public ResponseStatus UpdateMod(UpdateModRequest updateRequest)
        {
            try
            {
                WebRequest request = WebRequest.Create(URL + "UpdateMod");
                // convert long strings to base64
                if (!string.IsNullOrEmpty(updateRequest.About))
                    updateRequest.About = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(updateRequest.About));
                if (!string.IsNullOrEmpty(updateRequest.Patch.Notes))
                    updateRequest.Patch.Notes = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(updateRequest.Patch.Notes));
                // default attributes format
                if (string.IsNullOrEmpty(updateRequest.Patch.Attributes))
                    updateRequest.Patch.Attributes = "000";


                byte[] byteArray =
                    Encoding.UTF8.GetBytes(
                        System.Convert.ToBase64String(Encoding.UTF8.GetBytes(updateRequest.ToString())));
                request.Method = "POST";
                request.Timeout = 20000;
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = byteArray.Length;

                // This is required by Mono/Unity
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                var response = request.GetResponse() as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    var responseString = reader.ReadToEnd();
                    ResponseStatus result;
                    ResponseStatus.TryParse(responseString, out result);
                    return result;
                }
            }
            catch (Exception e)
            {
                return new ResponseStatus() { Error = (int)UpdateModApiErrors.UnknownError, Success = false };
            }
        }

        /// <summary>
        /// Needed by Mono/Unity
        /// </summary>
        public bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }
    }
}
