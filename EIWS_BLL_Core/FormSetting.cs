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
    public class FormSetting
        {


        private IFormSettingDao FormSettingDao;
       


        public FormSetting(IFormSettingDao pFormSettingDao)
            {
            this.FormSettingDao = pFormSettingDao;
            
            }

        public FormSettingBO GetFormSettings(string FormId, string Xml)
            {

            FormSettingBO result = this.FormSettingDao.GetFormSettings(FormId);
            if (!string.IsNullOrEmpty(Xml))
                {
                result.FormControlNameList = GetFormColumnNames(Xml, result.ColumnNameList);
                }
            return result;
            }

        public Dictionary<int, string> GetFormColumnNames(string Xml, Dictionary<int,string> Selected)
            {
            Dictionary<int, string> List = new Dictionary<int, string>();

            XDocument xdoc = XDocument.Parse(Xml);


            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")

                                 select _FieldTypeID;
            int Count = 0;
            foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                if (!Selected.ContainsValue(_FieldTypeID.Attribute("Name").Value.ToString()) && _FieldTypeID.Attribute("FieldTypeId").Value !="2")
                    {
                    List.Add(Count, _FieldTypeID.Attribute("Name").Value.ToString());
                    Count++;
                    }
                }
            return List;

            }


        public FormSettingBO SaveSettings(Dictionary<int, string> ColumnList, bool IsDraftMode)
            {
            throw new NotImplementedException();
            }





        public string SaveSettings(bool IsDraftMode, Dictionary<int, string> ColumnNameList, Dictionary<int, string> AssignedUserList,string FormId)
            {
            string Message="";
            FormSettingBO FormSettingBO = new FormSettingBO();
            FormSettingBO.ColumnNameList = ColumnNameList;
            FormSettingBO.AssignedUserList = AssignedUserList;
            FormInfoBO FormInfoBO = new FormInfoBO();
            FormInfoBO.FormId = FormId;
            FormInfoBO.IsDraftMode = IsDraftMode;
            try
                {

                this.FormSettingDao.UpDateColumnNames(FormSettingBO, FormId);
                this.FormSettingDao.UpDateFormMode(FormInfoBO);

                //GetFormCurrentUsers
                this.FormSettingDao.UpDateAssignedUserList(FormSettingBO, FormId);
                Message = "Success";
                }
            catch (Exception Ex){
                Message = "Error";
                throw Ex;
                
                }
            return Message;
            }
        }
    }