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
    public class NamedIntViewModel
    {
        public string Value;
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

        public NamedIntViewModel(string name, bool disabled, bool selected)
        {
            Value = string.Empty;
            Name = name.EscapeHtml();
            Disabled = disabled;
            Selected = selected;
        }

        public NamedIntViewModel(int value, string name, bool selected)
        {
            Value = value.ToString();
            Name = name.EscapeHtml();
            Selected = selected;
        }

        public NamedIntViewModel(Translator translator, Language language, bool selected)
            : this((int)language, language.Translate(translator), selected)
        {
        }
    }
}
