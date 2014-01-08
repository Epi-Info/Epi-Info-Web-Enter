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

namespace Epi.Web.MVC.Controllers
{
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
        public ActionResult Index(string formid, int pagenumber = 1)
        {

            var model = new FormResponseInfoModel();

            model = GetFormResponseInfoModel(formid, pagenumber);

            return View("Index", model);
        }
        

        //public List<ResponseModel> GetFormResponseList(string SurveyId, int PageNumber)
        //{

        //    SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
        //    FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
        //    FormResponseReq.Criteria.PageNumber = PageNumber;
        //    SurveyAnswerResponse FormResponseList = _isurveyFacade.GetFormResponseList(FormResponseReq);


        //    Columns.Add(new KeyValuePair<int, string>(6, "CaseID"));
        //    Columns.Add(new KeyValuePair<int, string>(2, "DateofInterview"));
        //    Columns.Add(new KeyValuePair<int, string>(3, "FirstName"));
        //    Columns.Add(new KeyValuePair<int, string>(1, "LastName"));
        //    Columns.Add(new KeyValuePair<int, string>(5, "Sex"));

        //    Columns.Add(new KeyValuePair<int, string>(10, "IsLocked"));

        //    Columns.Sort(Compare);

        //    NumberOfPages =  FormResponseList.NumberOfPages;
        //    NumberOfResponses = FormResponseList.NumberOfResponses;

        //    List<ResponseModel> ResponseList = new List<ResponseModel>();

        //    foreach (var item in FormResponseList.SurveyResponseList)
        //    {
        //        ResponseList.Add(ConvertXMLToModel(item, Columns));

        //    }

        //    return ResponseList;
        //}


        public FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, int PageNumber)
        {

            FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();
            SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
            FormSettingRequest FormSettingReq = new Common.Message.FormSettingRequest();

            //Populating the request
            FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
            FormResponseReq.Criteria.PageNumber = PageNumber;
            FormResponseReq.Criteria.IsMobile = true;
            FormSettingReq.FormSetting.FormId = new Guid(SurveyId);

            //Getting Column Name  List
            FormSettingResponse FormSettingResponse = _isurveyFacade.GetResponseColumnNameList(FormSettingReq);
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



            ResponseModel.Column0 = item.ResponseId;
            ResponseModel.IsLocked = item.IsLocked;

            var document = XDocument.Parse(item.XML);

            var nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[0].Value.ToString());

            ResponseModel.Column1 = nodes.First().Value;

            nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[1].Value.ToString());

            ResponseModel.Column2 = nodes.First().Value;

            nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[2].Value.ToString());

            ResponseModel.Column3 = nodes.First().Value;

            nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[3].Value.ToString());

            ResponseModel.Column4 = nodes.First().Value;

            nodes = document.Descendants().Where(e => e.Name.LocalName.StartsWith("ResponseDetail") && e.Attribute("QuestionName").Value == Columns[4].Value.ToString());

            ResponseModel.Column5 = nodes.First().Value;

            return ResponseModel;

        }

        [HttpPost]
        public ActionResult Index(string surveyid, string AddNewFormId, string EditForm)
        {
        int UserId = 2;
        if (!string.IsNullOrEmpty(EditForm))
            {
            Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
            surveyAnswerDTO.Status = 1;
            string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
            return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1 });
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
            Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(AddNewFormId, ResponseID.ToString(),2);
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
            SurveyAnswerRequest SurveyAnswerRequest = new SurveyAnswerRequest();
            SurveyAnswerResponse SurveyAnswerResponse = new SurveyAnswerResponse();
            string ChildId = Guid.NewGuid().ToString();
            surveyAnswerDTO.ParentRecordId = surveyAnswerDTO.ResponseId;
            surveyAnswerDTO.ResponseId = ChildId;
            SurveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            string result = ChildId;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            SurveyAnswerRequest.Criteria.UserId = 2;
            SurveyAnswerRequest.RequestId = ChildId;
            SurveyAnswerRequest.Action = "Create";
            SurveyAnswerResponse = _isurveyFacade.SetChildRecord(SurveyAnswerRequest);

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
            SARequest.Criteria.UserId = 2; //TBD
            SurveyAnswerResponse SAResponse = _isurveyFacade.DeleteResponse(SARequest);

            return Json(string.Empty);



        }
    }
}
