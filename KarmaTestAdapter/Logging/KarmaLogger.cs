﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaTestAdapter.Logging
{
    public class KarmaLogger : KarmaLoggerBase
    {
        public static IKarmaLogger Create(ILogger logger = null, IMessageLogger messageLogger = null)
        {
            var karmaLogger = new KarmaLogger();
            karmaLogger.AddLogger(logger);
            karmaLogger.AddLogger(messageLogger);
            if (Globals.LogToFile)
            {
                karmaLogger.AddLogger(Globals.LogFilename);
            }
            return karmaLogger;
        }

        private readonly List<IKarmaLogger> _loggers = new List<IKarmaLogger>();

        public KarmaLogger(params IKarmaLogger[] loggers)
        {
            _loggers.ForEach(l => AddLogger(l));
        }

        public void AddLogger(IKarmaLogger logger)
        {
            if (logger != null)
            {
                _loggers.Add(logger);
            }
        }

        public void AddLogger(ILogger logger)
        {
            if (logger != null)
            {
                _loggers.Add(new KarmaExtensibilityLogger(logger));
            }
        }

        public void AddLogger(IMessageLogger logger)
        {
            if (logger != null)
            {
                _loggers.Add(new KarmaTestMessageLogger(logger));
            }
        }

        public void AddLogger(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                _loggers.Add(new KarmaFileLogger(filename));
            }
        }

        public override void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            foreach (var logger in _loggers)
            {
                try
                {
                    logger.SendMessage(testMessageLevel, message);
                }
                catch { }
            }
        }

        public override void Clear()
        {
            foreach (var logger in _loggers)
            {
                try
                {
                    logger.Clear();
                }
                catch { }
            }
        }

        public override void Log(MessageLevel messageLevel, string message)
        {
            foreach (var logger in _loggers)
            {
                try
                {
                    logger.Log(messageLevel, message);
                }
                catch { }
            }
        }
    }
}
