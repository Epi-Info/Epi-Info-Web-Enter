using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Models;

namespace Epi.Web.MVC.Utility
{
    public class CssClassProvider
    {
        public List<FormInfoModel> GetFormInfoCssClass(List<FormInfoModel> forms) 
        {
            for (int i = 0; i < forms.Count; i++)
            {
                if (forms[i].IsSelected)
                {
                    forms[i].CssClassName = "metro-tile metro-design metro-staging metro-set";
                }
                else
                {
                    if (forms[i].IsDraftMode)
                    {
                        if (forms[i].IsOwner)
                        {
                            forms[i].CssClassName = "metro-tile metro-design metro-staging";
                        }
                        else
                        {
                            forms[i].CssClassName = "metro-tile metro-collect metro-staging";
                        }
                    }
                    else
                    {
                        if (forms[i].IsOwner)
                        {
                            forms[i].CssClassName = "metro-tile metro-design metro-prod";
                        }
                        else
                        {
                            forms[i].CssClassName = "metro-tile metro-collect metro-prod";
                        }
                    }
                        
                }
                
            }
            return forms;
        }
    }
}