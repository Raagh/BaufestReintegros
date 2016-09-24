using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Reintegros.Shared.BL
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sharepoint/soap/directory/")]

    public partial class User
    {

        private uint idField;

        private string sidField;

        private string nameField;

        private string loginNameField;

        private string emailField;

        private string notesField;

        private string isSiteAdminField;

        private string isDomainGroupField;

        private byte flagsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Sid
        {
            get
            {
                return this.sidField;
            }
            set
            {
                this.sidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string LoginName
        {
            get
            {
                return this.loginNameField;
            }
            set
            {
                this.loginNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsSiteAdmin
        {
            get
            {
                return this.isSiteAdminField;
            }
            set
            {
                this.isSiteAdminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsDomainGroup
        {
            get
            {
                return this.isDomainGroupField;
            }
            set
            {
                this.isDomainGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Flags
        {
            get
            {
                return this.flagsField;
            }
            set
            {
                this.flagsField = value;
            }
        }


        
        public override string ToString()
        {
            return Name;
        }
    }
}