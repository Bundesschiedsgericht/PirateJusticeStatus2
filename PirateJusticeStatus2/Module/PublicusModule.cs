﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nancy.Security;
using SiteLibrary;

namespace Publicus
{
    public class PublicusModule : NancyModule, IDisposable
    {
        protected IDatabase Database { get; private set; }
        protected Translation Translation { get; private set; }

        public PublicusModule()
        { 
            Database = Global.CreateDatabase();
            Translation = new Translation(Database);
        }

        protected string ReadBody()
        {
            using (var reader = new System.IO.StreamReader(Context.Request.Body))
            {
                return reader.ReadToEnd();
            }
        }

        protected byte[] GetDataUrlString(string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                var parts = stringValue.Split(new string[] { "data:", ";base64," }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    try
                    {
                        return Convert.FromBase64String(parts[1]);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void Notice(string text, params object[] parameters)
        {
            Global.Log.Notice(text, parameters);
        }

        public void Info(string text, params object[] parameters)
        {
            Global.Log.Info(text, parameters);
        }

        public void Warning(string text, params object[] parameters)
        {
            Global.Log.Warning(text, parameters);
        }

        public Session CurrentSession
        {
            get 
            {
                return Context.CurrentUser as Session;
            } 
        }

        public Negotiator AccessDenied()
        {
            return View["View/info.sshtml", new AccessDeniedViewModel(Translator)];
        }

        private static Language? ConvertLocale(string locale)
        {
            if (locale.StartsWith("de", StringComparison.InvariantCulture))
            {
                return Language.German;
            }
            else if (locale.StartsWith("fr", StringComparison.InvariantCulture))
            {
                return Language.French;
            }
            else if (locale.StartsWith("it", StringComparison.InvariantCulture))
            {
                return Language.Italian;
            }
            else if (locale.StartsWith("en", StringComparison.InvariantCulture))
            {
                return Language.English;
            }
            else
            {
                return null; 
            }
        }

        public Translator Translator
        {
            get
            {
                return new Translator(Translation, CurrentLanguage);
            }
        }

        public Translator GetTranslator(Language language)
        {
            return new Translator(Translation, language);
        }

        public string Translate(string key, string hint, string technical, params object[] parameters)
        {
            return Translation.Get(CurrentLanguage, key, hint, technical, parameters);
        }

        public void Dispose()
        {
            if (Translation != null)
            {
                Translation = null;
            }

            if (Database != null)
            {
                Database.Dispose();
                Database = null; 
            }
        }

        public Language CurrentLanguage
        {
            get
            {
                if (CurrentSession != null)
                {
                    return CurrentSession.User.Language.Value;
                }
                else
                {
                    return BrowserLanguage;
                }
            }
        }

        public Language BrowserLanguage
        {
            get
            {
                var language = Request.Headers.AcceptLanguage
                    .Select(l => new Tuple<Language?, decimal>(ConvertLocale(l.Item1), l.Item2))
                    .Where(l => l.Item1 != null)
                    .OrderByDescending(l => l.Item2)
                    .Select(l => l.Item1)
                    .FirstOrDefault();

                if (language.HasValue)
                {
                    return language.Value;
                }
                else
                {
                    return Language.English;
                }
            }
        }

        protected PostStatus CreateStatus()
        {
            return new PostStatus(Database, Translator, CurrentSession);
        }
    }
}
