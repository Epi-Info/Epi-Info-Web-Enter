using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.XPath;
using Epi.Web.Common.BusinessObject;
using System.Xml;
using System.Xml.Linq;
namespace Epi.Web.BLL
{
   /// <summary>
   /// 
   /// </summary>
  
    public class Publisher
    {
        private Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;
        private Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;
        #region"Public members"
        /// <summary>
        ///  This class is used to process the object sent from the WCF service “SurveyManager”, 
        ///  save survey info into SurvayMetaData Table 
        ///  and then returns a URL, StatusText and IsPublished indicator.
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        /// 
        public Publisher(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao, Epi.Web.Interfaces.DataInterfaces.IOrganizationDao pPrganizationDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
            this.OrganizationDao = pPrganizationDao;
        }
        public Publisher()
        {
            
        }
        public SurveyRequestResultBO PublishSurvey(SurveyInfoBO  pRequestMessage)
        {
        SurveyRequestResultBO result = new SurveyRequestResultBO();

        if (IsRelatedForm(pRequestMessage.XML))
            {
            result = PublishRelatedFormSurvey(pRequestMessage);
            }
        else
            {
            result = Publish(pRequestMessage);
                 
            }
            return result;
        }

       


        public SurveyRequestResultBO RePublishSurvey(SurveyInfoBO pRequestMessage)
        {

            SurveyRequestResultBO result = new SurveyRequestResultBO();
            if (IsRelatedForm(pRequestMessage.XML))
                {
                result = RePublishRelatedFormSurvey(pRequestMessage);
                }
            else
                {
                result = RePublish(pRequestMessage);
                }
            return result;
        }

     

        private static bool ValidateSurveyFields(SurveyInfoBO pRequestMessage)
        {

            bool isValid = true;


            if (pRequestMessage.ClosingDate == null)
            {

                isValid = false;

            }
            
            else if (string.IsNullOrEmpty(pRequestMessage.XML) || string.IsNullOrWhiteSpace(pRequestMessage.XML))
            {

                isValid = false;
            }
            else if (string.IsNullOrEmpty(pRequestMessage.SurveyName))
            {

                isValid = false;
            }
            
            else if ( string.IsNullOrEmpty(pRequestMessage.UserPublishKey.ToString()))
            {

                isValid = false;
            }


 
            return isValid;        
        }
     
        /// <summary>
        /// validate the Organization key passed with the list of Organization keys retrieved from database 
        /// through EF
        /// </summary>
        /// <param name="OrganizationKey"></param>
        /// <returns></returns>
        private bool ValidateOrganizationKey(Guid gOrganizationKey)
        {
            string strOrgKeyEncrypted = Epi.Web.Common.Security.Cryptography.Encrypt(gOrganizationKey.ToString());
            List<OrganizationBO> OrganizationBoList = this.OrganizationDao.GetOrganizationInfoByOrgKey(strOrgKeyEncrypted);
            if (OrganizationBoList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;    
            }
            
           
        }

        #endregion

        #region "Private members"
        private string GetURL(SurveyInfoBO  pRequestMessage, Guid SurveyId)
        {
            System.Text.StringBuilder URL = new System.Text.StringBuilder();
            URL.Append(System.Configuration.ConfigurationManager.AppSettings["URL"]);
           // URL.Append("/");
            //URL.Append(pRequestMessage.SurveyNumber.ToString());
            //URL.Append("/");
            URL.Append(SurveyId.ToString());
            return URL.ToString();
        }

        private bool IsRelatedForm(string Xml) 
            {

            bool IsRelatedForm = false;
            XDocument xdoc = XDocument.Parse(Xml);


            int NumberOfViews = xdoc.Descendants("View").Count();
            if (NumberOfViews > 1)
                {
                IsRelatedForm = true;
                
                }

             return IsRelatedForm;
            
            }

        private SurveyRequestResultBO Publish(SurveyInfoBO pRequestMessage)
            {
            SurveyRequestResultBO result = new SurveyRequestResultBO();

           
                var SurveyId = Guid.NewGuid();

                if (pRequestMessage != null)
                    {

                    //if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                    if (ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                        {

                        if (ValidateSurveyFields(pRequestMessage))
                            {
                            try
                                {

                                Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();

                                BO.SurveyId = SurveyId.ToString();
                                BO.ClosingDate = pRequestMessage.ClosingDate;

                                BO.IntroductionText = pRequestMessage.IntroductionText;
                                BO.ExitText = pRequestMessage.ExitText;
                                BO.DepartmentName = pRequestMessage.DepartmentName;
                                BO.OrganizationName = pRequestMessage.OrganizationName;

                                BO.SurveyNumber = pRequestMessage.SurveyNumber;

                                BO.XML = pRequestMessage.XML;

                                BO.SurveyName = pRequestMessage.SurveyName;

                                BO.SurveyType = pRequestMessage.SurveyType;
                                BO.UserPublishKey = pRequestMessage.UserPublishKey;
                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;
                                BO.IsDraftMode = pRequestMessage.IsDraftMode;
                                BO.StartDate = pRequestMessage.StartDate;
                                BO.ViewId = pRequestMessage.ViewId;
                                BO.ParentId = pRequestMessage.ParentId;
                                BO.OwnerId = pRequestMessage.OwnerId;
                                //Insert Survey MetaData
                                this.SurveyInfoDao.InsertSurveyInfo(BO);
                                

                                // Set Survey Settings
                                this.SurveyInfoDao.InsertFormdefaultSettings(SurveyId.ToString());
                                
                                result.URL = GetURL(pRequestMessage, SurveyId);
                                result.IsPulished = true;
                                }
                            catch (Exception ex)
                                {
                                System.Console.Write(ex.ToString());
                                //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
                                result.URL = "";
                                result.IsPulished = false;
                                result.StatusText = "An Error has occurred while publishing your survey.";
                                }




                            }
                        else
                            {

                            result.URL = "";
                            result.IsPulished = false;
                            result.StatusText = "One or more survey required fields are missing values.";
                            }

                        }
                    else
                        {

                        result.URL = "";
                        result.IsPulished = false;
                        result.StatusText = "Organization Key is invalid.";

                        }
                    }
               
