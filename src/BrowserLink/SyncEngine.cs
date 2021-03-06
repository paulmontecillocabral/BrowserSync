﻿using System.ComponentModel.Composition;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.Web.BrowserLink;

namespace BrowserSync
{
    [Export(typeof(IBrowserLinkExtensionFactory))]
    public class SyncEngineFactory : IBrowserLinkExtensionFactory
    {
        public BrowserLinkExtension CreateExtensionInstance(BrowserLinkConnection connection)
        {
            return new SyncEngineExtension();
        }

        public string GetScript()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("BrowserSync.BrowserLink.SyncEngine.js"))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public class SyncEngineExtension : BrowserLinkExtension
    {
        private BrowserLinkConnection _connection;
        public override void OnConnected(BrowserLinkConnection connection)
        {
            _connection = connection;
            base.OnConnected(connection);
        }

        [BrowserLinkCallback] // This method can be called from JavaScript
        public void Navigate(int xpos, int ypos)
        {
            if (VSPackage.Options.EnableNavigationHotkeys && Connections.Connections.Any(c => c != _connection))
            {
                IClientInvoke others = Browsers.AllExcept(new[] { _connection });
                others.Invoke("syncNavigate", _connection.Url, xpos, ypos);
            }
        }

        [BrowserLinkCallback] // This method can be called from JavaScript
        public void FormSync(string dto)
        {
            if (VSPackage.Options.EnableFormSync && Connections.Connections.Any(c => c != _connection))
            {
                IClientInvoke others = Browsers.AllExcept(new[] { _connection });
                others.Invoke("syncForm", dto);
            }
        }
    }
}
