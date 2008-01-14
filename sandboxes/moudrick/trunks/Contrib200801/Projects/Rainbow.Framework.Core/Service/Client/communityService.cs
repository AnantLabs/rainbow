using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Rainbow.Framework.Services.Client
{
	/// <summary>
    ///------------------------------------------------------------------------------
    /// <autogenerated>
    ///     Few changes by Jakob Hansen:
    ///        1) Added "namespace Rainbow.Services.Client"
    ///        2) Changed class name CommunityService to communityService to avoid name
    ///           name clash with CommunityService in file CommunityService.asmx.cs
    ///        3) Outcommented code at the bottom of the file (we get it via Rainbow.Services)
    ///     This code was generated by a tool.
    ///     Runtime Version: 1.0.3705.288
    ///
    ///     Changes to this file may cause incorrect behavior and will be lost if 
    ///     the code is regenerated.
    /// </autogenerated>
    ///------------------------------------------------------------------------------
    /// 
    /// This source code was auto-generated by wsdl, Version=1.0.3705.288.
	/// </summary>
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "CommunityServiceSoap", Namespace = "http://tempuri.org/")]
	[XmlInclude(typeof(ServiceResponseInfoItem))]
	[XmlInclude(typeof(object[]))]
	public class communityService : SoapHttpClientProtocol
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="communityService"/> class.
		/// </summary>
		/// <remarks/>
		public communityService()
		{
		    Url = "http://localhost/rainbow/CommunityService.asmx";
		}

		/// <summary>
		/// Gets the content of the community.
		/// </summary>
		/// <param name="requestInfo">The request info.</param>
		/// <returns></returns>
		/// <remarks/>
		[SoapDocumentMethod("http://tempuri.org/GetCommunityContent", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public ServiceResponseInfo GetCommunityContent(ServiceRequestInfo requestInfo)
		{
			object[] results = Invoke("GetCommunityContent", new object[] { requestInfo });
			return ((ServiceResponseInfo)(results[0]));
		}

		/// <summary>
		/// Begins the content of the get community.
		/// </summary>
		/// <param name="requestInfo">The request info.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		/// <remarks/>
		public IAsyncResult BeginGetCommunityContent(ServiceRequestInfo requestInfo, AsyncCallback callback, object asyncState)
		{
			return BeginInvoke("GetCommunityContent", new object[] { requestInfo }, callback, asyncState);
		}

		/// <summary>
		/// Ends the content of the get community.
		/// </summary>
		/// <param name="asyncResult">The async result.</param>
		/// <returns></returns>
		/// <remarks/>
		public ServiceResponseInfo EndGetCommunityContent(IAsyncResult asyncResult)
		{
			object[] results = EndInvoke(asyncResult);
			return ((ServiceResponseInfo)(results[0]));
		}
	}

	/* We got this by doing the "namespace Rainbow.Services.Client" above!
	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
	public class ServiceRequestInfo 
	{
    
		/// <remarks/>
		public ServiceType Type;
    
		/// <remarks/>
		public string Url;
    
		/// <remarks/>
		public string PortalAlias;
    
		/// <remarks/>
		public bool LocalMode;
    
		/// <remarks/>
		public string UserName;
    
		/// <remarks/>
		public string UserPassword;
    
		/// <remarks/>
		public ServiceListType ListType;
    
		/// <remarks/>
		public string ModuleType;
    
		/// <remarks/>
		public int MaxHits;
    
		/// <remarks/>
		public bool ShowID;
    
		/// <remarks/>
		public string SearchString;
    
		/// <remarks/>
		public string SearchField;
    
		/// <remarks/>
		public string SortField;
    
		/// <remarks/>
		public string SortDirection;
    
		/// <remarks/>
		public bool RootLevelOnly;
    
		/// <remarks/>
		public bool MobileOnly;
    
		/// <remarks/>
		public string IDList;
    
		/// <remarks/>
		public ServiceListType IDListType;
    
		/// <remarks/>
		public int Tag;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
	public enum ServiceType 
	{
    
		/// <remarks/>
		Unknown,
    
		/// <remarks/>
		CommunityWebService,
    
		/// <remarks/>
		CommunityRSSService,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
	public enum ServiceListType 
	{
    
		/// <remarks/>
		Item,
    
		/// <remarks/>
		Module,
    
		/// <remarks/>
		Tab,
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
	public class ServiceResponseInfoItem 
	{
    
		/// <remarks/>
		public string Link;
    
		/// <remarks/>
		public string Title;
    
		/// <remarks/>
		public string Description;
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
	public class ServiceResponseInfo 
	{
    
		/// <remarks/>
		public string ServiceStatus;
    
		/// <remarks/>
		public string ServiceTitle;
    
		/// <remarks/>
		public string ServiceLink;
    
		/// <remarks/>
		public string ServiceDescription;
    
		/// <remarks/>
		public string ServiceCopyright;
    
		/// <remarks/>
		public string ServiceImageTitle;
    
		/// <remarks/>
		public string ServiceImageUrl;
    
		/// <remarks/>
		public string ServiceImageLink;
    
		/// <remarks/>
		public object[] Items;
	}
	*/
}
