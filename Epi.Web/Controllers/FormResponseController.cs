using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using Epi.Core.EnterInterpreter;
using System.Collections.Generic;
using System.Web.Security;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Reflection;
using System.Diagnostics;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Utility;
using Epi.Web.Common.DTO;
using System.Web.Configuration;
namespace Epi.Web.MVC.Controllers
{
    [Authorize]
    public class FormResponseController : Controller
    {
        //
        // GET: /FormResponse/

        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        private IEnumerable<XElement> PageFields;
        private string RequiredList = "";
        List<KeyValuePair<int, string>> Columns = new List<KeyValuePair<int, string>>();
        private int NumberOfPages = -1;
        private int NumberOfResponses = -1;
        public FormResponseController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;

        }



        [HttpGet]
        //string responseid,string SurveyId, int ViewId, string CurrentPage
        public ActionResult Index(string formid, int Pagenumber = 1, int ViewId=0, string responseid="")
        {
        if (ViewId == 0) {


        Session["RootFormId"] = formid;
            
            }
        if (ViewId == 0 && string.IsNullOrEmpty(responseid))
            {
            var model = new FormResponseInfoModel();

            model = GetFormResponseInfoModel(formid, Pagenumber);

            return View("Index", model);
            }
        else {

        int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
         
        bool IsMobileDevice = this.Request.Browser.IsMobileDevice;



        List<FormsHierarchyDTO> FormsHierarchy = GetFormsHierarchy();
        int RequestedViewId;
        RequestedViewId = ViewId;

        Session["RequestedViewId"] = RequestedViewId;
        SurveyModel SurveyModel = new SurveyModel();

        SurveyModel.RequestedViewId = 10;

        SurveyModel.RelateModel = Mapper.ToRelateModel(FormsHierarchy, formid);
        SurveyModel.RequestedViewId = RequestedViewId;


        var RelateSurveyId = FormsHierarchy.Single(x => x.ViewId == ViewId);

        SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();


        SurveyModel.FormResponseInfoModel = GetFormResponseInfoModel(RelateSurveyId.FormId, responseid);
        SurveyModel.FormResponseInfoModel.NumberOfResponses = SurveyModel.FormResponseInfoModel.ResponsesList.Count();





        Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(RelateSurveyId.ResponseIds[0].ResponseId);
        var form = _isurveyFacade.GetSurveyFormData(RelateSurveyId.ResponseIds[0].SurveyId, 1, surveyAnswerDTO, IsMobileDevice, null);
        SurveyModel.Form = form;

        SurveyModel.FormResponseInfoModel.FormInfoModel.FormName = form.SurveyInfo.SurveyName;
        SurveyModel.FormResponseInfoModel.FormInfoModel.FormId = form.SurveyInfo.SurveyId;

        return View("Index", SurveyModel.FormResponseInfoModel);
            
            }
        }

