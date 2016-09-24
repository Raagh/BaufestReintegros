using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Reintegros.Shared.BL
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sharepoint/soap/directory/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/sharepoint/soap/directory/", IsNullable = false, ElementName = "Users")]
    public partial class UserContainer
    {

        private User[] userField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("User")]
        public User[] User
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }
    }
}