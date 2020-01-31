using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using BaseLibrary;
using SiteLibrary;

namespace Publicus
{
    public static class Global
    {
		private static PublicusConfig _config;
		private static Logger _logger;
		private static Mailer _mailer;
        private static SessionManager _login;
        private static SecurityThrottle _throttle;
        private static Gpg _gpg;
        private static MailCounter _mailCounter;

        public static MailCounter MailCounter
        {
            get
            {
                if (_mailCounter == null)
                {
                    _mailCounter = new MailCounter(30, 3);
                }

                return _mailCounter;
            }
        }

        public static SecurityThrottle Throttle
        {
            get
            {
                if (_throttle == null)
                {
                    _throttle = new SecurityThrottle();
                }

                return _throttle;
            }
        }

        public static SessionManager Sessions
        {
            get 
            {
                if (_login == null)
                {
                    _login = new SessionManager(); 
                }

                return _login;
            } 
        }

        public static PublicusConfig Config
		{
			get
			{
				if (_config == null)
				{
					_config = new PublicusConfig();
				}

				return _config;
			}
		}

        public static IDatabase CreateDatabase()
        {
            return new PostgresDatabase(Config.Database); 
        }

		public static Logger Log
        {
            get
            {
				if (_logger == null)
                {
					_logger = new Logger(Config.LogFilePrefix);
                }

				return _logger;
            }
        }

		public static Mailer Mail
		{
            get
            {
				if (_mailer == null)
                {
					_mailer = new Mailer(Log, Config.Mail, null);
                }

				return _mailer;
            }
        }
    }
}
