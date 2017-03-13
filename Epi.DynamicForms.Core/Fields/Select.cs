using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using  System.Web;
using Epi.Core.EnterInterpreter;
using System.Drawing;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Configuration;
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html select element.
    /// </summary>
    [Serializable]
    public class Select : ListField
    { 
        /// <summary>
        /// The number of options to display at a time.
        /// </summary>
        public int Size
        {
            get
            {
                string size;
                return _inputHtmlAttributes.TryGetValue("size", out size) ? int.Parse(size) : 1;
            }
            set { _inputHtmlAttributes["size"] = value.ToString(); }
        }
        /// <summary>
        /// Determines whether the select element will accept multiple selections.
        /// </summary>
        public bool MultipleSelection
        {
            get
            {
                string multiple;
                if (_inputHtmlAttributes.TryGetValue("multiple", out multiple))
                {
                    return multiple.ToLower() == "multiple";
                }
                return false;
            }
            set { _inputHtmlAttributes["multiple"] = value.ToString(); }
        }
        private string CacheIsOn = "";
        private string IsCacheSlidingExpiration = "";
        private int CacheDuration = 0;
        public int SelectType { get; set; }
 
        /// <summary>
        /// The text to be rendered as the first option in the select list when ShowEmptyOption is set to true.
        /// </summary>
        /// 
        public string EmptyOption { get; set; }
        /// <summary>
        /// Determines whether a valueless option is rendered as the first option in the list.
        /// </summary>
        public bool ShowEmptyOption { get; set; }

        public override string RenderHtml()
        {
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
            CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
            IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
            var html = new StringBuilder();

            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            
            // prompt
            var prompt = new TagBuilder("label");

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

            string newText = regex.Replace(Prompt.Replace(" ", "&nbsp;"), "<br />");

            string NewPromp = System.Web.Mvc.MvcHtmlString.Create(newText).ToString();


            prompt.InnerHtml=NewPromp;
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);

            
            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(),_IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj

                ErrorStyle = ";border-color: red";
             
            }

            // open select element
            //if (this.CodesList != null)
            //{
            //    if (this.CodesList.Count() > 0)
            //    {
            //        string Html = "";
            //        var ScriptRelateCondition = new TagBuilder("script");
            //        foreach (var code in CodesList)
            //        {
            //            Html = "";

            //            var NewCode = Regex.Replace(code.Key.ToString(), @"[^0-9a-zA-Z]+", "");
            //            NewCode = Regex.Replace(NewCode, @"\s+", "");
            //            Html = "var " + NewCode + "=[";
            //            var json1 = JsonConvert.SerializeObject(code.Value);
            //            foreach (var item in code.Value)
            //            {
            //                var values = item.Split('=');
            //                Html = Html + "\"" + values[0] + "," + values[1].ToString().Replace("\"", "") + "\",";


            //            }
            //            Html = Html + "]; ";
            //            ScriptRelateCondition.InnerHtml = ScriptRelateCondition.InnerHtml + Html.ToString();

            //        }
            //        html.Append(ScriptRelateCondition.ToString(TagRenderMode.Normal));
            //        //var JasonObj =
            //        // var jsonSerialiser = new JavaScriptSerializer();
            //        // var json = JsonConvert.SerializeObject(CodesList);

            //    }
            //}
             TagBuilder select = null;
            if(this._choices.Count()<100){
              select = new TagBuilder("select");
            }else {
            
               select =new  TagBuilder("input");
               select.Attributes.Add("list", inputName + "_DataList");
               select.Attributes.Add("data-autofirst", "true");
            }
            select.Attributes.Add("id", inputName);
            select.Attributes.Add("name", inputName);
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {

                select.Attributes.Add("onblur", "return " + _key + "_after();"); //After
               // select.Attributes.Add("onchange", "return " + _key + "_after();"); //After
            }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {

                select.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }
            EnterRule FunctionObjectClick = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + _key);

            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
            {
                select.Attributes.Add("onclick", "return " + _key + "_click();"); //click
            }
            if (!string.IsNullOrEmpty(this.RelateCondition))
            {
              select.Attributes.Add("onchange", "return SetCodes_Val(this,'" + _form.SurveyInfo.SurveyId + "','" + _key + "');"); //click
              
            
            }
            ////////////Check code end//////////////////
            int LargestChoiseLength =0 ;
            string measureString = "";
            foreach (var choise in _choices) {

                if (choise.Key.ToString().Length > LargestChoiseLength) 
             {
                 LargestChoiseLength = choise.Key.ToString().Length;

                 measureString = choise.Key.ToString();
             } 
            
            
            }

           // LargestChoiseLength = LargestChoiseLength * _ControlFontSize;

            Font stringFont = new Font(ControlFontStyle, _ControlFontSize);

            SizeF size = new SizeF() ;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                  size = g.MeasureString(measureString.ToString(), stringFont);
            }


           
          // stringSize = (int) Graphics.MeasureString(measureString.ToString(), stringFont).Width;
        

            if (_IsRequired == true)
            {
                //awesomplete
                if (this._choices.Count() < 100)
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                        select.Attributes.Add("class", GetControlClass() + "fix-me");
                    }
                    else
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input");
                        select.Attributes.Add("class", GetControlClass());
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");

                }
                else {
                    if ((size.Width) > _ControlWidth)
                    {
                         
                        // select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                        select.Attributes.Add("class", GetControlClass() + "fix-me awesomplete");
                    }
                    else
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input");
                        select.Attributes.Add("class", GetControlClass() + " awesomplete");
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");
                
                }
            }
            else 
            {
                 //awesomplete
                if (this._choices.Count() < 100)
                {
                    //select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me ");
                    }
                    else
                    {

                        select.Attributes.Add("class", GetControlClass());
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");
                }
                else {
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me awesomplete");
                    }
                    else
                    {

                        select.Attributes.Add("class", GetControlClass() + " awesomplete");
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");
                
                
                
                }
            }
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";
            if(_IsHidden){
                IsHiddenStyle = "display:none";
            }
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }
             
            //if (_IsDisabled)
            //{
            //    select.Attributes.Add("disabled", "disabled");
            //}
            
            string InputFieldStyle = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());
            select.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px ; font-size:" + _ControlFontSize + "pt;" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";" + InputFieldStyle);
            select.MergeAttributes(_inputHtmlAttributes);
            html.Append(select.ToString(TagRenderMode.StartTag));

            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            if (this._choices.Count() > 100)
            {

                var scriptReadOnlyText = new TagBuilder("script");
                StringBuilder Script = new StringBuilder();
                Script.Append("$(window).load(function () {  ");
                //Script.Append(" $( '#" + inputName + "' ).next().css( 'width', '" + _ControlWidth.ToString() + "px' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'left', '" + _left.ToString() + "px' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'top', '" + (_top + 20).ToString() + "px' );  ");
               
                Script.Append("});");
                scriptReadOnlyText.InnerHtml = Script.ToString();
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            // initial empty option
            if (this._choices.Count() < 100)
            {
                if (ShowEmptyOption)
                {
                    var opt = new TagBuilder("option");
                    opt.Attributes.Add("value", null);
                    opt.SetInnerText(EmptyOption);
                    html.Append(opt.ToString());
                }
            }
            // options

            // Build codes RelateCondition script object 


            if (this._choices.Count() < 100 && this.SelectType.ToString() != "18")
            {
                switch (this.SelectType.ToString() )
                {
                    case "11":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            var optSelectedVale = "";
                            if (!string.IsNullOrEmpty(SelectedValue.ToString()))
                            {
                                optSelectedVale = SelectedValue.ToString();//=="1"? "Yes" : "No";
                            }
                            opt.Attributes.Add("value", (choice.Key == "Yes" ? "1" : "0"));
                            if (choice.Key == optSelectedVale.ToString())
                            {
                                opt.Attributes.Add("selected", "selected");

                            }
                            if (choice.Key == "Yes" || choice.Key == "No")
                            {
                                opt.SetInnerText(choice.Key);
                                html.Append(opt.ToString());
                            }

                        }
                        break;
                    case "17":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            opt.Attributes.Add("value", choice.Key);
                            if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                            opt.SetInnerText(choice.Key);
                            html.Append(opt.ToString());
                        }

                        break;
                    case "18":


                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            opt.Attributes.Add("value", choice.Key);
                            if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                            {
                                opt.SetInnerText(choice.Key);
                            }
                            html.Append(opt.ToString());
                        }



                        break;
                    case "19":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");

                            if (choice.Key.Contains("-"))
                            {
                                string[] keyValue = choice.Key.Split(new char[] { '-' }, 2);
                                string comment = keyValue[0].Trim();
                                string description = keyValue[1].Trim();

                                opt.Attributes.Add("value", comment);

                                if (choice.Value || comment == SelectedValue.ToString())
                                {
                                    opt.Attributes.Add("selected", "selected");
                                }

                                opt.SetInnerText(description);
                            }

                            html.Append(opt.ToString());
                        }
                        break;
                }
            }
            else { 
            ///////////////////////////

                var datalist = new TagBuilder("datalist ");
                datalist.Attributes.Add("id", inputName + "_DataList");
                html.Append(datalist.ToString(TagRenderMode.StartTag));
            foreach (var choice in _choices)
            {
                var opt = new TagBuilder("option");
                opt.Attributes.Add("value", choice.Key);
                if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                {
                    opt.SetInnerText(choice.Key);
                }
                 html.Append(opt.ToString());
            }

           
                 
            
            ///////////////////////////
            }
 
            // close select element
            html.Append(select.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent for select tags
            var hidden1 = new TagBuilder("input");
            hidden1.Attributes.Add("type", "hidden");
            hidden1.Attributes.Add("id", inputName + "_hidden");
            hidden1.Attributes.Add("name", inputName);
            hidden1.Attributes.Add("value", string.Empty);
            html.Append(hidden1.ToString(TagRenderMode.SelfClosing));

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            if (_IsHidden)
            {
                wrapper.Attributes["style"] = "display:none";

            }
            wrapper.Attributes["id"] = inputName + "_fieldWrapper";
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }

        
        public string GetControlClass()
        {

            StringBuilder ControlClass = new StringBuilder();

            ControlClass.Append("validate[");


            if (_IsRequired == true)
            {

                ControlClass.Append("required");

            }
            ControlClass.Append("]");

            return ControlClass.ToString();

        }
    }
}
