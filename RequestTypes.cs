using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ModSyncNinjaApiBridge
{
    /*Data:
{
mod: String, mod id GUID
modKey: String, mod key
version: String, mod version
publishedField: String, numbers only. Published field content (do not read as INT\LONG since Steam may change format)
            	about: String, Base64 Content of About.xml file
            	patch:{
                            	notes: String, Base64 Patch notes
                            	attributes:  String, attributes formatting
            	}
}
*/
    [Serializable]
    public class UpdateModRequest
    {
        /// <summary>
        /// The mod ModSyncNinja ID
        /// </summary>
        public string ModId;

        /// <summary>
        /// The mod secret key
        /// </summary>
        public string ModKey;

        /// <summary>
        /// Updated version
        /// </summary>
        public string Version;

        /// <summary>
        /// The steam published field contents
        /// </summary>
        public string PublishedField;

        /// <summary>
        /// Contents of mods About.xml file
        /// </summary>
        public string About;

        /// <summary>
        /// Patch details. Notes and attributes
        /// </summary>
        public Patch Patch;

        /// <summary>
        /// CSV List of languages names from mod Languages folder
        /// </summary>
        public string Languages;
        public bool SaveBreaking;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");
            if (this.ModId != null)
            {
                sb.Append("\"ModId\":\"");
                sb.Append(ModId.ToString());
            }
            if (this.ModKey != null)
            {
                sb.Append("\",\"ModKey\":\"");
                sb.Append(ModKey.ToString());
            }
            if (this.Version != null)
            {
                sb.Append("\",\"Version\":\"");
                sb.Append(Version.ToString());
            }
            if (this.PublishedField != null)
            {
                sb.Append("\",\"PublishedField\":\"");
                sb.Append(PublishedField.ToString());
            }
            if (this.Languages != null)
            {
                sb.Append("\",\"Languages\":\"");
                sb.Append(Languages.ToString());
            }
            if (this.About != null)
            {
                sb.Append("\",\"About\":\"");
                sb.Append(About.ToString());
            }
            sb.Append("\",\"SaveBreaking\":\"");
            sb.Append(SaveBreaking.ToString());
            if (this.Patch != null)
            {
                sb.Append("\",\"Patch\":");
                sb.Append(Patch.ToString());
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    [Serializable]
    public class Patch
    {
        /// <summary>
        /// Patch notes
        /// </summary>
        public string Notes;

        /// <summary>
        /// Patch attributes represented with a binary string. 000 - none, 111 - all, 010 - feature, etc
        /// </summary>
        public string Attributes;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.Append("\"Notes\":\"");
            sb.Append(Notes.ToString());
            sb.Append("\",\"Attributes\":\"");
            sb.Append(Attributes.ToString());
            sb.Append("\"}");
            return sb.ToString();
        }
    }
}
