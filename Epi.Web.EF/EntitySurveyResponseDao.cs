using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.Extension;
namespace Epi.Web.EF
{
    /// <summary>
    /// Entity Framework implementation of the ISurveyResponseDao interface.
    /// </summary>
    public class EntitySurveyResponseDao : ISurveyResponseDao
    {
        /// <summary>
        /// Gets a specific SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            if (SurveyResponseIdList.Count > 0)
            {
                foreach (string surveyResponseId in SurveyResponseIdList.Distinct())
                {
                    Guid Id = new Guid(surveyResponseId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.ResponseId == Id )));
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.SurveyResponses.ToList());
                }
            }

            if (PageNumber > 0 && PageSize > 0)
            {
                result.Sort(CompareByDateCreated);
                // remove the items to skip
                if (PageNumber * PageSize - PageSize > 0)
                {
                    result.RemoveRange(0, PageSize);
                }

             

                if (PageNumber * PageSize < result.Count)
                {
                    result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                }
            }


            return result;
        }


        public List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
         

            List<SurveyResponseBO> resultRows =  GetSurveyResponse(SurveyResponseIdList,  UserPublishKey,  PageNumber ,  PageSize );


            return resultRows;
        }

        /// <summary>
        /// Gets SurveyResponses per a SurveyId.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<string> SurveyIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            try {
            foreach (string surveyResponseId in SurveyIdList.Distinct())
            {
                Guid Id = new Guid(surveyResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.SurveyId == Id )));
                }
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (PageNumber > 0 && PageSize > 0)
            {
                result.Sort(CompareByDateCreated);
                // remove the items to skip
                if (PageNumber * PageSize - PageSize > 0)
                {
                    result.RemoveRange(0, PageSize);
                }

                if (PageNumber * PageSize < result.Count)
                {
                    result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                }
            }


            return result;
        }


        public List<SurveyResponseBO> GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey , int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
         
        

            List<SurveyResponseBO> resultRows =  GetSurveyResponseBySurveyId(SurveyIdList,  UserPublishKey,  PageNumber ,  PageSize );

        
            return resultRows;
         }

        /// <summary>
        /// Gets SurveyResponses depending on criteria.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        //public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1)
        //{
        //    List<SurveyResponseBO> result = new List<SurveyResponseBO>();
        //    List<SurveyResponse> responseList = new List<SurveyResponse>();

        //    if (SurveyAnswerIdList.Count > 0)
        //    {
        //        foreach (string surveyResponseId in SurveyAnswerIdList.Distinct())
        //        {
        //            try
        //            {
        //                Guid Id = new Guid(surveyResponseId);
                       

        //                using (var Context = DataObjectFactory.CreateContext())
        //                {
        //                    SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id );
        //                    if (surveyResponse != null)
        //                    {
        //                        responseList.Add(surveyResponse);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try{
        //        using (var Context = DataObjectFactory.CreateContext())
        //        {
        //            if (!string.IsNullOrEmpty(pSurveyId))
        //            {
        //                Guid Id = new Guid(pSurveyId);
        //                responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id).ToList();
        //            }
                    
                   
        //        }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw (ex);
        //        }
        //    }


        //    //if(! string.IsNullOrEmpty(pSurveyId))
        //    //{
        //    //    Guid Id = new Guid(pSurveyId);
        //    //    List<SurveyResponse> surveyList = new List<SurveyResponse>();
        //    //    surveyList.AddRange(responseList.Where(x => x.SurveyId == Id));
        //    //    responseList = surveyList;
        //    //}

        //    if (pStatusId > -1)
        //    {
        //        List<SurveyResponse> statusList = new List<SurveyResponse>();
        //        statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
        //        responseList = statusList;
        //    }

        //    if (pDateCompleted > DateTime.MinValue)
        //    {
        //        List<SurveyResponse> dateList = new List<SurveyResponse>();

        //        //dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
        //        dateList.AddRange(responseList.Where(x =>x.DateCompleted !=null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
        //        responseList = dateList;
        //    }

        //    result = Mapper.Map(responseList);

        //    if (PageNumber > 0 && PageSize > 0)
        //    {
        //        result.Sort(CompareByDateCreated);
        //        // remove the items to skip
        //        if (PageNumber * PageSize - PageSize > 0)
        //        {
        //            result.RemoveRange(0, PageSize);
        //        }

        //        if (PageNumber * PageSize < result.Count)
        //        {
        //            result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
        //        }
        //    }


        //    return result;
        //}
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1)
            {
            List<SurveyResponseBO> Finalresult = new List<SurveyResponseBO>();
            IEnumerable<SurveyResponseBO> result;
            List<SurveyResponse> responseList = new List<SurveyResponse>();

            if (SurveyAnswerIdList.Count > 0)
                {
                foreach (string surveyResponseId in SurveyAnswerIdList.Distinct())
                    {
                    try
                        {
                        Guid Id = new Guid(surveyResponseId);


                        using (var Context = DataObjectFactory.CreateContext())
                            {
                            SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id);
                            if (surveyResponse != null)
                                {
                                responseList.Add(surveyResponse);
                                }
                            }
                        }
                    catch (Exception ex)
                        {
                        throw (ex);
                        }
                    }
                }
            else
                {
                try
                    {
                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        if (!string.IsNullOrEmpty(pSurveyId))
                            {
                            Guid Id = new Guid(pSurveyId);
                            responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id).ToList();
                            }

                       
                        }
                    }
                catch (Exception ex)
                    {
                    throw (ex);
                    }
                }
 

            //if (pStatusId > -1)
            //    {
            //    List<SurveyResponse> statusList = new List<SurveyResponse>();
            //    statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
            //    responseList = statusList;
            //    }

            //if (pDateCompleted > DateTime.MinValue)
            //    {
            //    List<SurveyResponse> dateList = new List<SurveyResponse>();

            //    dateList.AddRange(responseList.Where(x => x.DateCompleted != null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
            //    responseList = dateList;
            //    }

             



           



            if (PageSize != -1 && PageNumber != -1)
                {
                result = Mapper.Map(responseList);

                foreach (SurveyResponseBO item in result)
                               {
                               List<SurveyResponseBO> ResponsesHierarchy = this.GetResponsesHierarchyIdsByRootId(item.ResponseId.ToString());

                               result.Where(x => x.ResponseId == item.ResponseId).Single().ResponseHierarchyIds = ResponsesHierarchy;

                               }


                result = result.Skip((PageNumber - 1) * PageSize).Take(PageSize);
                foreach (var item in result)
                    {
                    Finalresult.Add(item);

                    }
                return Finalresult;
                }
            else
                {


                Finalresult = Mapper.Map(responseList);
                foreach (SurveyResponseBO item in Finalresult)
                    {
                    List<SurveyResponseBO> ResponsesHierarchy = this.GetResponsesHierarchyIdsByRootId(item.ResponseId.ToString());

                    Finalresult.Where(x => x.ResponseId == item.ResponseId).Single().ResponseHierarchyIds = ResponsesHierarchy;

                    }

                return Finalresult;
                }





            }


        public List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
          

            List<SurveyResponseBO> resultRows =  GetSurveyResponse(SurveyAnswerIdList,  pSurveyId,pDateCompleted,pStatusId , PageNumber ,  PageSize );
 

            return resultRows;
         
         }

        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public  void InsertSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            try
            {
            using (var Context = DataObjectFactory.CreateContext() ) 
            {
                SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                //SurveyResponseEntity.Users.Add(new User { UserID = 2 });
                User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);
                SurveyResponseEntity.Users.Add(User);
                Context.AddToSurveyResponses(SurveyResponseEntity);
               
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
             
        }

        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void InsertChildSurveyResponse(SurveyResponseBO SurveyResponse )
            {



            try
                {
                using (var Context = DataObjectFactory.CreateContext())
                    {
                    SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                    User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);
                    SurveyResponseEntity.Users.Add(User);
                    Context.AddToSurveyResponses(SurveyResponseEntity);

                    Context.SaveChanges();
                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }

            }
        /// <summary>
        /// Updates a SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void UpdateSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            try{
            Guid Id = new Guid(SurveyResponse.ResponseId);

        //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id 
                            select response;

                var DataRow = Query.Single();

                if (!string.IsNullOrEmpty(SurveyResponse.RelateParentId))
                    {
                DataRow.RelateParentId = new Guid(SurveyResponse.RelateParentId);
                    }
                DataRow.ResponseXML = SurveyResponse.XML;
                //DataRow.DateCompleted = DateTime.Now;
                DataRow.DateCompleted = SurveyResponse.DateCompleted;
                DataRow.StatusId = SurveyResponse.Status;
                DataRow.DateUpdated = DateTime.Now;
             //   DataRow.ResponsePasscode = SurveyResponse.ResponsePassCode;
                DataRow.IsDraftMode = SurveyResponse.IsDraftMode;
                DataRow.ResponseXMLSize = RemoveWhitespace(SurveyResponse.XML).Length; 
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public static string RemoveWhitespace(string xml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }
        public void UpdatePassCode(UserAuthenticationRequestBO passcodeBO) {

            try 
            {
            Guid Id = new Guid(passcodeBO.ResponseId);

            //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id
                            select response;

                var DataRow = Query.Single();
                
                DataRow.ResponsePasscode = passcodeBO.PassCode;
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO UserAuthenticationRequestBO)
        {

            UserAuthenticationResponseBO UserAuthenticationResponseBO = Mapper.ToAuthenticationResponseBO(UserAuthenticationRequestBO);
            try
            {

                Guid Id = new Guid(UserAuthenticationRequestBO.ResponseId);


                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id);
                    if (surveyResponse != null)
                    {
                        UserAuthenticationResponseBO.PassCode = surveyResponse.ResponsePasscode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return UserAuthenticationResponseBO;

        }

        /// <summary>
        /// Deletes a SurveyResponse
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void DeleteSurveyResponse(SurveyResponseBO SurveyResponse)
        {


       
          
           
        try
            {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            Guid Id = new Guid(SurveyResponse.ResponseId);

            using (var Context = DataObjectFactory.CreateContext())
                {
                result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).OrderBy(x=>x.DateCreated).Traverse(x => x.SurveyResponse1));
                foreach (var Obj in result)
                    {
                 
                    if (!string.IsNullOrEmpty(Obj.ResponseId))
                        {
                        Guid NewId = new Guid(Obj.ResponseId);

                        User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                        SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == NewId);
                        Response.Users.Remove(User);

                        Context.SurveyResponses.DeleteObject(Response);
                     
                        Context.SaveChanges();
                        }

                  
                    }


                }
            }
        catch (Exception ex)
            {
            throw (ex);
            }



       }

        public void DeleteSurveyResponseInEditMode(SurveyResponseBO SurveyResponse)
            {


            string parentRecordId;


            try
                {
                List<SurveyResponseBO> result = new List<SurveyResponseBO>();
                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                    {
                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).OrderBy(x => x.DateCreated).Traverse(x => x.SurveyResponse1));
                    foreach (var Obj in result)
                        {
                        parentRecordId = "";
                        if (!string.IsNullOrEmpty(Obj.ParentRecordId))
                            {

                            parentRecordId = Obj.ParentRecordId;
                            }
                        if (!string.IsNullOrEmpty(Obj.ResponseId))
                            {
                            Guid NewId = new Guid(Obj.ResponseId);

                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == NewId);
                            Response.Users.Remove(User);

                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();
                            }

                        if (!string.IsNullOrEmpty(parentRecordId))
                            {
                            Guid pId = new Guid(parentRecordId);
                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == pId);
                            Response.Users.Remove(User);

                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();


                            }
                        }


                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }



            }



        public void DeleteSingleSurveyResponse(SurveyResponseBO SurveyResponse)
            {


            try
                {
                
                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                    {
                   
                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == Id);
                            Response.Users.Remove(User);

                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();
                            
                      }
                    }
                
            catch (Exception ex)
                {
                throw (ex);
                }



            }


        private static int CompareByDateCreated(SurveyResponseBO x, SurveyResponseBO y)
        {
            return x.DateCreated.CompareTo(y.DateCreated);
        }


        public List<SurveyResponseBO> GetFormResponseByFormId(string FormId, int PageNumber, int PageSize)
            {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            try
                {

                Guid Id = new Guid(FormId);

                    using (var Context = DataObjectFactory.CreateContext())
                        {

                        IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && string.IsNullOrEmpty(x.RelateParentId.ToString()) == true && x.StatusId > 1).OrderByDescending(x => x.DateUpdated);

                        SurveyResponseList = SurveyResponseList.Skip((PageNumber - 1) * PageSize).Take(PageSize);
                                                

                        foreach (SurveyResponse Response in SurveyResponseList)
                            {

                            result.Add(Mapper.Map(Response, Response.Users.First()));
                            
                            }
              

                        }
                    
                }
            catch (Exception ex)
                {
                throw (ex);
                }

          


            return result;
            }

        public int GetFormResponseCount(string FormId) 
            {
            int ResponseCount = 0;
            try
                {

                Guid Id = new Guid(FormId);

                using (var Context = DataObjectFactory.CreateContext())
                    {

                    IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && x.StatusId>1);
                    ResponseCount = SurveyResponseList.Count();

                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }



            return ResponseCount;
            
         
            }



        public SurveyResponseBO GetFormResponseByResponseId(string ResponseId)
            {

            SurveyResponseBO result = new SurveyResponseBO();

            try
                {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                    {

               
                    SurveyResponse Response = Context.SurveyResponses.ToList().Where(x => x.ResponseId == Id).First();
                    result  = (Mapper.Map(Response));

                   


                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }




            return result;
            }

        public string GetResponseParentId(string ResponseId)
            {

        SurveyResponseBO result = new SurveyResponseBO();

        try
            {

            Guid Id = new Guid(ResponseId);

            using (var Context = DataObjectFactory.CreateContext())
                {


                SurveyResponse Response = Context.SurveyResponses.ToList().Where(x => x.ResponseId == Id).First();
                result = (Mapper.Map(Response));

               }

            }
        catch (Exception ex)
            {
            throw (ex);
            }
        if (!string.IsNullOrEmpty(result.ParentRecordId))
                {
                return result.ParentRecordId;
                }
            else{
                return "";
                }
        
            }

        public SurveyResponseBO GetSingleResponse(string ResponseId)
            {
            SurveyResponseBO result = new SurveyResponseBO();

            try
                {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                    {


                    SurveyResponse Response = Context.SurveyResponses.ToList().Where(x => x.ResponseId == Id).First();
                    result = (Mapper.Map(Response));




                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }




            return result;
            
            }

        public List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId)
            {



            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            List<string> list = new List<string>();
            try
                {

                Guid Id = new Guid(RootId);

                using (var Context = DataObjectFactory.CreateContext())
                    {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => x.SurveyResponse1));



                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }
            return result;
            
            
            
            }

        public SurveyResponseBO GetFormResponseByParentRecordId(string ParentRecordId)
            {

            SurveyResponseBO result = new SurveyResponseBO();

            try
                {

                Guid Id = new Guid(ParentRecordId);

                using (var Context = DataObjectFactory.CreateContext())
                    {


                    var Response = Context.SurveyResponses.ToList().Where(x => x.ParentRecordId == Id);
                    if (Response.Count() > 0)
                        {
                          result = (Mapper.Map(Response.Single()));

                        }


                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }




            return result;
            }

        public List<SurveyResponseBO> GetAncestorResponseIdsByChildId(string ChildId)
            {

          List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            List<string> list = new List<string>();
            try
                {

                Guid Id = new Guid(ChildId);

                using (var Context = DataObjectFactory.CreateContext())
                    {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => Context.SurveyResponses.Where(y => x.RelateParentId == y.ResponseId)));



                    }

                }
            catch (Exception ex)
                {
                throw (ex);
                }
            return result;



            }

        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, string SurveyId) 
            {
        List<SurveyResponseBO> result = new List<SurveyResponseBO>();
           
          
            try
                {

                Guid RId = new Guid(ResponseId);
                Guid SId = new Guid(SurveyId);

                using (var Context = DataObjectFactory.CreateContext())
                    {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.RelateParentId == RId && x.SurveyId == SId )).OrderBy(x=>x.DateCreated).ToList();

                    }
                

                }
            catch (Exception ex)
                {
                throw (ex);
                }

            return result;
            
            }


        public SurveyResponseBO GetResponseXml(string ResponseId) {


        SurveyResponseBO result = new SurveyResponseBO();

        try
            {

            Guid Id = new Guid(ResponseId);

            using (var Context = DataObjectFactory.CreateContext())
                {


                var Response = Context.ResponseXmls.Where(x => x.ResponseId == Id);
                if (Response.Count() > 0)
                    {
                    result = (Mapper.Map(Response.Single()));

                    }

                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }




        return result;
            
            }


        public void DeleteResponseXml(ResponseXmlBO ResponseXmlBO)
            {


            Guid Id = new Guid(ResponseXmlBO.ResponseId);

            using (var Context = DataObjectFactory.CreateContext())
                {

                ResponseXml Response = Context.ResponseXmls.First(x => x.ResponseId == Id);
                

                Context.ResponseXmls.DeleteObject(Response);

                Context.SaveChanges();

                }
            
            }




        public void InsertResponseXml(ResponseXmlBO ResponseXmlBO)
        {

        try
            {
            using (var Context = DataObjectFactory.CreateContext())
                {
                ResponseXml ResponseXml = Mapper.ToEF(ResponseXmlBO);


                Context.AddToResponseXmls(ResponseXml);
                
                Context.SaveChanges();
                }
            }
        catch (Exception ex)
            {
            throw (ex);
            }
            
            
         }

    }

    
}
