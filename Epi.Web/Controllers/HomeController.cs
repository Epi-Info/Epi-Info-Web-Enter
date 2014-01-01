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
    public class HomeController : Controller
    {
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        private IEnumerable<XElement> PageFields;
        private string RequiredList = "";
        private int NumberOfPages = -1;
        private int PageSize = -1;
        private int NumberOfResponses = -1;
        List<KeyValuePair<int, string>> Columns = new List<KeyValuePair<int, string>>();

        /// <summary>
        /// injecting surveyFacade to the constructor 
        /// </summary>
        /// <param name="surveyFacade"></param>
        public HomeController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        public ActionResult Default()
        {
            return View("Default");
        }

        [HttpGet]
        public ActionResult Index(string surveyid)
        {
            //    return View();
            //}

            ///// <summary>
            ///// Accept SurveyId as parameter, 
            ///// 
            ///// Get the SurveyInfoResponse by GetSurveyInfo call and convert it to a SurveyInfoModel object
            ///// pump the SurveyInfoModel to the "SurveyIntroduction" view
            ///// </summary>
            ///// <param name="surveyid"></param>
            ///// <returns></returns>
            //[HttpGet]
            //public ActionResult ListForms()
            //{
            Guid UserId = new Guid();
            try
            {
                string SurveyMode = "";
                //SurveyInfoModel surveyInfoModel = GetSurveyInfo(surveyid);
                List<FormInfoModel> listOfformInfoModel = GetFormsInfoList(UserId);
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                //if (surveyInfoModel.IntroductionText != null)
                //{
                //    string introText = regex.Replace(surveyInfoModel.IntroductionText.Replace("  ", " &nbsp;"), "<br />");
                //    surveyInfoModel.IntroductionText = MvcHtmlString.Create(introText).ToString();
                //}

                //if (surveyInfoModel.IsDraftMode)
                //{
                //    surveyInfoModel.IsDraftModeStyleClass = "draft";
                //    SurveyMode = "draft";
                //}
                //else
                //{
                //    surveyInfoModel.IsDraftModeStyleClass = "final";
                //    SurveyMode = "final";
                //}
                bool IsMobileDevice = false;
                IsMobileDevice = this.Request.Browser.IsMobileDevice;
                Omniture OmnitureObj = Epi.Web.MVC.Utility.OmnitureHelper.GetSettings(SurveyMode, IsMobileDevice);

                ViewBag.Omniture = OmnitureObj;

                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                ViewBag.Version = version;

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, listOfformInfoModel);
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage(ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        /// <summary>
        /// redirecting to Survey controller to action method Index
        /// </summary>
        /// <param name="surveyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string surveyid, string AddNewFormId, string EditForm)
        {
        if (!string.IsNullOrEmpty(EditForm))
                {
                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(EditForm);
                string ChildRecordId = GetChildRecordId(surveyAnswerDTO);
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ChildRecordId, PageNumber = 1 });
            }
            bool IsMobileDevice = this.Request.Browser.IsMobileDevice;


            if (IsMobileDevice == false)
            {
                IsMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
            }

            //if (IsMobileDevice == true)
            // {
            //     if (!string.IsNullOrEmpty(surveyid))
            //     {
            //         //return RedirectToAction(new { Controller = "FormResponse", Action = "Index", surveyid = surveyid });
            //         return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "FormResponse", new { surveyid = surveyid  });
            //     }
            // }

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

                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, ResponseID.ToString(), form, SurveyAnswer, false, false, 0);
                }
                catch (Exception ex)
                {
                    // do nothing so that processing
                    // can continue
                }
            }
            else
            {
                SurveyAnswer.XML = CreateResponseDocument(xdoc, SurveyAnswer.XML);//, RequiredList);
                form.RequiredFieldsList = RequiredList;
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 0);
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

        //[HttpPost]
        //public ActionResult Index(List<FormInfoModel> model) {
        //    return View("ListResponses", model);
        //}

        [HttpGet]

        public ActionResult ReadResponseInfo(string formid, int page = 1)//List<FormInfoModel> ModelList, string formid)
        {
            bool IsMobileDevice = this.Request.Browser.IsMobileDevice;

            var model = new FormResponseInfoModel();


            model = GetFormResponseInfoModel(formid, page);

            if (IsMobileDevice == false)
            {
                return PartialView("ListResponses", model);
            }
            else
            {
                return View("ListResponses", model);
            }
        }

        [HttpGet]

        public void Delete(string ResponseId)//List<FormInfoModel> ModelList, string formid)
        {
            SurveyAnswerRequest SARequest = new SurveyAnswerRequest();
            SARequest.SurveyAnswerList.Add(new SurveyAnswerDTO() { ResponseId = ResponseId });

            _isurveyFacade.DeleteResponse(SARequest);


        }

        //[HttpPost]
        //public ActionResult ReadResponseInfo(List<FormInfoModel> ModelList, string formid, string pageNumber)
        //{
        //    return View("");
        //}
        //    bool IsMobileDevice = this.Request.Browser.IsMobileDevice;

        //    var model = new FormResponseInfoModel();

        //    FormInfoModel NewModel = ModelList.Single(x => x.FormId == formid);
        //    model.FormInfoModel = NewModel;

        //    SurveyAnswerResponse SurveyAnswerResponse = GetFormResponseList(formid, 1);

        //    model.SurveyAnswerResponse = SurveyAnswerResponse;

        //    if (IsMobileDevice == false)
        //    {
        //        return PartialView("ListResponses", model);
        //    }
        //    else
        //    {
        //        return View("ListResponses", model);
        //    }
        //}

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID)
                && TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] != null
                && !string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString())
                )
            {
                string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();

                //TODO: Now repopulating the TempData (by reassigning to responseId) so it persisits, later we will need to find a better 
                //way to replace it. 
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseId;
                return _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
            }

            return result;
        }
        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")
                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
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

        public List<FormInfoModel> GetFormsInfoList(Guid UserId)
        {
            FormsInfoRequest formReq = new FormsInfoRequest();
            formReq.Criteria.UserId = 2;//Hard coded user for now.
            // formReq.Criteria.UserId = UserId;
            //define filter criteria here.
            //define sorting criteria here.
            List<FormInfoModel> listOfFormsInfoModel = _isurveyFacade.GetFormsInfoModelList(formReq);



            return listOfFormsInfoModel;
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

        public FormResponseInfoModel GetFormResponseInfoModel(string SurveyId, int PageNumber)
        {

            FormResponseInfoModel FormResponseInfoModel = new FormResponseInfoModel();
            if (!string.IsNullOrEmpty(SurveyId))
            {
                SurveyAnswerRequest FormResponseReq = new SurveyAnswerRequest();
                FormSettingRequest FormSettingReq = new Common.Message.FormSettingRequest();

                //Populating the request
                FormResponseReq.Criteria.SurveyId = SurveyId.ToString();
                FormResponseReq.Criteria.PageNumber = PageNumber;
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
                FormResponseInfoModel.PageSize = ReadPageSize();
                FormResponseInfoModel.NumberOfResponses = FormResponseList.NumberOfResponses;
                FormResponseInfoModel.CurrentPage = PageNumber;
            }
            return FormResponseInfoModel;
        }

        private int ReadPageSize() 
        {
            return Convert.ToInt16(WebConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"].ToString());
        }

    //  [HttpPost]

    //    public ActionResult Edit(string ResId)
    //    {
    ////    Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(ResId);

    //    return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, Epi.Web.MVC.Constants.Constant.SURVEY_CONTROLLER, new { responseid = ResId, PageNumber = 1 });
    //    }
      private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId)
          {
          Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

          //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
          result = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

          return result;

          }
    }
}
