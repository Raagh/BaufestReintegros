using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using Reintegros.Shared.SL.SPUsersService;
using Reintegros.Shared.SL.SPListsService;
using Reintegros.Shared.BL;
using Reintegros.Shared.DL;

namespace Reintegros.Shared.SL
{
    public static class ServiceHelper
    {
        public static NetworkCredential userCredential;
        private static string listName = "Reintegros";
        public static List<User> usersList = null;
        private static SPListsService.Lists listService = new Lists();
        private static SPUsersService.UserGroup userService = new UserGroup();


        public static List<string> GetComboChoices(string fieldName)
        {
            XmlNode ndList = listService.GetList(listName);
        
            return XmlHelper.ParseComboResult(ndList,fieldName);
        }

        public static int SubmitReintegro(Reintegro newReintegro)
        {
            /*Get Name attribute values (GUIDs) for list and view. */
            System.Xml.XmlNode ndList = listService.GetListAndView(listName, "");
            string strListID = ndList.ChildNodes[0].Attributes["Name"].Value;
            string strViewID = ndList.ChildNodes[1].Attributes["Name"].Value;

            /*Create an XmlDocument object and construct a Batch element and its
            attributes. Note that an empty ViewName parameter causes the method to use the default view. */
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement batchElement = doc.CreateElement("Batch");
            batchElement.SetAttribute("OnError", "Continue");
            batchElement.SetAttribute("ListVersion", "1");
            batchElement.SetAttribute("ViewName", strViewID);

            
            /*Specify methods for the batch post using CAML. To update or delete, 
            specify the ID of the item, and to update or add, specify 
            the value to place in the specified column.*/
            batchElement.InnerXml =
               "<Method ID='1' Cmd='New'>" +
                    "<Field Name='Title'>" + newReintegro.Title+ "</Field>" +
                    "<Field Name='Persona'>" + newReintegro.Persona + "</Field>" +
                    "<Field Name='Subunidad'>" + newReintegro.Subunidad + "</Field>" +
                    "<Field Name='Proyecto_x002e_Subproyecto_x002e'>" + newReintegro.Subproyecto + "</Field>" +
                    "<Field Name='Motivo_x0020_de_x0020_Solicitud'>" + newReintegro.Motivo + "</Field>" +
                    "<Field Name='Importe'>" + newReintegro.Importe + "</Field>" +
                    "<Field Name='Fecha'>" + newReintegro.Fecha + "</Field>" +
                    "<Field Name='Cliente_x002f_Proyecto'>" + newReintegro.ClienteProyecto + "</Field>" +
                    "<Field Name='Debe_x0020_Autorizar'>" + newReintegro.DebeAutorizar + "</Field>" +
                    "<Field Name='Persona_x0020_Autorizante'>" + newReintegro.PersonaAutorizante +"</Field>" +
               "</Method>";

            /*Update list items. This example uses the list GUID, which is recommended, 
            but the list display name will also work.*/
            XmlNode insertedNode = listService.UpdateListItems(strListID, batchElement);
          
           return XmlHelper.GetResultID(insertedNode);
        }

        internal static void AttachPicturesToReintegro(int submitedID, List<string> paths)
        {           
            int counter = 0;

            foreach (string item in paths)
            {
                counter++;
                FileStream fStream = File.OpenRead(item);
                string fileName = fStream.Name.Substring(3);
                byte[] contents = new byte[fStream.Length];
                fStream.Read(contents, 0, (int)fStream.Length);
                fStream.Close();

                //listService.AddAttachment(listName, submitedID.ToString(), "picture"+ counter +".jpg", contents);
                listService.AddAttachmentAsync(listName, submitedID.ToString(), "picture" + counter + ".jpg", contents);
            }        
        }

        public static UserContainer GetListUsers()
        {
            
            XmlNode ndUsers = userService.GetUserCollectionFromSite();

            UserContainer myUsers = XmlHelper.ConvertNode<UserContainer>(ndUsers);

            usersList = myUsers.User.ToList();

            return myUsers;
        }

        public static bool AuthenticateCredentials(NetworkCredential userCredential)
        {
            try
            {
                listService.Credentials = userCredential;
                userService.Credentials = userCredential;

                XmlNode ndList = listService.GetList(listName);

                return true;
            }
            catch (Exception)
            {
                return false;            
            }
        }
    }
}