using Epi.Web.Enter.Common.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Enter.Interfaces.DataInterfaces
{
    public interface IReportDao
    {
        void PublishReport(ReportInfoBO Report);
        List<ReportInfoBO> GetSurveyReports(string surveyID,bool IncludHTML);
        List<ReportInfoBO> GetReport(string reportID);
        void DeleteReport(ReportInfoBO Report);
    }
}
