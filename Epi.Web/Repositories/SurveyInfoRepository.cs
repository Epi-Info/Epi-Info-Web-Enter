using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Exception;
using System.ServiceModel;
using Epi.Web.MVC.DataServiceClient;
using System.Web.Caching;
using System.Configuration;

namespace Epi.Web.MVC.Repositories
{
    public class SurveyInfoRepository : RepositoryBase, ISurveyInfoRepository
    {



        private Epi.Web.MVC.DataServiceClient.IEWEDataService _iDataService;
        private string CacheIsOn = "";
        private string IsCacheSlidingExpiration = "";
        private int CacheDuration = 0;
        public SurveyInfoRepository(Epi.Web.MVC.DataServiceClient.IEWEDataService iDataService)
        {
            _iDataService = iDataService;
            
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
            CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
              IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
        }
        
        /// <summary>
        /// Calling the proxy client to fetch a SurveyInfoResponse object
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
        {

            try
            {
                //SurveyInfoResponse result = Client.GetSurveyInfo(pRequest);
                //SurveyInfoResponse result = _iDataService.GetSurveyInfo(pRequest);
                SurveyInfoResponse result = null;
                string SurveyId = pRequest.Criteria.SurveyIdList[0].ToString();
                var CacheObj = HttpRuntime.Cache.Get(SurveyId);



                if (CacheIsOn.ToUpper() == "TRUE")
                {
                    if (CacheObj == null)
                    {
                        result = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);

                        if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                        {

                            HttpRuntime.Cache.Insert(SurveyId, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                        }
                        else
                        {
                            HttpRuntime.Cache.Insert(SurveyId, result, null, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);

                        }
                        return result;
                    }
                    else
                    {


                        return (SurveyInfoResponse)CacheObj;

                    }
                }
                else
                {
                    result = (SurveyInfoResponse)_iDataService.GetSurveyInfo(pRequest);
                    return result;
                }



                // return result;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region stubcode
            public List<Enter.Common.DTO.SurveyInfoDTO> GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public Enter.Common.DTO.SurveyInfoDTO Get(int id)
            {
                throw new NotImplementedException();
            }

            public int GetCount(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            public void Insert(Enter.Common.DTO.SurveyInfoDTO t)
            {
                throw new NotImplementedException();
            }

            public void Update(Enter.Common.DTO.SurveyInfoDTO t)
            {
                throw new NotImplementedException();
            }

            public void Delete(int id)
            {
                throw new NotImplementedException();
            } 
        #endregion


            List<SurveyInfoResponse> IRepository<SurveyInfoResponse>.GetList(Criterion criterion = null)
            {
                throw new NotImplementedException();
            }

            SurveyInfoResponse IRepository<SurveyInfoResponse>.Get(int id)
            {
                throw new NotImplementedException();
            }

            public void Insert(SurveyInfoResponse t)
            {
                throw new NotImplementedException();
            }

            public void Update(SurveyInfoResponse t)
            {
                throw new NotImplementedException();
            }


            public FormsInfoResponse GetFormsInfoList(FormsInfoRequest fRequest)
            {

                //SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest)
                //throw new NotImplementedException();
                fRequest.Criteria.UserId = 2;//Hard coded user for now
                FormsInfoResponse result;
                result = (FormsInfoResponse)_iDataService.GetFormsInfo(fRequest);
                return result;

            }


            public SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest SARequest)
            {
              return _iDataService.DeleteResponse(SARequest);
            }

            public SurveyInfoResponse GetFormChildInfo(SurveyInfoRequest SurveyInfoRequest)
                {
                   return _iDataService.GetFormChildInfo(SurveyInfoRequest);
 
                }
            public FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest pRequest)
                {

                    
                      string SurveyId = pRequest.SurveyInfo.FormId;
                      var CacheObj = HttpRuntime.Cache.Get(SurveyId);
                      FormsHierarchyResponse result = new FormsHierarchyResponse();


                      if (CacheIsOn.ToUpper() == "TRUE")
                      {
                          if (CacheObj == null)
                          {
                              result = (FormsHierarchyResponse)_iDataService.GetFormsHierarchy(pRequest);

                              if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                              {

                                  HttpRuntime.Cache.Insert(SurveyId, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                              }
                              else
                              {
                                  HttpRuntime.Cache.Insert(SurveyId, result, null, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);

                              }
                              return result;
                          }
                          else
                          {


                              return (FormsHierarchyResponse)CacheObj;

                          }
                      }
                      else
                      {
                          result = (FormsHierarchyResponse)_iDataService.GetFormsHierarchy(pRequest);
                          return result;

                      }
                }
            public SurveyAnswerResponse GetResponseAncestor(SurveyAnswerRequest SARequest)
                {

                return _iDataService.GetAncestorResponseIdsByChildId(SARequest);
                }
       public OrganizationResponse GetUserOrganizations(OrganizationRequest OrgRequest)

        {
        return _iDataService.GetUserOrganizations(OrgRequest);
        }
       public SourceTablesResponse GetSourceTables(SourceTablesRequest pRequest)
       {

           try
           {
               string SurveyId = pRequest.SurveyId + "_SourceTables";
               var CacheObj = HttpRuntime.Cache.Get(SurveyId);
               SourceTablesResponse result = new SourceTablesResponse();


               if (CacheIsOn.ToUpper() == "TRUE")
               {
                   if (CacheObj == null)
                   {
                       result = (SourceTablesResponse)_iDataService.GetDropDowns(pRequest);

                       if (IsCacheSlidingExpiration.ToUpper() == "TRUE")
                       {

                           HttpRuntime.Cache.Insert(SurveyId, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(CacheDuration));
                       }
                       else
                       {
                           HttpRuntime.Cache.Insert(SurveyId, result, null, DateTime.Now.AddMinutes(CacheDuration), Cache.NoSlidingExpiration);

                       }
                       return result;
                   }
                   else
                   {


                       return (SourceTablesResponse)CacheObj;

                   }
               }
               else
               {
                   result = (SourceTablesResponse)_iDataService.GetDropDowns(pRequest);
                   return result;

               }

           }
           catch (FaultException<CustomFaultException> cfe)
           {
               throw cfe;
           }
           catch (FaultException fe)
           {
               throw fe;
           }
           catch (CommunicationException ce)
           {
               throw ce;
           }
           catch (TimeoutException te)
           {
               throw te;
           }
           catch (Exception ex)
           {
               throw ex;
           }

       }

       public SurveyAnswerResponse GetSurveyAnswerHierarchy(SurveyAnswerRequest pRequest) {

           return _iDataService.GetSurveyAnswerHierarchy(pRequest);
       }
    }
}