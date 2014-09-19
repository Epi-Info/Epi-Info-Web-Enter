using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using System.Configuration;
using Epi.Web.Enter.Common.Extension;
using Epi.Web.Enter.Common.Criteria;
namespace Epi.Web.BLL
{
    public class SurveyResponse
    {
        private Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao;

        public SurveyResponse(Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao)
        {
            this.SurveyResponseDao = pSurveyResponseDao;
        }

        public List<SurveyResponseBO> GetSurveyResponseById(List<String> pId, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(pId, UserPublishKey);
            return result;
        }

        public List<SurveyResponseBO> GetFormResponseListById(string FormId, int PageNumber, bool IsMobile)
        {
            List<SurveyResponseBO> result = null;

            int PageSize;
            if (IsMobile)
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            result = this.SurveyResponseDao.GetFormResponseByFormId(FormId, PageNumber, PageSize);
            return result;
        }

        public List<SurveyResponseBO> GetFormResponseListById(SurveyAnswerCriteria criteria)
        {
            List<SurveyResponseBO> result = null;

            //int PageSize;
            if (criteria.IsMobile)
            {
                criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            result = this.SurveyResponseDao.GetFormResponseByFormId(criteria);
            return result;
        }
        public int GetNumberOfPages(string FormId, bool IsMobile)
        {
            int PageSize;
            if (IsMobile)
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            int result = this.SurveyResponseDao.GetFormResponseCount(FormId);
            if (PageSize > 0)
            {
                result = (result + PageSize - 1) / PageSize;
            }
            return result;
        }

        //Validate User
        public bool ValidateUser(UserAuthenticationRequestBO PassCodeBoObj)
        {
            string PassCode = PassCodeBoObj.PassCode;
            string ResponseId = PassCodeBoObj.ResponseId;
            List<string> ResponseIdList = new List<string>();
            ResponseIdList.Add(PassCodeBoObj.ResponseId);

            UserAuthenticationResponseBO results = this.SurveyResponseDao.GetAuthenticationResponse(PassCodeBoObj);



            bool ISValidUser = false;

            if (results != null && !string.IsNullOrEmpty(PassCode))
            {

                if (results.PassCode == PassCode)
                {
                    ISValidUser = true;


                }
                else
                {
                    ISValidUser = false;
                }
            }
            return ISValidUser;
        }
        //Save Pass code 
        public void SavePassCode(UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationRequestBO result = pValue;
            this.SurveyResponseDao.UpdatePassCode(pValue);



        }
        // Get Authentication Response
        public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationResponseBO result = this.SurveyResponseDao.GetAuthenticationResponse(pValue);

            return result;

        }
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<String> pSurveyIdList, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponseBySurveyId(pSurveyIdList, UserPublishKey);
            return result;
        }

        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted, pStatusId);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponse(pValue);
            return result;
        }
        public List<SurveyResponseBO> InsertSurveyResponse(List<SurveyResponseBO> pValue, int UserId, bool IsNewRecord = false)
        {

            foreach (var item in pValue)
            {
                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                ResponseXmlBO.User = UserId;
                ResponseXmlBO.ResponseId = item.ResponseId;
                ResponseXmlBO.Xml = item.XML;
                ResponseXmlBO.IsNewRecord = IsNewRecord;
                this.SurveyResponseDao.InsertResponseXml(ResponseXmlBO);

            }

            return pValue;
        }



        public SurveyResponseBO InsertChildSurveyResponse(SurveyResponseBO pValue, SurveyInfoBO ParentSurveyInfo, string RelateParentId)
        {

            SurveyResponseBO result = pValue;
            pValue.ParentId = ParentSurveyInfo.ParentId;
            pValue.RelateParentId = RelateParentId;
            this.SurveyResponseDao.InsertChildSurveyResponse(pValue);
            return result;
        }

        public SurveyResponseBO UpdateSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            //Check if this respose has prent
            string ParentId = SurveyResponseDao.GetResponseParentId(pValue.ResponseId);
            if (!string.IsNullOrEmpty(ParentId) && pValue.Status == 2)
            {
                //read the child 

                SurveyResponseBO Child = this.SurveyResponseDao.GetSingleResponse(pValue.ResponseId);
                // read the parent
                SurveyResponseBO Parent = this.SurveyResponseDao.GetSingleResponse(ParentId);
                //copy and update
                Parent.XML = Child.XML;
                this.SurveyResponseDao.UpdateSurveyResponse(Parent);
                result = Parent;
                //Check if this child has a related form (subchild)
                List<SurveyResponseBO> Children = this.GetResponsesHierarchyIdsByRootId(Child.ResponseId);
                if (Children.Count() > 1)
                {
                    SurveyResponseBO NewChild = Children[1];
                    NewChild.RelateParentId = Parent.ResponseId;
                    this.SurveyResponseDao.UpdateSurveyResponse(NewChild);
                }
                // Set  child recod UserId
                Child.UserId = pValue.UserId;
                // delete the child
                this.DeleteSingleSurveyResponse(Child);

            }
            else
            {
                //Check if the record existes.If it does update otherwise insert new 
                this.SurveyResponseDao.UpdateSurveyResponse(pValue);

                SurveyResponseBO SurveyResponseBO = SurveyResponseDao.GetResponseXml(pValue.ResponseId);



            }
            return result;
        }
        public List<SurveyResponseBO> UpdateSurveyResponse(List<SurveyResponseBO> pValue, int Status)
        {
            List<SurveyResponseBO> result = pValue;
            //Check if this respose has prent
            foreach (var Obj in pValue)
            {
                string ParentId = SurveyResponseDao.GetResponseParentId(Obj.ResponseId);
                if (!string.IsNullOrEmpty(ParentId) && Status == 2)
                {
                    //read the child 

                    SurveyResponseBO Child = this.SurveyResponseDao.GetSingleResponse(Obj.ResponseId);
                    // read the parent
                    SurveyResponseBO Parent = this.SurveyResponseDao.GetSingleResponse(ParentId);
                    //copy and update
                    Parent.XML = Child.XML;
                    Parent.Status = Status;
                    this.SurveyResponseDao.UpdateSurveyResponse(Parent);
                    result.Add(Parent);
                    // Set  child recod UserId
                    Child.UserId = Obj.UserId;
                    // delete the child
                    this.DeleteSurveyResponse(Child);

                }
                else
                {
                    Obj.Status = Status;
                    this.SurveyResponseDao.UpdateSurveyResponse(Obj);
                }
            }
            return result;
        }
        public bool DeleteSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSurveyResponse(pValue);
            result = true;

            return result;
        }
        public bool DeleteSurveyResponseInEditMode(SurveyResponseBO pValue)
        {
            bool result = false;
            List<SurveyResponseBO> Children = this.GetResponsesHierarchyIdsByRootId(pValue.ResponseId);

            foreach (var child in Children)
            {
                //Get the original copy of the xml
                SurveyResponseBO ResponseXml = this.SurveyResponseDao.GetResponseXml(child.ResponseId);
                if (!ResponseXml.IsNewRecord)
                {
                    child.XML = ResponseXml.XML;
                    this.SurveyResponseDao.UpdateSurveyResponse(child);
                }
                else
                {
                    child.UserId = pValue.UserId;
                    this.SurveyResponseDao.DeleteSurveyResponse(child);

                }
                // delete record from ResponseXml Table

                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                ResponseXmlBO.ResponseId = child.ResponseId;
                this.SurveyResponseDao.DeleteResponseXml(ResponseXmlBO);

            }

            result = true;

            return result;
        }
        public bool DeleteSingleSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSingleSurveyResponse(pValue);
            result = true;

            return result;
        }

        public PageInfoBO GetResponseSurveySize(List<string> SurveyResponseIdList, string SurveyId, DateTime pClosingDate, int BandwidthUsageFactor, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, SurveyId, pClosingDate, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);
            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;
        }

        public PageInfoBO GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseBySurveyIdSize(SurveyIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;

        }
        public PageInfoBO GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {

            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;
        }
        public int GetNumberOfResponses(string FormId)
        {

            int result = this.SurveyResponseDao.GetFormResponseCount(FormId);

            return result;
        }

        public List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesHierarchyIdsByRootId(RootId);


            return SurveyResponseBO;

        }



        public SurveyResponseBO GetFormResponseByParentRecordId(string ParentRecordId)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();

            SurveyResponseBO = this.SurveyResponseDao.GetFormResponseByParentRecordId(ParentRecordId);
            return SurveyResponseBO;
        }

        public List<SurveyResponseBO> GetAncestorResponseIdsByChildId(string ChildId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetAncestorResponseIdsByChildId(ChildId);


            return SurveyResponseBO;

        }

        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, string SurveyId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesByRelatedFormId(ResponseId, SurveyId);


            return SurveyResponseBO;

        }

        public SurveyResponseBO GetResponseXml(string ResponseId)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();

            SurveyResponseBO = this.SurveyResponseDao.GetResponseXml(ResponseId);

            return SurveyResponseBO;
        }

        public void DeleteResponseXml(ResponseXmlBO ResponseXmlBO)
        {

            this.SurveyResponseDao.DeleteResponseXml(ResponseXmlBO);
        }
    }
}
