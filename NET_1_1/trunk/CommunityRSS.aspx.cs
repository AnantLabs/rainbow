using System;
using System.Text;
using Rainbow.Configuration;
using Rainbow.Services;
using Rainbow.Settings;
using Rainbow.UI;

namespace Rainbow
{
	/// <summary>
	/// By Jakob Hansen
	/// This class that implements the community RSS provider. The
	/// community RSS file exposes content items from this community
	/// to other community Web sites using standard RSS 0.91 file format.
	///
	/// Special XSLT files for the XmlFeed module has been made to this
	/// RSS service. Note that RSS feeds comming from this service does
	/// apply to the RSS 0.91 file format.
	///
	/// Based on CommunityRSS Class V0.2 code (03/26/2003, clauer@dotnet-fr.org)
	/// to the Community Starter Kit from www.asp.net.
	/// </summary>
	[History("jminond", "jminond@gmail.com", "1.5", "March 18, 2005", "Corrected output XML to be valid")]
	public class CommunityRSS : Page
	{
		//<asp:Literal id=RSSLiteral runat="server"></asp:Literal>
		//protected System.Web.UI.WebControls.Literal RSSLiteral;
        
		//<asp:xml id="xml1" runat="server" />
		//protected System.Web.UI.WebControls.Xml xml1;
		// Jakob Hansen: using the WebControls.Xml does work but I dont
		// want to impose extra XML stuff here - we simply dont need it!
		
		ServiceRequestInfo requestInfo;
			
		private void Page_Load(object sender, EventArgs e)
		{
			string parameterError = string.Empty;
			requestInfo = new ServiceRequestInfo();

			bool RequestInfoOK = ServiceHelper.FillRSSServiceRequestInfo(Request.Params, ref parameterError, ref requestInfo);

			// Create the response info             
			try
			{
				ServiceResponseInfo responseInfo = new ServiceResponseInfo();
				responseInfo = ServiceHelper.CallService(this.portalSettings.PortalID, -1, Path.ApplicationFullPath, ref requestInfo, this);

				if (!RequestInfoOK || requestInfo.Tag !=0)
				{
					responseInfo.ServiceStatus = string.Empty;
					if (requestInfo.Tag !=0)
						responseInfo.ServiceStatus = "ERROR! Unknown value of parameter Tag! ";
					if (!RequestInfoOK)
						responseInfo.ServiceStatus += "WARNING! Bad request. Problem with value of " + parameterError;
					responseInfo.ServiceDescription += " (" + responseInfo.ServiceStatus + ")";
				}

				StringBuilder sb;
				sb = new StringBuilder(string.Empty, 4000);
            
				// Header
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sb.Append("<!DOCTYPE rss PUBLIC \"-//Netscape Communications//DTD RSS 0.91//EN\" \"http://my.netscape.com/publish/formats/rss-0.91.dtd\">");
				sb.Append("<rss version=\"0.91\">");
				sb.Append("<channel>");
				sb.Append("<title>"+Server.HtmlEncode(responseInfo.ServiceTitle)+"</title>");
				sb.Append("<link>"+Server.HtmlEncode(responseInfo.ServiceLink)+"</link>");
				sb.Append("<description>"+Server.HtmlEncode(responseInfo.ServiceDescription)+"</description>");
				sb.Append("<image>");
				sb.Append("<title>"+Server.HtmlEncode(responseInfo.ServiceImageTitle)+"</title>");
				sb.Append("<url>"+Server.HtmlEncode(responseInfo.ServiceImageUrl)+"</url>");
				sb.Append("<link>"+Server.HtmlEncode(responseInfo.ServiceImageLink)+"</link>");
				sb.Append("<width>100</width>");
				sb.Append("<height>40</height>");
				sb.Append("</image>");
                              
				// Loop on each Item of the responseInfo collection
				foreach(ServiceResponseInfoItem srii in responseInfo.Items)
				{
					sb.Append("<item>");
					sb.Append("<title>"+Server.HtmlEncode(srii.Title)+"</title>");
					sb.Append("<link>"+Server.HtmlEncode(srii.Link)+"</link>");
					sb.Append("<description>"+Server.HtmlEncode(srii.Description)+"</description>");
					sb.Append("</item>");
				}
            
				// Footer
				sb.Append("</channel>");
				sb.Append("</rss>");

				//RSSLiteral.Text = sb.ToString();
				// Cleans the response buffer.
				Response.ClearHeaders() ;
				Response.Clear() ;

				// Prevent the browser from caching the result.
				Response.CacheControl = "no-cache" ;

				// Set the response format.
				Response.ContentEncoding	= UTF8Encoding.UTF8 ;
				Response.ContentType		= "text/xml" ;

				// Output the resulting XML.
				Response.Write(  sb.ToString() ) ;

				Response.End() ;
				//xml1.DocumentContent = sb.ToString();
			}
			catch (Exception ex)
			{
				//ErrorHandler.HandleException("FATAL ERROR IN CSS SERVICE", ex);
				ErrorHandler.Publish(LogLevel.Error, "FATAL ERROR IN CSS SERVICE", ex);

				string outText = ServiceHelper.CreateErrorRSSFeed(
					"FATAL ERROR IN CSS SERVICE! (Click here to go to Forum)", 
					"http://rainbow.duemetri.net/ASPNetForums/",
					Server.HtmlEncode("Please check the parameters. Error: " + ex.ToString()));
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}