            return result;
            }
        private SurveyRequestResultBO RePublish(SurveyInfoBO pRequestMessage)
            {

            SurveyRequestResultBO result = new SurveyRequestResultBO();
            
                var SurveyId = new Guid(pRequestMessage.SurveyId);

                if (pRequestMessage != null)
                    {

                    //if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                    if (ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                        {

                        if (ValidateSurveyFields(pRequestMessage))
                            {
                            try
                                {

                                Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();

                                BO.SurveyId = SurveyId.ToString();
                                BO.ClosingDate = pRequestMessage.ClosingDate;

                                BO.IntroductionText = pRequestMessage.IntroductionText;
                                BO.ExitText = pRequestMessage.ExitText;
                                BO.DepartmentName = pRequestMessage.DepartmentName;
                                BO.OrganizationName = pRequestMessage.OrganizationName;

                                BO.SurveyNumber = pRequestMessage.SurveyNumber;

                                BO.XML = pRequestMessage.XML;

                                BO.SurveyName = pRequestMessage.SurveyName;

                                BO.SurveyType = pRequestMessage.SurveyType;
                                BO.UserPublishKey = pRequestMessage.UserPublishKey;
                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;
                                BO.IsDraftMode = pRequestMessage.IsDraftMode;
                                BO.StartDate = pRequestMessage.StartDate;


                                this.SurveyInfoDao.UpdateSurveyInfo(BO);
                                result.URL = GetURL(pRequestMessage, SurveyId);
                                result.IsPulished = true;
                                }
                            catch (Exception ex)
                                {
                                System.Console.Write(ex.ToString());
                                //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
                                result.URL = "";
                                result.IsPulished = false;
                                result.StatusText = "An Error has occurred while publishing your survey.";
                                }




                            }
                        else
                            {

                            result.URL = "";
                            result.IsPulished = false;
                            result.StatusText = "One or more survey required fields are missing values.";
                            }

                        }
                    else
                        {

                        result.URL = "";
                        result.IsPulished = false;
                        result.StatusText = "Organization Key is invalid.";

                        }
                    }
                
            return result;
            }
        private SurveyRequestResultBO RePublishRelatedFormSurvey(SurveyInfoBO pRequestMessage)
            {
            throw new NotImplementedException();
            }
        private SurveyRequestResultBO PublishRelatedFormSurvey(SurveyInfoBO pRequestMessage)
            {

            SurveyRequestResultBO SurveyRequestResultBO = new Web.Common.BusinessObject.SurveyRequestResultBO();
            string ParentId = "";
            // 1- breck down the xml to n views
            List<string> XmlList = new List<string>();
            XmlList = XmlChunking(pRequestMessage.XML);  

            // 2- call publish() with each of the views
            foreach (string Xml in XmlList)
            {
            XDocument xdoc = XDocument.Parse(Xml);
            SurveyInfoBO SurveyInfoBO = new SurveyInfoBO();
            XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
            
            int ViewId;
            int.TryParse(ViewElement.Attribute("ViewId").Value.ToString(), out ViewId);

            SurveyInfoBO = pRequestMessage;
            SurveyInfoBO.XML = Xml;
            SurveyInfoBO.SurveyName = ViewElement.Attribute("Name").Value.ToString();
            SurveyInfoBO.ViewId = ViewId;
            SurveyInfoBO.ParentId = ParentId;
            SurveyInfoBO.OwnerId = 2; //HardCode
            SurveyRequestResultBO = Publish(SurveyInfoBO);
            ParentId = SurveyRequestResultBO.URL.Split('/').Last();
            }

            return SurveyRequestResultBO;
            }

        private List<string> XmlChunking(string Xml)
            {
            List<string> XmlList = new List<string>();
            XDocument xdoc = XDocument.Parse(Xml);
            XDocument xdoc1 = XDocument.Parse(Xml);
           
            xdoc.Descendants("View").Remove();

            foreach (XElement Xelement in xdoc1.Descendants("Project").Elements("View"))
                {

                //xdoc.Element("Project").Add(Xelement);
                xdoc.Root.Element("Project").Add(Xelement);
                XmlList.Add(xdoc.ToString());
                xdoc.Descendants("View").Remove();
                }

            return XmlList;
            }
        #endregion
        
        
      
    }
}
