using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ModSyncNinjaApiBridge
{
    [Serializable]
    public class ResponseStatus
    {
        public bool Success;
        public int Error;
        
        public static bool TryParse(string response, out ResponseStatus status)
        {
            try
            {
                if (string.IsNullOrEmpty(response))
                {
                    status = new ResponseStatus();
                    status.Success = true;
                    return true;
                }

                response = response.ToLower();
                bool success = false;
                int error = 0;
                int start = response.IndexOf("success");
                if (start != -1)
                {
                    start = response.IndexOf(':', start);
                    if (start != -1)
                    {
                        char c = response[start + 1];
                        if (c == 't')
                            success = true;
                    }
                }
                start = response.IndexOf("error");
                if (start != -1)
                {
                    start = response.IndexOf(':', start);
                    if (start != -1)
                    {
                        ++start;
                        int end = start;
                        while (response[end] >= '0' && response[end] <= '9')
                        {
                            ++end;
                        }
                        if (end > start + 1)
                        {
                            int.TryParse(response.Substring(start, end - start), out error);
                        }
                    }
                }

                status = new ResponseStatus();
                status.Success = success;
                status.Error = error;
                return true;
            }
            catch
            {
                status = null;
                return false;
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + " Success=[" + this.Success + "] Error=[" + this.Error + "]";
        }
    }

    [Serializable]
    public class ModVersionPayload
    {
        public Guid ModId;
        public string Version = String.Empty;

        public override string ToString()
        {
            return this.GetType().Name + " ModId=[" + this.ModId + "] Version=[" + this.Version + "]";
        }
    }

    [Serializable]
    public class GetModVersionResponse
    {
        public ResponseStatus Status;
        public ModVersionPayload Payload;

        public static bool TryParse(string response, out GetModVersionResponse getModVersion)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(response);
                XElement root = xdoc.Element("NinjaApi2Controller.ModVersion");
                // Payload portion will be ignored as it's not used
                /*XElement parent = root.Element("Payload");
                XElement v = parent.Element("ModId");
                string id = v.Value;
                v = parent.Element("Version");
                string version = v.Value;*/

                XElement parent = root.Element("Status");
                XElement v = parent.Element("Error");
                int error;
                int.TryParse(v.Value, out error);
                v = parent.Element("Success");
                bool success;
                bool.TryParse(v.Value, out success);

                getModVersion = new GetModVersionResponse();
                getModVersion.Status.Error = error;
                getModVersion.Status.Success = success;
                return true;
            }
            catch
            {
                getModVersion = null;
                return false;
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + " Status=[" + this.Status.ToString() + "] Payload=[" + this.Payload.ToString() + "]";
        }
    }
}
