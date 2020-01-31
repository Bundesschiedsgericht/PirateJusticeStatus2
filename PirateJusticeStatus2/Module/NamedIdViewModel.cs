using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Newtonsoft.Json;
using SiteLibrary;

namespace Publicus
{
    public class NamedIdViewModel
    {
        public string Id;
        public string Name;
        public bool Disabled;
        public bool Selected;

        public string Options
        {
            get
            {
                var options = string.Empty;

                if (Disabled)
                {
                    options += " disabled"; 
                }

                if (Selected)
                {
                    options += " selected";
                }

                return options;
            }
        }

        public NamedIdViewModel(string name, bool disabled, bool selected)
        {
            Id = string.Empty;
            Name = name.EscapeHtml();
            Disabled = disabled;
            Selected = selected;
        }
    }
}
