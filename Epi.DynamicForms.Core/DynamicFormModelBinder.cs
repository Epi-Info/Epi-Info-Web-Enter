using System;
using System.Linq;
using System.Web.Mvc;
using MvcDynamicForms.Fields;
using MvcDynamicForms.Utilities;

namespace MvcDynamicForms
{
    class DynamicFormModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var postedForm = controllerContext.RequestContext.HttpContext.Request.Form;

            var form = (Form)bindingContext.Model;
            if (form == null && !string.IsNullOrEmpty(postedForm[MagicStrings.MvcDynamicSerializedForm]))
            {
                form = SerializationUtility.Deserialize<Form>(postedForm[MagicStrings.MvcDynamicSerializedForm]);
            }

            foreach (var key in postedForm.AllKeys.Where(x => x.StartsWith(form.FieldPrefix)))
            {
                string fieldKey = key.Remove(0, form.FieldPrefix.Length);
                
                try
                {
                    InputField dynField = form.InputFields.SingleOrDefault(f => f.Key == fieldKey);

                    if (dynField != default(InputField))
                    {

                        if (dynField is AutoComplete)
                        {
                            var txtField = (AutoComplete)dynField;
                            txtField.Response = postedForm[key].TrimEnd(',');
                        }

                        else if (dynField is TextField)
                        {
                            var txtField = (TextField)dynField;
                            txtField.Value = postedForm[key];
                        }

                        else if (dynField is NumericTextField)
                        {
                            var numerictxtField = (NumericTextField)dynField;
                            numerictxtField.Value = postedForm[key];
                        }

                        else if (dynField is DatePickerField)
                        {
                            //var datepickerField = (DatePickerField)dynField;
                            //datepickerField.Value = postedForm[key];
                            var datepickerField = (DatePickerField)dynField;
                            DateTime dt;
                            // datepickerField.Response = DateTimeOffset.Parse(postedForm[key]).UtcDateTime.ToString()  ;
                            var isValidDate = DateTime.TryParse(postedForm[key], out dt);
                            if (!string.IsNullOrEmpty(postedForm[key]) && isValidDate)
                            {
                                string date = DateTimeOffset.Parse(postedForm[key]).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                datepickerField.Response = date.Remove(date.IndexOf('T'));
                            }
                            else
                            {
                                datepickerField.Response = postedForm[key];
                            }
                        }
                        else if (dynField is TimePickerField)
                        {
                            var timepickerField = (TimePickerField)dynField;
                            timepickerField.Value = postedForm[key];
                        }
                       
                        else if (dynField is ListField)
                        {
                            var lstField = (ListField)dynField;

                            // clear all choice selections                    
                            foreach (string k in lstField.Choices.Keys.ToList())
                                lstField.Choices[k] = false;

                            // set current selections
                            foreach (string value in postedForm.GetValues(key))
                                lstField.Choices[value] = true;

                            lstField.Choices.Remove("");
                        }
                        else if (dynField is CheckBox)
                        {
                            var chkField = (CheckBox)dynField;

                            bool test;
                            if (bool.TryParse(postedForm.GetValues(key)[0], out test))
                            {
                                chkField.Checked = test;
                            }
                        }
                        
                        else if (dynField is MobileCheckBox)
                        {
                            var chkField = (MobileCheckBox)dynField;
                            bool test;
                            if (bool.TryParse(postedForm.GetValues(key)[0], out test))
                            {
                                chkField.Checked = test;
                            }
                        }
                             else if (dynField is AutoComplete)
                             {
                                 var AutoCompleteField = (AutoComplete)dynField;
                                 AutoCompleteField.Value = postedForm[key];
                             }
                    }
                }
                catch (System.InvalidOperationException ex)
                {

                    //continue;
                    //form.AddFields
                    //(new Field[]  
                    //    { new Hidden
                    //    {
                    //        Title = fieldKey,
                    //        Key = fieldKey,
                    //        IsPlaceHolder = true,
                    //        Value =  postedForm[key]
                    //    }
                    //    }
                    //);
                }
            }

            return form;
        }
    }
}