        [HttpPost]
        public ActionResult Index(string surveyid, string AddNewFormId, string EditForm)
            {
            if (!string.IsNullOrEmpty(EditForm))
                {
                //Session["RootFormId"] = surveyid;
                Session["RootResponseId"] = EditForm;

                Session["IsEditMode"] = true;
                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
                string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1, Edit = "Edit" });
                }
            else
                {
                Session["IsEditMode"] = false;
                }
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            if (!string.IsNullOrEmpty(EditForm))
                {
                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
                surveyAnswerDTO.Status = 1;
                string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1, Edit = "Edit" });
                }
            bool IsMobileDevice = this.Request.Browser.IsMobileDevice;


            if (IsMobileDevice == false)
                {
                IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }

            FormsAuthentication.SetAuthCookie("BeginSurvey", false);

            //create the responseid
            Guid ResponseID = Guid.NewGuid();
            TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

            // create the first survey response
            // Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyModel.SurveyId, ResponseID.ToString());
            Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(AddNewFormId, ResponseID.ToString(), 2);
            SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);

            // set the survey answer to be production or test 
            SurveyAnswer.IsDraftMode = surveyInfoModel.IsDraftMode;
            XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);

            MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, IsMobileDevice);

            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")
                                 select _FieldTypeID;

            TempData["Width"] = form.Width + 100;

            XDocument xdocResponse = XDocument.Parse(SurveyAnswer.XML);

            XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
            string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();

            form.FormCheckCodeObj = form.GetCheckCodeObj(xdoc, xdocResponse, checkcode);

            ///////////////////////////// Execute - Record Before - start//////////////////////
            Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();
            EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=record&event=before&identifier=");
            if (FunctionObject_B != null && !FunctionObject_B.IsNull())
                {
                try
                    {
                    SurveyAnswer.XML = CreateResponseDocument(xdoc, SurveyAnswer.XML);
                    //SurveyAnswer.XML = Epi.Web.MVC.Utility.SurveyHelper.CreateResponseDocument(xdoc, SurveyAnswer.XML, RequiredList);

                    form.RequiredFieldsList = this.RequiredList;
                    FunctionObject_B.Context.HiddenFieldList = form.HiddenFieldsList;
                    FunctionObject_B.Context.HighlightedFieldList = form.HighlightedFieldsList;
                    FunctionObject_B.Context.DisabledFieldList = form.DisabledFieldsList;
                    FunctionObject_B.Context.RequiredFieldList = form.RequiredFieldsList;

                    FunctionObject_B.Execute();

                    // field list
                    form.HiddenFieldsList = FunctionObject_B.Context.HiddenFieldList;
                    form.HighlightedFieldsList = FunctionObject_B.Context.HighlightedFieldList;
                    form.DisabledFieldsList = FunctionObject_B.Context.DisabledFieldList;
                    form.RequiredFieldsList = FunctionObject_B.Context.RequiredFieldList;


                    ContextDetailList = Epi.Web.MVC.Utility.SurveyHelper.GetContextDetailList(FunctionObject_B);
                    form = Epi.Web.MVC.Utility.SurveyHelper.UpdateControlsValuesFromContext(form, ContextDetailList);

                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, ResponseID.ToString(), form, SurveyAnswer, false, false, 0, UserId);
                    }
                catch (Exception ex)
                    {
                    // do nothing so that processing
                    // can continue
                    }
                }
            else
                {
                SurveyAnswer.XML = CreateResponseDocument(xdoc, SurveyAnswer.XML);
                //SurveyAnswer.XML = Epi.Web.MVC.Utility.SurveyHelper.CreateResponseDocument(xdoc, SurveyAnswer.XML, RequiredList);
                form.RequiredFieldsList = RequiredList;
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 0, UserId);
                }

            SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(SurveyAnswer.ResponseId).SurveyResponseList[0];

            ///////////////////////////// Execute - Record Before - End//////////////////////
            //string page;
            // return RedirectToAction(Epi.Web.Models.Constants.Constant.INDEX, Epi.Web.Models.Constants.Constant.SURVEY_CONTROLLER, new {id="page" });
            return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ResponseID, PageNumber = 1 });
            //}
            //catch (Exception ex)
            //{
            //    //Epi.Web.Utility.ExceptionMessage.SendLogMessage(ex, this.HttpContext);
            //    //return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            //}

            }
         

        public FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, int PageNumber)
        {

            FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();
            SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
            FormSettingRequest FormSettingReq = new Common.Message.FormSettingRequest();

            //Populating the request
            FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
            FormResponseReq.Criteria.PageNumber = PageNumber;
            FormResponseReq.Criteria.IsMobile = true;
            FormSettingReq.FormInfo.FormId = new Guid(SurveyId).ToString();

            //Getting Column Name  List
            FormSettingResponse FormSettingResponse = _isurveyFacade.GetFormSettings(FormSettingReq);
            Columns = FormSettingResponse.FormSetting.ColumnNameList.ToList();
            Columns.Sort(Compare);

            // Setting  Column Name  List
            FormResponseInfoModel.Columns = Columns;

            //Getting Resposes
            SurveyAnswerResponse FormResponseList = _isurveyFacade.GetFormResponseList(FormResponseReq);

            //Setting Resposes List
            List<ResponseModel> ResponseList = new List<ResponseModel>();
            foreach (var item in FormResponseList.SurveyResponseList)
            {
                ResponseList.Add(ConvertXMLToModel(item, Columns));
            }

            FormResponseInfoModel.ResponsesList = ResponseList;
            //Setting Form Info 
            FormResponseInfoModel.FormInfoModel = Mapper.ToFormInfoModel(FormResponseList.FormInfo);
            //Setting Additional Data

            FormResponseInfoModel.NumberOfPages = FormResponseList.NumberOfPages;
            FormResponseInfoModel.PageSize = FormResponseList.PageSize;
            FormResponseInfoModel.NumberOfResponses = FormResponseList.NumberOfResponses;
            FormResponseInfoModel.CurrentPage = PageNumber;
            return FormResponseInfoModel;
        }

        private int Compare(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private ResponseModel ConvertXMLToModel(SurveyAnswerDTO item, List<KeyValuePair<int, string>> Columns)
        {
        ResponseModel ResponseModel = new Models.ResponseModel();


        var MetaDataColumns = Epi.Web.MVC.Constants.Constant.MetaDaTaColumnNames();

        try
            {
            ResponseModel.Column0 = item.ResponseId;
            ResponseModel.IsLocked = item.IsLocked;
            IEnumerable<XElement> nodes;
            var document = XDocument.Parse(item.XML);
            if (MetaDataColumns.Contains(Columns[0].Value.ToString()))
                {

                ResponseModel.Column1 = GetColumnValue(item, Columns[0].Value.ToString());
                }
            else
                {
                nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[0].Value.ToString());
                ResponseModel.Column1 = nodes.First().Value;
                }
            if (Columns.Count >= 2)
                {
                if (MetaDataColumns.Contains(Columns[1].Value.ToString()))
                    {

                    ResponseModel.Column2 = GetColumnValue(item, Columns[1].Value.ToString());
                    }
                else
                    {
                    nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[1].Value.ToString());
                    ResponseModel.Column2 = nodes.First().Value;
                    }
                }


            if (Columns.Count >= 3)
                {
                if (MetaDataColumns.Contains(Columns[2].Value.ToString()))
                    {

                    ResponseModel.Column3 = GetColumnValue(item, Columns[2].Value.ToString());
                    }
                else
                    {
                    nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[2].Value.ToString());
                    ResponseModel.Column3 = nodes.First().Value;
                    }
                }

            if (Columns.Count >= 4)
                {
                if (MetaDataColumns.Contains(Columns[3].Value.ToString()))
                    {

                    ResponseModel.Column4 = GetColumnValue(item, Columns[3].Value.ToString());
                    }
                else
                    {
                    nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[3].Value.ToString());
                    ResponseModel.Column4 = nodes.First().Value;
                    }
                }

            if (Columns.Count >= 5)
                {
                if (MetaDataColumns.Contains(Columns[4].Value.ToString()))
                    {

                    ResponseModel.Column5 = GetColumnValue(item, Columns[4].Value.ToString());
                    }
                else
                    {
                    nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[4].Value.ToString());
                    ResponseModel.Column5 = nodes.First().Value;
                    }
                }


            return ResponseModel;

            }
        catch (Exception Ex)
            {

            throw new Exception(Ex.Message);
            }

        }
        private string GetColumnValue(SurveyAnswerDTO item, string columnName)
            {
            string ColumnValue = "";
            switch (columnName)
                {
                case "UserEmail":
                    ColumnValue = item.UserEmail;
                    break;
                case "DateUpdated":
                    ColumnValue = item.DateUpdated.ToString();
                    break;
                case "DateCreated":
                    ColumnValue = item.DateCreated.ToString();
                    break;
                case "IsDraftMode":
                    ColumnValue = item.IsDraftMode.ToString();
                    break;
                }
            return ColumnValue;
            }
        

        private string CreateResponseDocument(XDocument pMetaData, string pXML)
        {
            XDocument XmlResponse = new XDocument();
            int NumberOfPages = GetNumberOfPages(pMetaData);
            for (int i = 0; NumberOfPages > i - 1; i++)
            {
                var _FieldsTypeIDs = from _FieldTypeID in
                                         pMetaData.Descendants("Field")
                                     where _FieldTypeID.Attribute("Position").Value == (i - 1).ToString()
                                     select _FieldTypeID;

                PageFields = _FieldsTypeIDs;

                XDocument CurrentPageXml = ToXDocument(CreateResponseXml("", false, i, ""));

                if (i == 0)
                {
                    XmlResponse = ToXDocument(CreateResponseXml("", true, i, ""));
                }
                else
                {
                    XmlResponse = MergeXml(XmlResponse, CurrentPageXml, i);
                }
            }

            return XmlResponse.ToString();
        }

        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")
                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
        }

        public XmlDocument CreateResponseXml(string SurveyId, bool AddRoot, int CurrentPage, string Pageid)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("SurveyResponse");

            if (CurrentPage == 0)
            {
                root.SetAttribute("SurveyId", SurveyId);
                root.SetAttribute("LastPageVisited", "1");
                root.SetAttribute("HiddenFieldsList", "");
                root.SetAttribute("HighlightedFieldsList", "");
                root.SetAttribute("DisabledFieldsList", "");
                root.SetAttribute("RequiredFieldsList", "");

                xml.AppendChild(root);
            }

            XmlElement PageRoot = xml.CreateElement("Page");
            if (CurrentPage != 0)
            {
                PageRoot.SetAttribute("PageNumber", CurrentPage.ToString());
                PageRoot.SetAttribute("PageId", Pageid);//Added PageId Attribute to the page node
                xml.AppendChild(PageRoot);
            }

            foreach (var Field in this.PageFields)
            {
                XmlElement child = xml.CreateElement(Epi.Web.MVC.Constants.Constant.RESPONSE_DETAILS);
                child.SetAttribute("QuestionName", Field.Attribute("Name").Value);
                child.InnerText = Field.Value;
                PageRoot.AppendChild(child);
                //Start Adding required controls to the list
                SetRequiredList(Field);
            }

            return xml;
        }

        public static XDocument ToXDocument(XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
        {
            XDocument xdoc = XDocument.Parse(SavedXml.ToString());
            XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");

            if (oldXElement == null)
            {
                SavedXml.Root.Add(CurrentPageResponseXml.Elements());
                return SavedXml;
            }

            else
            {
                oldXElement.Remove();
                xdoc.Root.Add(CurrentPageResponseXml.Elements());
                return xdoc;
            }
        }

        public void SetRequiredList(XElement _Fields)
        {
            bool isRequired = false;
            string value = _Fields.Attribute("IsRequired").Value;

            if (bool.TryParse(value, out isRequired))
            {
                if (isRequired)
                {
                    if (!RequiredList.Contains(_Fields.Attribute("Name").Value))
                    {
                        if (RequiredList != "")
                        {
                            RequiredList = RequiredList + "," + _Fields.Attribute("Name").Value.ToLower();
                        }
                        else
                        {
                            RequiredList = _Fields.Attribute("Name").Value.ToLower();
                        }
                    }
                }
            }
        }

        public SurveyInfoModel GetSurveyInfo(string SurveyId)
        {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;
        }
        private string GetChildRecordId(SurveyAnswerDTO surveyAnswerDTO)
            {
            //SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
            //SurveyAnswerResponse SurveyAnswerResponse = new SurveyAnswerResponse();
            //string ChildId = Guid.NewGuid().ToString();
            //surveyAnswerDTO.ParentRecordId = surveyAnswerDTO.ResponseId;
            //surveyAnswerDTO.ResponseId = ChildId;
            //SurveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            //string result = ChildId;

            ////responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            //SurveyAnswerRequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString()); ;
            //SurveyAnswerRequest.RequestId = ChildId;
            //SurveyAnswerRequest.Action = "Create";
            //SurveyAnswerResponse = _isurveyFacade.SetChildRecord(SurveyAnswerRequest);

            //return result;
            SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse SurveyAnswerResponse = new SurveyAnswerResponse();
            string ChildId = Guid.NewGuid().ToString();
            surveyAnswerDTO.ParentRecordId = surveyAnswerDTO.ResponseId;
            surveyAnswerDTO.ResponseId = ChildId;
            surveyAnswerDTO.Status = 1;
            SurveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            string result;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            string Id = Session["UserId"].ToString();
            SurveyAnswerRequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Id);//_UserId;
            SurveyAnswerRequest.RequestId = ChildId;
            SurveyAnswerRequest.Action = "CreateMulti";
            SurveyAnswerResponse = _isurveyFacade.SetChildRecord(SurveyAnswerRequest);
            result = SurveyAnswerResponse.SurveyResponseList[0].ResponseId.ToString();
            return result;
            }
        private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId)
            {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            result = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

            return result;

            }


        /// <summary>
        /// Following Action method takes ResponseId as a parameter and deletes the response.
        /// For now it returns nothing as a confirmation of deletion, we may add some error/success
        /// messages later. TBD
        /// </summary>
        /// <param name="ResponseId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string ResponseId)
            {
            SurveyAnswerRequest SARequest = new SurveyAnswerRequest();
            SARequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = ResponseId });
            string Id = Session["UserId"].ToString();
            SARequest.Criteria.UserId = SurveyHelper.GetDecryptUserId(Id);

            SurveyAnswerResponse SAResponse = _isurveyFacade.DeleteResponse(SARequest);

            return Json(string.Empty);


            }
        private List<FormsHierarchyDTO> GetFormsHierarchy()
            {
            FormsHierarchyResponse FormsHierarchyResponse = new FormsHierarchyResponse();
            FormsHierarchyRequest FormsHierarchyRequest = new FormsHierarchyRequest();
            if (Session["RootFormId"] != null && Session["RootResponseId"] != null)
                {
                FormsHierarchyRequest.SurveyInfo.FormId = Session["RootFormId"].ToString();
                FormsHierarchyRequest.SurveyResponseInfo.ResponseId = Session["RootResponseId"].ToString();
                FormsHierarchyResponse = _isurveyFacade.GetFormsHierarchy(FormsHierarchyRequest);
                }
            return FormsHierarchyResponse.FormsHierarchy;
            }
        
        private FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, string ResponseId)
            {
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();

            SurveyResponseXML SurveyResponseXML = new SurveyResponseXML();
            if (!string.IsNullOrEmpty(SurveyId))
                {
                SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
                FormSettingRequest FormSettingReq = new Common.Message.FormSettingRequest();

                //Populating the request

                FormSettingReq.FormInfo.FormId = SurveyId;
                FormSettingReq.FormInfo.UserId = UserId;
                //Getting Column Name  List
                FormSettingResponse FormSettingResponse = _isurveyFacade.GetFormSettings(FormSettingReq);
                Columns = FormSettingResponse.FormSetting.ColumnNameList.ToList();
                Columns.Sort(Compare);

                // Setting  Column Name  List
                FormResponseInfoModel.Columns = Columns;

                //Getting Resposes
                FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
                FormResponseReq.Criteria.SurveyAnswerIdList.Add(ResponseId);

                FormResponseReq.Criteria.PageNumber = 1;
                FormResponseReq.Criteria.UserId = UserId;
                SurveyAnswerResponse FormResponseList = _isurveyFacade.GetResponsesByRelatedFormId(FormResponseReq);

                //Setting Resposes List
                List<ResponseModel> ResponseList = new List<ResponseModel>();
                foreach (var item in FormResponseList.SurveyResponseList)
                    {
                    ResponseList.Add(SurveyResponseXML.ConvertXMLToModel(item, Columns));
                    }

                FormResponseInfoModel.ResponsesList = ResponseList;
                //Setting Form Info 
                //  FormResponseInfoModel.FormInfoModel = Mapper.ToFormInfoModel(FormResponseList.FormInfo);
                //Setting Additional Data

                FormResponseInfoModel.NumberOfPages = FormResponseList.NumberOfPages;
                FormResponseInfoModel.PageSize = ReadPageSize();
                FormResponseInfoModel.NumberOfResponses = FormResponseList.NumberOfResponses;
                FormResponseInfoModel.CurrentPage = 1;
                }
            return FormResponseInfoModel;
            }
       
        private int ReadPageSize()
            {
            return Convert.ToInt16(WebConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"].ToString());
            }
      
    }
}
