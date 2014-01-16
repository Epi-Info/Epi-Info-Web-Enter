using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Interfaces.DataInterface;
namespace Epi.Web.BLL
    {
  public  class FormSetting
        {


      private IFormSettingDao FormSettingDao;



      public FormSetting(IFormSettingDao pFormSettingDao)
        {
        this.FormSettingDao = pFormSettingDao;
        }

      public FormSettingBO GetFormSettings(string FormId,string Xml)
            {
             
                FormSettingBO result = this.FormSettingDao.GetFormSettings(FormId);
                if (!string.IsNullOrEmpty(Xml))
                    {
                    result.FormControlNameList = GetFormColumnNames(Xml);
                    }
                return result;
            }

      public Dictionary<int, string> GetFormColumnNames(string Xml)
          {
          Dictionary<int, string> List = new Dictionary<int, string>();

          XDocument xdoc = XDocument.Parse(Xml);


          var _FieldsTypeIDs = from _FieldTypeID in
                                   xdoc.Descendants("Field")
                                
                               select _FieldTypeID;
          int Count = 0;
          foreach (var _FieldTypeID in _FieldsTypeIDs)
              {
              List.Add(Count, _FieldTypeID.Attribute("Name").Value.ToString());
              Count++ ;
              }
          return List;

          }
  
        }
    }
